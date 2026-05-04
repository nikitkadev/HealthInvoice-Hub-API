using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HealthInvoice.Core.Entities.Domain;

namespace HealthInvoice.Infrastructure.Database.EF.Configurations.Tables.Users;

public class UserModelConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("app_users").HasKey(x => x.Uid);

        builder.Property(prop => prop.Uid)
            .ValueGeneratedOnAdd();

        builder.Property(prop => prop.Username)
            .HasColumnName("username")
            .HasMaxLength(255)
            .IsRequired(true);

        builder.Property(prop => prop.PasswordHash)
            .HasColumnName("password_hash")
            .HasMaxLength(255)
            .IsRequired(true);

        builder.Property(prop => prop.CodeOrg)
            .HasColumnName("code_org")
            .HasMaxLength(6)
            .IsRequired(true);

        builder.Property(prop => prop.Surname)
            .HasColumnName("surname")
            .HasMaxLength(255)
            .IsRequired(true);

        builder.Property(prop => prop.Name)
            .HasColumnName("name")
            .HasMaxLength(255)
            .IsRequired(true);

        builder.Property(prop => prop.Patronymic)
            .HasColumnName("patronymic")
            .HasMaxLength(255)
            .IsRequired(true);

        builder.Property(prop => prop.Phone)
            .HasColumnName("phone")
            .HasMaxLength(255)
            .IsRequired(true);

        builder.Property(prop => prop.OrganiztionName)
            .HasColumnName("organization_name")
            .HasMaxLength(255)
            .IsRequired(true);

        builder.Property(prop => prop.LastActivity)
            .HasColumnName("last_activity")
            .IsRequired(true);

        builder.Property(prop => prop.PersDataAccepted)
            .HasColumnName("pers_data_accepted")
            .IsRequired(true);
    }
}