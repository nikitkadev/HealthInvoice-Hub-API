using HealthInvoice.Core.Dtos.Auth;

namespace HealthInvoice.Core.Interfaces.Services.Authorization;

public interface IUsersDeserializer
{
    List<UserJsonConvertDto> DeserializeUsers(
        Stream jsonWithUsersData);
}
