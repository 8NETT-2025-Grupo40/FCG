using FCG.Domain.Games.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Infrastructure.Configurations;

public class PlayerProfileConfiguration : BaseEntityConfiguration<PlayerProfile>
{
    public override void Configure(EntityTypeBuilder<PlayerProfile> builder)
    {
        base.Configure(builder);
        builder.ToTable("PlayerProfile");

        builder.Property(p => p.UserId).IsRequired();

        builder.OwnsOne(p => p.Nickname, nickname =>
        {
            nickname.Property(n => n.Value)
                .HasColumnName("Nickname")
                .HasMaxLength(20)
                .IsRequired();

            nickname.WithOwner();
        });

        builder.HasMany(p => p.Games)
            .WithOne(pg => pg.PlayerProfile)
            .HasForeignKey(pg => pg.PlayerProfileId);
    }
}