using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using HealthInvoice.Core.Entities.Invoices.Main.L;

namespace HealthInvoice.Infrastructure.Database.EF.Configurations.Tables.Invoices.Main.L;

public class PersModelConfiguration : IEntityTypeConfiguration<PersEntity>
{
    public void Configure(EntityTypeBuilder<PersEntity> builder)
    {
        builder.ToTable("pers").HasKey(pers => pers.Uid);
        
        builder.Property(prop => prop.Uid).HasColumnName("uid").ValueGeneratedOnAdd();
        builder.Property(prop => prop.PersListUid).HasColumnName("pers_list_uid");
        builder.Property(prop => prop.IdPac).HasColumnName("id_pac").HasMaxLength(36);
        builder.Property(prop => prop.Fam).HasColumnName("fam").HasMaxLength(40);
        builder.Property(prop => prop.Im).HasColumnName("im").HasMaxLength(40);
        builder.Property(prop => prop.Ot).HasColumnName("ot").HasMaxLength(40);
        builder.Property(prop => prop.W).HasColumnName("w");
        builder.Property(prop => prop.Dr).HasColumnName("dr");
        builder.Property(prop => prop.Dost).HasColumnName("dost").IsRequired(false);
        builder.Property(prop => prop.Tel).HasColumnName("tel").HasMaxLength(100).IsRequired(false);
        builder.Property(prop => prop.FamP).HasColumnName("fam_p").HasMaxLength(40).IsRequired(false);
        builder.Property(prop => prop.ImP).HasColumnName("im_p").HasMaxLength(40).IsRequired(false);
        builder.Property(prop => prop.OtP).HasColumnName("ot_p").HasMaxLength(40).IsRequired(false);
        builder.Property(prop => prop.WP).HasColumnName("w_p").IsRequired(false);
        builder.Property(prop => prop.DrP).HasColumnName("dr_p").IsRequired(false);
        builder.Property(prop => prop.DostP).HasColumnName("dost_p").IsRequired(false);
        builder.Property(prop => prop.MR).HasColumnName("mr").HasMaxLength(100).IsRequired(false);
        builder.Property(prop => prop.Doctype).HasColumnName("doctype").HasMaxLength(2).IsRequired(false);
        builder.Property(prop => prop.Docser).HasColumnName("docser").HasMaxLength(10).IsRequired(false);
        builder.Property(prop => prop.Docnum).HasColumnName("docnum").HasMaxLength(20).IsRequired(false);
        builder.Property(prop => prop.Snils).HasColumnName("snils").HasMaxLength(14).IsRequired(false);
        builder.Property(prop => prop.Okatog).HasColumnName("okatog").HasMaxLength(11).IsRequired(false);
        builder.Property(prop => prop.Okatop).HasColumnName("okatop").HasMaxLength(11).IsRequired(false);
        builder.Property(prop => prop.Comentp).HasColumnName("comentp").HasMaxLength(255).IsRequired(false);
        builder.Property(prop => prop.DocDate).HasColumnName("docdate").IsRequired(false);
        builder.Property(prop => prop.DocOrg).HasColumnName("docorg").HasMaxLength(1000).IsRequired(false);

        builder.HasOne<PersListEntity>()
            .WithMany(persList => persList.Pers)
            .HasForeignKey(pers => pers.PersListUid);
    }
}