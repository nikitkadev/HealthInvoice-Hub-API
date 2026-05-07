namespace HealthInvoice.Core.Dtos.Rcontrol.Categories;

/// <summary>
/// Сводная информация по счёту (суммы, санкции, принято).
/// </summary>
public class InvoiceSummaryDto
{
    /// <summary>Имя файла счёта.</summary>
    public string Filename { get; set; } = string.Empty;

    /// <summary>Уникальный идентификатор счёта.</summary>
    public int SchetUid { get; set; }

    /// <summary>Дата загрузки счёта.</summary>
    public DateTime UploadDate { get; set; }


    /// <summary>Предъявлено к оплате (summav).</summary>
    public decimal Summav { get; set; }

    /// <summary>Принято ТФОМС (summap).</summary>
    public decimal Summap { get; set; }


    /// <summary>Снято по МЭК (sank_mek).</summary>
    public decimal SankMek { get; set; }

    /// <summary>Снято по МЭЭ (sank_mee).</summary>
    public decimal SankMee { get; set; }

    /// <summary>Снято по ЭКМП (sank_ekmp).</summary>
    public decimal SankEkmp { get; set; }


    /// <summary>Принято СМО (smo_summap).</summary>
    public decimal SmoSummap { get; set; }

    /// <summary>Снято СМО по МЭК (smo_sank_mek).</summary>
    public decimal SmoSankMek { get; set; }

    /// <summary>Снято СМО по МЭЭ (smo_sank_mee).</summary>
    public decimal SmoSankMee { get; set; }

    /// <summary>Снято СМО по ЭКМП (smo_sank_ekmp).</summary>
    public decimal SmoSankEkmp { get; set; }
}