namespace MusicPlatform.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using MusicPlatform.Data.Models;

    public class PlaylistTrackConfiguration : IEntityTypeConfiguration<PlaylistTrack>
    {
        public void Configure(EntityTypeBuilder<PlaylistTrack> entity)
        {
            entity.HasKey(pt => new { pt.PlaylistId, pt.TrackId });

            // We make sure if a Playlist is soft-deleted, its entries in this
            // join table are also filtered out automatically.
            entity.HasQueryFilter(pt => !pt.Playlist.IsDeleted);
        }
    }
}
