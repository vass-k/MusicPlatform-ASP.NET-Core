namespace MusicPlatform.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using MusicPlatform.Data.Models;

    public class TrackConfiguration : IEntityTypeConfiguration<Track>
    {
        public void Configure(EntityTypeBuilder<Track> entity)
        {
            entity.HasQueryFilter(e => !e.IsDeleted);

            entity
                .HasOne(t => t.Uploader)
                .WithMany(u => u.UploadedTracks)
                .HasForeignKey(t => t.UploaderId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(t => t.Genre)
                .WithMany(g => g.Tracks)
                .HasForeignKey(t => t.GenreId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
