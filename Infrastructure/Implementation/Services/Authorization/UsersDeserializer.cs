using System.Text.Json;

using Microsoft.Extensions.Logging;

using HealthInvoice.Core.Dtos.Auth;
using HealthInvoice.Core.Interfaces.Services.Authorization;

namespace HealthInvoice.Infrastructure.Implementation.Services.Authorization;

public class UsersDeserializer(
    ILogger<UsersDeserializer> logger) : IUsersDeserializer
{
    public List<UserJsonConvertDto> DeserializeUsers(Stream jsonWithUsersData)
    {
        try
        {
            var result = JsonSerializer.Deserialize<List<UserJsonConvertDto>>(jsonWithUsersData)
                ?? throw new Exception("Не удалось десериализовать пользоваталей с файла");

            return result;
        }
        catch (Exception ex)
        {
            logger.LogCritical(
                ex,
                "Произошла критическая ошибка при попытке десериализации пользователей");

            throw;
        }
    }
}
