namespace HealthInvoice.Core.Interfaces.Services.Invoices.Management;

/// <summary>
/// Сервис для доступа к XML‑схемам (XSD) в виде потоков данных.
/// Предоставляет возможность открывать потоки для чтения схем по их имени,
/// что позволяет использовать их для валидации XML‑файлов или других целей.
/// </summary>
public interface ISchemaSource
{
    /// <summary>
    /// Открывает поток для чтения XML‑схемы (XSD) по указанному имени.
    /// </summary>
    /// <param name="schemaName">
    /// Имя схемы, которую необходимо открыть. 
    /// </param>
    /// <param name="cancellationToken">
    /// Токен отмены асинхронной операции. 
    /// </param>
    /// <returns>
    /// Поток (Stream), содержащий данные XML‑схемы.
    /// Вызывающий код обязан закрыть поток после использования (рекомендуется использовать конструкцию using).
    /// Позиция потока установлена в начало (Position = 0).
    /// </returns>
    Stream OpenSchemaStream(
        string schemaName,
        CancellationToken cancellationToken = default);
}
