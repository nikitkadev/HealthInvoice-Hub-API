using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

using HealthInvoice.Core.Common;
using HealthInvoice.Core.Entities.Domain;
using HealthInvoice.Core.Interfaces.Repository.Users;
using HealthInvoice.Infrastructure.Database.EF.Context;

namespace HealthInvoice.Infrastructure.Implementation.Repository;

public class UserRepository(
    ILogger<UserRepository> logger,
    SMODbContext dbContext) : IUserRepository
{
    public async Task AddUsersAsync(List<User> newUsers)
    {
        var existingUsernames = await dbContext.Users
            .Select(u => u.Username)
            .ToListAsync();

        var usersToAdd = newUsers
            .Where(u => !existingUsernames.Contains(u.Username))
            .ToList();

        if (usersToAdd.Count == 0)
            return; 

        await dbContext.Users.AddRangeAsync(usersToAdd);
        await dbContext.SaveChangesAsync();
    }
    public async Task AddUserAsync(User user)
    {
        var dbUser = dbContext.Users
            .FirstOrDefault(x => x.Username == user.Username);

        if(dbUser is not null)
        {
            return;
        }

        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();
    }
    
    public async Task RemoveUserAsync(int userUid)
    {
        var dbUser = await dbContext.Users.FindAsync(userUid) ?? throw new UserIsNotFoundException(userUid.ToString());

        dbContext.Users.Remove(dbUser);
        await dbContext.SaveChangesAsync();
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await dbContext.Users.OrderByDescending(p => p.LastActivity).ToListAsync();
    }

    public async Task<User?> GetUserByUsernameAsync(
        string username, 
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Users.FirstOrDefaultAsync(
            user => user.Username == username, cancellationToken);
    }

    public async Task UpdateActivityTimeAsync(string username)
    {
        await dbContext.Users
            .Where(user => user.Username == username)
            .ExecuteUpdateAsync(
                s => s.SetProperty(
                    user => user.LastActivity, DateTime.Now));
    }

    public async Task AcceptPersonalDataProcessingAsync(string username)
    {
        await dbContext.Users
            .Where(user => user.Username == username)
            .ExecuteUpdateAsync(
                s => s.SetProperty(
                    prop => prop.PersDataAccepted, true));
    }

    public async Task<string> GetUserOrganizationName(string organizationCode)
    {

        logger.LogInformation("[NIKITKADEV] Код организации: {OrgCode}",
            organizationCode);

        var pOrgCode = new SqlParameter()
        {
            ParameterName = "@pCodeMo",
            Direction = System.Data.ParameterDirection.Input,
            Value = organizationCode,
            Size = 6
        };

        var pOrgName = new SqlParameter()
        {
            ParameterName = "@pOrgName",
            Direction = System.Data.ParameterDirection.Output,
            Size = 255
        };

        await dbContext.Database.ExecuteSqlRawAsync(
            "EXEC sp26_get_organization_name @pCodeMo, @pOrgName OUTPUT",
            [pOrgCode, pOrgName]);

        return pOrgName.Value == DBNull.Value 
            ? "Отсутствует в справочнике" 
            : (string)pOrgName.Value;
    }
}