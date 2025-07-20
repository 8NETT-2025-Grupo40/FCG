using FCG.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Infrastructure.Configurations;

public class UserConfiguration : BaseEntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.ToTable("User");
        builder.Property(p => p.Role).IsRequired();

        builder.OwnsOne(o => o.Name, name =>
        {
            name.Property(p => p.Value).HasMaxLength(100).HasColumnName("Name").IsRequired();
            name.WithOwner();
        });
        builder.OwnsOne(u => u.Email, email =>
        {
            email.Property(p => p.Address).HasMaxLength(150).HasColumnName("Email").IsRequired();
            email.HasIndex(e => e.Address).IsUnique();
            email.WithOwner();
        });
        builder.OwnsOne(u => u.Password, password =>
        {
            password.Property(p => p.HashPassword).HasMaxLength(255).HasColumnName("Password").IsRequired();
            password.WithOwner();
        });
    }
}