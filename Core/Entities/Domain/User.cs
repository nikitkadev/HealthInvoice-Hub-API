namespace HealthInvoice.Core.Entities.Domain;

public class User
{
    public int Uid { get; set; }
    public string Username{ get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string CodeOrg { get; set; } = string.Empty;
    public string Surname { get; set;  } = string.Empty;
    public string Name { get;set; } = string.Empty;
    public string Patronymic { get; set; } = string.Empty;
    public string Phone { get; set;  } = string.Empty;
    public string OrganiztionName { get; set; } = string.Empty;
    public DateTime LastActivity { get; set; } = DateTime.Now;
    public bool PersDataAccepted { get; set; }
}
