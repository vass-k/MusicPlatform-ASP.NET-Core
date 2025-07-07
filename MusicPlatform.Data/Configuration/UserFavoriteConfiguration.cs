namespace MusicPlatform.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using MusicPlatform.Data.Models;

    public class UserFavoriteConfiguration : IEntityTypeConfiguration<UserFavorite>
    {
        public void Configure(EntityTypeBuilder<UserFavorite> entity)
        {
            entity
                .HasKey(uf => new { uf.UserId, uf.TrackId });

            entity
                .HasOne(uf => uf.User)
                .WithMany(u => u.FavoriteTracks)
                .HasForeignKey(uf => uf.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(uf => uf.Track)
                .WithMany(t => t.UserFavorites)
                .HasForeignKey(uf => uf.TrackId)
                .OnDelete(DeleteBehavior.Restrict);

            // We make sure if a Track is soft-deleted, the likes for
            // it are also filtered out automatically.
            entity.HasQueryFilter(uf => !uf.Track.IsDeleted);
        }
    }
}
