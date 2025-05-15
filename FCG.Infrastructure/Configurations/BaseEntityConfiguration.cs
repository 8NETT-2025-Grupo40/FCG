using FCG.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Infrastructure.Configurations;

public abstract class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(p => p.Id).IsRequired();

        builder.Property(p => p.DateCreated).IsRequired();
        builder.Property(p => p.DateUpdated);
        builder.Property(p => p.Status).IsRequired();
    }
}