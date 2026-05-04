using System.Threading.Channels;

namespace HealthInvoice.Core.Interfaces.Services.Invoices.Publishers;

/// <summary>
/// Сервис для публикации DTO‑объектов в очередь, обрабатываемую фоновым сервисом (Background Service).
/// Использует Channel для безопасной асинхронной передачи данных между компонентами приложения.
/// </summary>
/// <typeparam name="T">Тип DTO‑объекта, передаваемого через очередь.</typeparam>
public interface IQueuePublisher<T>
{
    /// <summary>
    /// Асинхронно публикует коллекцию объектов в указанную очередь.
    /// </summary>
    /// <param name="items">Список объектов типа T, которые необходимо поместить в очередь для последующей обработки.</param>
    /// <param name="queueName">
    /// Имя очереди, в которую будут отправлены объекты.
    /// Если не указано, используется очередь по умолчанию.
    /// </param>
    /// <returns>Задача, представляющая асинхронную операцию публикации.</returns>
    /// <exception cref="ArgumentNullException">Если параметр items равен null.</exception>
    /// <exception cref="InvalidOperationException">
    /// Если канал (Channel) недоступен или находится в состоянии завершения записи.
    /// </exception>
    /// <remarks>
    /// Метод добавляет все элементы коллекции items в канал.
    /// Гарантируется, что объекты будут обработаны фоновым сервисом в порядке поступления (FIFO),
    /// если очередь не настроена иначе.
    /// </remarks>
    Task PublishAsync(List<T> items, string queueName = "", CancellationToken cancellationToken = default);


    /// <summary>
    /// Предоставляет доступ к считывателю канала (ChannelReader) для чтения объектов из очереди.
    /// Используется фоновым сервисом для получения данных из очереди.
    /// </summary>
    /// <value>
    /// Объект ChannelReader, позволяющий асинхронно считывать элементы типа T из канала.
    /// Через этот считыватель фоновый сервис получает объекты, опубликованные методом PublishAsync.
    /// </value>
    /// <remarks>
    /// ChannelReader обеспечивает потокобезопасное чтение данных из канала.
    /// Фоновый сервис может использовать методы ReadAsync или TryRead для получения элементов.
    /// </remarks>
    ChannelReader<T> Reader { get; }
}
