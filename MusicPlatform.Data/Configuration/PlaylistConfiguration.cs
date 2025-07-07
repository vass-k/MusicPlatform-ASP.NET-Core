namespace MusicPlatform.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using MusicPlatform.Data.Models;

    public class PlaylistConfiguration : IEntityTypeConfiguration<Playlist>
    {
        public void Configure(EntityTypeBuilder<Playlist> entity)
        {
            entity.HasQueryFilter(e => !e.IsDeleted);

            entity
                .HasOne(p => p.Creator)
                .WithMany(u => u.Playlists)
                .HasForeignKey(p => p.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
