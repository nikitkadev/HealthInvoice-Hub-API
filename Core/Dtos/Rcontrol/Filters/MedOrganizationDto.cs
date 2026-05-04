namespace HealthInvoice.Core.Dtos.Rcontrol.Filters;

/// <summary>Транспортный класс для передачи на фронтенд информации о медицинской организации.</summary>
public class MedOrganizationDto
{
    /// <summary>Код медицинской организации.</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Имя медицинской организации.</summary>
    public string Name { get; set; } = string.Empty;
}