using FCG.Domain.Modules.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Infrastructure.Configurations;

public class UserConfiguration : BaseEntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.OwnsOne(u => u.Name, name =>
        {
            name.Property(n => n.Value)
                .HasMaxLength(100)
                .IsRequired();
        });

        builder.OwnsOne(u => u.Email, email =>
        {
            email.Property(e => e.Address)
                .HasMaxLength(150)
                .IsRequired();
        });

        builder.OwnsOne(u => u.Password, password =>
        {
            password.Property(p => p.HashPassword)
                .HasMaxLength(256)
                .IsRequired();
        });

        builder.Property(u => u.Role)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(u => u.CreateDate)
            .IsRequired();
    }
}