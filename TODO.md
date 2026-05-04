# HealthInvoice Hub — подробные рекомендации по рефакторингу

> Документ сфокусирован на практическом техдолге после выхода в тестовый релиз.
> Приоритет: надежность фоновой обработки, предсказуемые статусы, транзакционная целостность и эксплуатационная безопасность.

---

## 1) Ключевые риски (что исправлять в первую очередь)

### R1. Неполная «машина состояний» статусов ЛК
Сейчас статус переводится в `Processing` при постановке в очередь, но нет гарантированного финализатора для каждого счета в воркере (`Done/Failed`).

**Риски:**
- «Зависшие» счета в `Processing`.
- Нет однозначного SLA по завершению МЭК.
- Сложно объяснять пользователю «что произошло». 

**Цель:**
- Ввести строгий конечный автомат статусов для каждого `schetUid`:
  - `Queued`
  - `Processing`
  - `Done`
  - `Failed`
  - `Retrying` (опционально)

---

### R2. "Молчаливое" падение SQL `EXEC`
Вызов процедуры через `ExecuteSqlRawAsync` не всегда гарантирует исключение при бизнес-ошибке внутри SQL (например, ошибка обработана в `TRY/CATCH` и не `THROW`).

**Риски:**
- Приложение считает операцию успешной, хотя процедура завершилась неуспешно.
- Статусы расходятся с фактическим состоянием в БД.

**Цель:**
- Для всех критичных `EXEC` ввести единый контракт: `@pRetCode`, `@pRetMessage`, `RETURN`.
- Считать выполнение успешным только если реткод = 0.

---

### R3. Неатомарный `RewriteInvoice`
`RewriteInvoice` сейчас логически объединяет удаление+вставку, но в текущей реализации шаги создают отдельные контексты/транзакции.

**Риски:**
- Возможны частичные состояния (удалили, но не вставили / вставили частично).
- Непредсказуемость при падениях между шагами.

**Цель:**
- Переписать `RewriteInvoice` на единый `DbContext` + единую транзакцию + общий cancellation token.

---

### R4. Неограниченные очереди (`Channel.CreateUnbounded`)
При всплеске входящих запросов канал может расти без ограничений.

**Риски:**
- Рост памяти процесса.
- Ухудшение времени отклика и нестабильность сервиса.

**Цель:**
- Перейти на bounded channel + backpressure + telemetry по длине очереди.

---

### R5. Эксплуатационная безопасность
Секреты подключения и настройки cookie требуют ужесточения перед prod.

**Цель:**
- Вынести секреты в переменные окружения / secret storage.
- Включить `CookieSecurePolicy.Always` и HTTPS-only traffic.

---

## 2) Целевое состояние архитектуры

### 2.1. Pipeline (упрощенная схема)
1. Upload → форматный контроль (`Integrity` + `Compliance`)  
2. Запись результата в `JOURNAL_FK`  
3. Upsert в очередь БД (insert/update)  
4. Постановка в очередь МЭК (logic control)  
5. Worker выполняет SQL-процедуру с валидацией retcode  
6. Worker **обязательно** завершает статус счета (`Done/Failed`)  
7. Watchdog обрабатывает «зависшие» записи

---

### 2.2. Принципы
- Один счет — один жизненный цикл статуса.
- Любой фоновый шаг обязан иметь **idempotency**.
- Любой SQL-вызов обязан иметь **явный бизнес-результат**, а не только факт отсутствия exception.
- Любой воркер обязан иметь: retry policy, DLQ/parking, метрики, трассировку.

---

## 3) Детальный план рефакторинга (с приоритетами)

## P0 — критично (делать первым)

### P0.1. Ввести конечные статусы и централизованный статус-сервис
Создать `IInvoiceStatusService` с методами:
- `MarkQueued(List<int> schetUids, JournalType jt)`
- `MarkProcessing(int schetUid, JournalType jt)`
- `MarkDone(int schetUid, JournalType jt)`
- `MarkFailed(int schetUid, JournalType jt, string reason, bool transient)`

**Почему:** убирает разрозненные обновления статусов по проекту.

---

### P0.2. Переписать воркер ЛК на «безусловную финализацию»
Для каждого item:
- try: `MarkProcessing` → выполнить МЭК → `MarkDone`
- catch: лог+причина → `MarkFailed`

**Важно:** исключение конкретной задачи не должно «ронять» весь worker pipeline.

**Рекомендация:**
- Ловить исключения внутри `ProcessItemAsync`.
- Хранить correlation fields: `SchetUid`, `JournalType`, `TraceId`.

---

### P0.3. SQL contract для всех критичных `EXEC`
Для процедур `sp26_check_preparation`, `sp26_delbyschetuid` (и других критичных):
- договориться о параметрах:
  - `@pRetCode INT OUTPUT`
  - `@pRetMessage NVARCHAR(1024) OUTPUT`
- если `@pRetCode <> 0` → бросать доменное исключение `StoredProcedureBusinessException`.

**Пример результата в коде:**
- `retCode == 0` -> success
- `retCode in (known business codes)` -> controlled failed status
- SQL exception -> technical failed status

---

### P0.4. Атомарный Rewrite (delete+insert)
Сконцентрировать delete/insert в одном методе репозитория с одним `DbContext` и одной `BeginTransactionAsync`.

**Шаблон:**
- begin tx
- execute delete SP
- insert entities
- insert/update journal row
- commit
- rollback on any error

---

### P0.5. Watchdog для зависших статусов
Новый hosted service (каждые 5-10 минут):
- найти записи в `Processing` старше SLA (например, 30/60 мин)
- поставить `Failed` + reason=`Timeout/WorkerInterruption`
- (опционально) перекинуть в retry очередь

---

## P1 — высокая важность

### P1.1. Ограничить очереди (BoundedChannel)
Пример политики:
- емкость: 1_000 или 5_000
- full mode: `Wait` (backpressure)

Добавить метрики:
- queue length
- enqueue latency
- dequeue latency
- item processing duration
- fail/success counters

---

### P1.2. Политика retry
Для transient-ошибок SQL (deadlock, timeout):
- экспоненциальная пауза + jitter
- max attempts (например, 3)
- после исчерпания — `Failed` и в parking/DLQ

---

### P1.3. Репозиторный слой: разделить технические и доменные исключения
Сейчас в целом логируется корректно, но полезно стандартизировать типы:
- `InfrastructureException` (network/db)
- `StoredProcedureBusinessException` (retCode business fail)
- `InvoiceStateTransitionException` (недопустимый переход статуса)

---

### P1.4. Нормализовать числовые статусы в enum
Сейчас встречаются «магические числа» (`4`, `2`, `1`, `0`).

Рекомендация:
- вынести в `enum InvoiceProcessingStatus : short`
- использовать enum в контроллере/репозитории/сервисах
- централизовать mapping с SQL статусами

---

## P2 — средний приоритет

### P2.1. Усилить observability (Serilog + correlation)
В каждый лог добавлять:
- `SchetUid`
- `JournalType`
- `ArchiveFilename`
- `Operation`
- `ElapsedMs`

Добавить dashboard-показатели:
- количество `Processing > SLA`
- доля `Failed`
- P95/P99 времени обработки счета

---

### P2.2. Безопасность конфигов
- Убрать чувствительные строки из `appsettings.json`.
- Поддержать env vars:
  - `ConnectionStrings__...`
- Добавить шаблон `appsettings.Production.json` без секретов.

---

### P2.3. Сессионная безопасность
Перед продом:
- `CookieSecurePolicy.Always`
- `SameSite` по сценариям фронта (Lax/Strict)
- HTTPS termination + HSTS

---

### P2.4. Техническая чистка
- Устранить опечатки/encoding-артефакты в лог-сообщениях.
- Вычистить copy-paste сообщения (`journal_flk` vs `journal_fk`/`journal_lk`).
- Проверить нейминг и консистентность комментариев.

---

## 4) Как закрыть вопрос «EXEC молча упал» (практика)

## 4.1. SQL-side
Для каждой critical SP:
1. В конце всегда выставлять `@pRetCode`, `@pRetMessage`.
2. В `CATCH`:
   - писать тех. деталь в лог SQL (если есть)
   - выставлять `@pRetCode != 0`, `@pRetMessage`
   - `THROW` при фатальной ошибке.

---

## 4.2. .NET-side
Создать helper-обертку `ExecuteStoredProcedureWithResultAsync(...)`:
- принимает sql + параметры
- возвращает typed result `{ RetCode, RetMessage }`
- при `RetCode != 0` бросает `StoredProcedureBusinessException`

Плюсы:
- единая обработка на всех репозиториях
- меньше дублирования
- проще тестировать

---

## 5) Тестовая стратегия после рефакторинга

## 5.1. Unit
- Проверка переходов статусов (`Queued -> Processing -> Done`, `Processing -> Failed`)
- Проверка недопустимых переходов
- Проверка retry policy

## 5.2. Integration
- Имитация `retCode != 0` из SP
- Имитация timeout/deadlock
- Проверка, что статус всегда финализирован

## 5.3. E2E
- Пакет из N счетов одновременно
- Принудительный fault одной части
- Проверка: остальные не зависли, очередь не «взорвалась», статусы корректны

---

## 6) План внедрения по неделям (прагматичный)

### Неделя 1
- Enum статусов + `IInvoiceStatusService`
- Финализация статусов в `InvoicesLogicalControlQueueProcessor`
- Базовые метрики обработки

### Неделя 2
- SQL contract (`retCode/retMessage`) для 1-2 критичных SP
- Общий helper для EXEC
- Интеграционные тесты на бизнес-ошибки SQL

### Неделя 3
- Атомарный `RewriteInvoice`
- bounded channels + backpressure
- watchdog для «зависших» Processing

### Неделя 4
- security hardening (секреты, cookie, prod-конфиг)
- observability dashboard
- load test/regression

---

## 7) Definition of Done (DoD)

Фича считается завершенной, если:
1. Любой счет после запуска ЛК получает финальный статус (`Done/Failed`) в пределах SLA.
2. Нет «вечных» Processing.
3. При `retCode != 0` от SQL статус корректно переходит в Failed и причина видна пользователю/оператору.
4. `RewriteInvoice` атомарен и не оставляет БД в частичном состоянии.
5. Очередь ограничена, есть метрики глубины и времени обработки.
6. Секреты не хранятся в репозитории.

---

## 8) Быстрые wins (можно начать сегодня)

1. В воркере ЛК добавить `try/catch/finally` с `UpdateInvoiceStatusAsync`.
2. Завести enum статусов вместо литералов `4/2/1/0`.
3. Для `sp26_check_preparation` добавить output реткод/сообщение и проверку на стороне C#.
4. Поставить bounded channel хотя бы для логконтроля.
5. Вынести connection strings в env vars для тест/прод окружений.

---

## 9) Примечание

Текущая база уже достаточно зрелая для тестового контура. Этот план не «переписывает все», а делает систему устойчивой к сбоям, понятной в эксплуатации и предсказуемой для пользователей.
