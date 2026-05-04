namespace HealthInvoice.Core.Interfaces.Services.Invoices.Mapping;

/// <summary>
/// Универсальный интерфейс для преобразования объектов одного типа в другой.
/// Позволяет реализовать типобезопасное преобразование данных между различными DTO и моделями.
/// </summary>
/// <typeparam name="TIn">Тип входных данных для преобразования.</typeparam>
/// <typeparam name="TOut">Тип выходных данных после преобразования.</typeparam>
public interface IMapper<TIn, TOut>
{
    /// <summary>
    /// Преобразует объект или коллекцию объектов типа TIn в объект типа TOut.
    /// </summary>
    /// <param name="input">Входные данные для преобразования.</param>
    /// <returns>Преобразованный объект целевого типа.</returns>
    TOut MapTo(TIn input);
}
