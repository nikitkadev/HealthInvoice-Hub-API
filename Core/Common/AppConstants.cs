using System.Dynamic;

namespace HealthInvoice.Core.Common;

/// <summary>Класс, содержащий константы под параметры организаций.</summary>
public static class OrganizationConstants
{
    /// <summary>Код организации ТФОМС РХ</summary>
    public const string AdminOrgCode = "19000";
}

/// <summary>Класс, содержащий константы для файловых операций.</summary>
public static class FileConstants
{
    /// <summary>Максимальный суммарный размер всех файлов (100 МБ)</summary>
    public const int FilesSizeLimit = 100 * 1024 * 1024;

    /// <summary>Максимальный размер одного файла (5 МБ)</summary>
    public const int FileSizeLimit = 5 * 1024 * 1024;

    /// <summary> Полное имя файла H-схемы.</summary>
    public const string H_SchemaName = "H.xsd";

    /// <summary>Полное имя файла L-схемы.</summary>
    public const string L_SchemaName = "L.xsd";
}

/// <summary>Класс, содержащий служебные константы.</summary>
public static class ServiceConstants
{
    /// <summary>Количество повторных вызывов процессов.</summary>
    public const int MaxRetries = 3;

    /// <summary>Максимальное количество отображаемых ошибок ФЛК в браузере</summary>
    public const int MaxDefectsCount = 500;
}

/// <summary>Класс, содержащий константы кодов различных сущностей (код ошибок и прочее)</summary>
public static class CodeConstants
{
    /// <summary>Код ошибки дедлока при выполнение хранимой процедуры.</summary>
    public const int SqlExceptionDeadlockCode = 1205;

    /// <summary>Код ошибки таймаута.</summary>
    public const int SqlExceptionTimeoutCode = 1222;
}