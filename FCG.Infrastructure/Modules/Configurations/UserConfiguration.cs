using FCG.Domain.Entity;
using FCG.Domain.Modules.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Infrastructure.Modules.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("User");
            builder.HasKey(e => e.Id);
            builder.Property(p => p.Id).HasColumnType("UNIQUEIDENTIFIER").IsRequired();
            builder.Property(p => p.Name).HasColumnType("NVARCHAR(100)").IsRequired();
            builder.Property(p => p.Email).HasColumnType("NVARCHAR(150)").IsRequired();
            builder.Property(p => p.Password).HasColumnType("VARCHAR(88)").IsRequired();
            builder.Property(p => p.Role).HasColumnType("INT").IsRequired();
            builder.Property(p => p.CreateDate).HasColumnType("DATETIME2").IsRequired();
            builder.HasData(ReturnAdmUser(), ReturnCommonUser());
        }

        private UserEntity ReturnAdmUser()
        {
            return new UserEntity()
            {
                Id = Guid.NewGuid(),
                Name = "Usuario Adm",
                CreateDate = new DateTime(2025, 5, 7),
                Email = "adm@fcg.com",
                Password = "H9i6kZgxyPZ0F4t0a9XxUt8zRMNCwdzktYlJv8EYaWg=",
                Role = 1
            };
        }
        
        private UserEntity ReturnCommonUser()
        {
            return new UserEntity()
            {
                Id = Guid.NewGuid(),
                Name = "Usuario Comum",
                CreateDate = new DateTime(2025, 5, 7),
                Email = "usr@fcg.com",
                Password = "FEYtYdA4Pp8nOH6mFGLwFxOX2XGBGmUQW9n+Ot8BHzA=",
                Role = 0
            };
        }
    }
}
