using FCG.Domain.Modules.Games;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Infrastructure.Configurations;

public class PlayerProfileGameConfiguration : IEntityTypeConfiguration<PlayerProfileGame>
{
    public void Configure(EntityTypeBuilder<PlayerProfileGame> builder)
    {
        builder.ToTable("PlayerProfileGame");

        builder.HasKey(pg => new { pg.PlayerProfileId, pg.GameId });

        builder.Property(pg => pg.AcquiredAt)
            .IsRequired();

        builder.HasOne(pg => pg.PlayerProfile)
            .WithMany(p => p.Games)
            .HasForeignKey(pg => pg.PlayerProfileId);

        builder.HasOne(pg => pg.Game)
            .WithMany(g => g.PlayerProfiles)
            .HasForeignKey(pg => pg.GameId);
    }
}