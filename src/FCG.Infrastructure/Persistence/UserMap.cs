using FCG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Infrastructure.Persistence;

public class UserMap : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Name).IsRequired().HasMaxLength(150);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(150);
        builder.Property(u => u.Password).IsRequired();
        builder.Property(u => u.CpfNumber).HasMaxLength(11);
        builder.Property(u => u.Phone).HasMaxLength(20);
        builder.Property(u => u.BirthDate).IsRequired();
        builder.Property(u => u.Situation).IsRequired();

        builder.HasMany(u => u.Roles)
               .WithOne(r => r.User)
               .HasForeignKey(r => r.UserId);
    }
}