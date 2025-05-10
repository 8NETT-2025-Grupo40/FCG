using FCG.Domain.Modules.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Infrastructure.Modules.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(e => e.Id);
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.Role).IsRequired();
            builder.Property(p => p.Status).IsRequired();
            builder.Property(p => p.CreateDate).IsRequired();
            builder.OwnsOne(o => o.Name, name =>
            {
                name.Property(p => p.Value).HasMaxLength(100).HasColumnName("Name").IsRequired();
                name.WithOwner();
            });
            builder.OwnsOne(u => u.Email, email =>
            {
                email.Property(p => p.Address).HasMaxLength(150).HasColumnName("Email").IsRequired();
                email.WithOwner();
            });
            builder.OwnsOne(u => u.Password, password =>
            {
                password.Property(p => p.HashPassword).HasMaxLength(88).HasColumnName("Password").IsRequired();
                password.WithOwner();
            });
        }
    }
}
