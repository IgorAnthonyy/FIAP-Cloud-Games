using FCG.Domain.Entities;
using FCG.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Infrastructure.Data.Configuration;

public class UserMap : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Name).IsRequired().HasMaxLength(150);

        builder.Property(u => u.Email)
       .HasConversion(
           email => email.Value,
           value => new Domain.ValueObjects.Email(value)
       )
       .HasColumnName("Email")
       .IsRequired();

        builder.Property(u => u.Password).IsRequired();

        builder.Property(u => u.Cpf)
        .HasConversion(
            cpf => cpf.Code,
            value => new CPF(value)
        )
        .HasColumnName("CpfNumber")
        .HasMaxLength(11);

        builder.Property(u => u.Phone).HasMaxLength(20);
        builder.Property(u => u.BirthDate).IsRequired();
        builder.Property(u => u.Situation).IsRequired();

        builder.HasMany(u => u.Roles)
               .WithOne(r => r.User)
               .HasForeignKey(r => r.UserId);
    }
}