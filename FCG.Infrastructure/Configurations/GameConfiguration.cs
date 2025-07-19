using FCG.Domain.Games.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Infrastructure.Configurations;

public class GameConfiguration : BaseEntityConfiguration<Game>
{
    public override void Configure(EntityTypeBuilder<Game> builder)
    {
        base.Configure(builder);

        builder.ToTable("Game");

        builder.Property(g => g.Title)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(g => g.Description)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(g => g.Genre)
            .IsRequired();

        builder.Property(g => g.ReleaseDate)
            .IsRequired();

        builder.OwnsOne(g => g.Price, price =>
        {
            price.Property(p => p.Value)
                .HasColumnName("Price")
                .HasPrecision(10, 2)
                .IsRequired();

            price.WithOwner();
        });

        builder.HasMany(g => g.PlayerProfiles)
            .WithOne(pg => pg.Game)
            .HasForeignKey(pg => pg.GameId);
    }
}