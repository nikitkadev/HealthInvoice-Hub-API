using HealthInvoice.Core.Dtos.Invoices;
using HealthInvoice.Core.Entities.Invoices.Main.H;
using HealthInvoice.Core.Entities.Invoices.Main.L;
using System.Text.RegularExpressions;

namespace HealthInvoice.Core.Common;

/// <summary>Класс для расширений, касаемых имен входящих файлов.</summary>
public static partial class FilenameExtension
{
    /// <summary>
    /// Регулярное выражание с паттерном имени архива для счетов СМО РХ.
    /// </summary>
    [GeneratedRegex(@"^NM(\d{6})S\d{5}_\d{4}.*\.zip$")]
    private static partial Regex SmorxRegex();

    /// <summary>
    /// Регулярное выражание с паттерном имени архива для иногородних счетов.
    /// </summary>
    [GeneratedRegex(@"^NM(\d{6})T19_\d{4}.*\.zip$")]
    private static partial Regex InogorodRegex();

    /// <summary>
    /// Метод проверки имени архива на соответствие установленным стандартам.
    /// </summary>
    /// <param name="archiveName">Имя архива</param>
    /// <param name="codeSendingOrg">Код организации, которая отправила архив.</param>
    /// <param name="journalType">Тип журнала, для которого был отправлен архив.</param>
    /// <returns>Развернутый тип Result с результатом проверки имени архива.</returns>
    public static Result HasCorrectArchiveName(
        this string archiveName,
        string codeSendingOrg,
        JournalType journalType)
    {
        var errorId = Guid.NewGuid();

        var regex = journalType == JournalType.SMORX ? SmorxRegex() : InogorodRegex();
        var match = regex.Match(archiveName);

        if (!match.Success)
        {
            var errorMessage = $"Имя архива не соответствует установленным стандартам\n" +
                           $"Уникальный идентификатор ошибки: {errorId}";

            return Result.Fail(errorMessage, errorId);
        }

        var orgCodeInArchiveName = match.Groups[1].Value;

        if (orgCodeInArchiveName != codeSendingOrg && codeSendingOrg != OrganizationConstants.AdminOrgCode)
        {
            var errorMessage = $"Код, указанный в имени файла ({orgCodeInArchiveName}) " +
                           $"не соответствует коду вашей организации ({codeSendingOrg})\n" +
                           $"Уникальный идентификатор ошибки: {errorId}";

            return Result.Fail(errorMessage, errorId);
        }

        return Result.Success();
    }
}

/// <summary>Класс для расширений, касаемых нормализации отправленных счетов.</summary>
public static class InvoiceStructureExtension
{
    /// <summary>Нормализация сущности счета в связи с рукожопием тех, кто составлял структуру БД.</summary>
    /// <param name="invoices">Кортеж сущностей двух счетов.</param>
    /// <returns>Нормализованная форма счетов в виде кортежа.</returns>
    public static Result<(ZlListEntity, PersListEntity)> Normalize(this (ZlListEntity, PersListEntity) invoices)
    {
        if (invoices.Item1 is null)
            return Result<(ZlListEntity, PersListEntity)>
                .Fail("Переданная сущность ZL_LIST_ENTITY является null.", Guid.NewGuid());

        foreach (var zap in invoices.Item1.Zaps)
        {
            if (zap.ZSl is null)
                return Result<(ZlListEntity, PersListEntity)>.Success(invoices);

            if (zap.ZSl.Sls is null)
                return Result<(ZlListEntity, PersListEntity)>.Success(invoices);

            foreach (var sl in zap.ZSl.Sls)
            {
                NormalizeDs2Entity(sl);
                NormalizeDs3Entity(sl);
                NormalizeDateInjectionEntity(sl);
                NormalizeCritEntity(sl);
            }
        }

        return Result<(ZlListEntity, PersListEntity)>.Success(invoices);
    }

    /// <summary>Нормализация DS2 сущности.</summary>
    /// <param name="entity">Переданная сущность случая.</param>
    private static void NormalizeDs2Entity(SlEntity entity)
    {
        if (entity.Ds2 is null)
            return;

        foreach (var DS2 in entity.Ds2)
            entity.Ds2s.Add(new() { DS2 = DS2 });
    }

    /// <summary>Нормализация DS3 сущности.</summary>
    /// <param name="entity">Переданная сущность случая.</param>
    private static void NormalizeDs3Entity(SlEntity entity)
    {
        if (entity.Ds3 is null)
            return;

        foreach (var DS3 in entity.Ds3)
            entity.Ds3s.Add(new() { DS3 = DS3 });
    }

    /// <summary>Нормализация CRIT сущности.</summary>
    /// <param name="entity">Переданная сущность случая.</param>
    private static void NormalizeCritEntity(SlEntity entity)
    {
        if (entity.KsgKpg is null || entity.KsgKpg.Crits is null || entity.KsgKpg.Crits.Count == 0)
            return;

        foreach (var crit in entity.KsgKpg.Crits)
            entity.KsgKpg.CritEntities.Add(new() { Crit = crit });
    }

    /// <summary>Нормализация DATE_INJ сущности.</summary>
    /// <param name="entity">Переданная сущность случая.</param>
    private static void NormalizeDateInjectionEntity(SlEntity entity)
    {
        if (entity.OnkSl is null || entity.OnkSl.OnkUsls is null || entity.OnkSl.OnkUsls.Count == 0)
            return;

        foreach (var onkUsl in entity.OnkSl.OnkUsls)
        {
            if (onkUsl.LekPrs is null || onkUsl.LekPrs.Count == 0)
                return;

            foreach (var lekPr in onkUsl.LekPrs)
            {
                if (lekPr.DateInjs is null || lekPr.DateInjs.Count == 0)
                    return;

                foreach (var dateInj in lekPr.DateInjs)
                {
                    lekPr.DateInjEntities.Add(new() { DateInj = dateInj });
                }
            }
        }
    }
}

public static class ErrorsExtenstion
{
    public static void FillError(
        this IntegrityValidationResult result,
        string errorMessage)
    {
        result.IsValid = false;
        result.ClientErrorMessage = errorMessage;
    }
}