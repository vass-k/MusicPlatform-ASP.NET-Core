namespace MusicPlatform.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using MusicPlatform.Data.Models;

    using static Common.DataSeedConstants.TrackSeed;

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

            entity
                .HasData(this.SeedTracks());
        }

        private IEnumerable<Track> SeedTracks()
        {
            List<Track> tracks = new List<Track>()
            {
                new Track
                {
                    Id = 1,
                    PublicId = Guid.Parse("a1b2c3d4-e5f6-7890-1234-567890abcdef"),
                    Title = "Lofi Chill",
                    ArtistName = "BoDleasons",
                    AudioUrl = AudioUrlTrack1,
                    DurationInSeconds = 122,
                    UploaderId = DefaultUserId,
                    GenreId = 7,
                    CreatedOn = new DateTime(2025, 1, 15, 10, 30, 0, DateTimeKind.Utc)
                },

                new Track
                {
                    Id = 2,
                    PublicId = Guid.Parse("b2c3d4e5-f6a7-8901-2345-67890abcdef1"),
                    Title = "Midnight Drive",
                    ArtistName = "AlexiAction",
                    AudioUrl = AudioUrlTrack2,
                    DurationInSeconds = 183,
                    UploaderId = DefaultUserId,
                    GenreId = 1,
                    CreatedOn = new DateTime(2025, 2, 20, 14, 0, 0, DateTimeKind.Utc)
                },

                new Track
                {
                    Id = 3,
                    PublicId = Guid.Parse("c3d4e5f6-a7b8-9012-3456-7890abcdef12"),
                    Title = "The Power of Rock",
                    ArtistName = "AlexGrohl",
                    AudioUrl = AudioUrlTrack3,
                    DurationInSeconds = 130,
                    UploaderId = DefaultUserId,
                    GenreId = 3,
                    CreatedOn = new DateTime(2025, 3, 1, 18, 45, 0, DateTimeKind.Utc)
                },

                new Track
                {
                    Id = 4,
                    PublicId = Guid.Parse("d4e5f6a7-b8c9-0123-4567-890abcdef123"),
                    Title = "Summer Glow",
                    ArtistName = "Sun Chasers",
                    AudioUrl = AudioUrlTrack4,
                    DurationInSeconds = 145,
                    UploaderId = DefaultUserId,
                    GenreId = 4,
                    CreatedOn = new DateTime(2025, 3, 5, 11, 0, 0, DateTimeKind.Utc)
                },

                new Track
                {
                    Id = 5,
                    PublicId = Guid.Parse("e5f6a7b8-c9d0-1234-5678-90abcdef1234"),
                    Title = "Stardust Echoes",
                    ArtistName = "Celestial Sound",
                    AudioUrl = AudioUrlTrack5,
                    DurationInSeconds = 195,
                    UploaderId = DefaultUserId,
                    GenreId = 8,
                    CreatedOn = new DateTime(2025, 3, 10, 22, 15, 0, DateTimeKind.Utc)
                },

                new Track
                {
                    Id = 6,
                    PublicId = Guid.Parse("f6a7b8c9-d0e1-2345-6789-0abcdef12345"),
                    Title = "Urban Canvas",
                    ArtistName = "Beat Architect",
                    AudioUrl = AudioUrlTrack6,
                    DurationInSeconds = 110,
                    UploaderId = DefaultUserId,
                    GenreId = 2,
                    CreatedOn = new DateTime(2025, 3, 12, 9, 5, 0, DateTimeKind.Utc)
                },

                new Track
                {
                    Id = 7,
                    PublicId = Guid.Parse("a7b8c9d0-e1f2-3456-7890-bcdef1234567"),
                    Title = "Smoky Lounge",
                    ArtistName = "The Midnight Trio",
                    AudioUrl = AudioUrlTrack7,
                    DurationInSeconds = 155,
                    UploaderId = DefaultUserId,
                    GenreId = 6,
                    CreatedOn = new DateTime(2025, 3, 18, 23, 0, 0, DateTimeKind.Utc)
                },

                new Track
                {
                    Id = 8,
                    PublicId = Guid.Parse("b8c9d0e1-f2a3-4567-8901-cdef12345678"),
                    Title = "Default Image Track",
                    ArtistName = "Serene Strings",
                    AudioUrl = AudioUrlTrack8,
                    DurationInSeconds = 210,
                    UploaderId = DefaultUserId,
                    GenreId = 5,
                    CreatedOn = new DateTime(2025, 3, 25, 7, 30, 0, DateTimeKind.Utc)
                },

                new Track
                {
                    Id = 9,
                    PublicId = Guid.Parse("c9d0e1f2-a3b4-5678-9012-def123456789"),
                    Title = "Rainy Afternoon",
                    ArtistName = "Cozy Beats",
                    AudioUrl = AudioUrlTrack9,
                    DurationInSeconds = 140,
                    UploaderId = DefaultUserId,
                    GenreId = 7,
                    CreatedOn = new DateTime(2025, 4, 2, 16, 20, 0, DateTimeKind.Utc)
                },

                new Track
                {
                    Id = 10,
                    PublicId = Guid.Parse("d0e1f2a3-b4c5-6789-0123-ef1234567890"),
                    Title = "Deep Down",
                    ArtistName = "Studio 54",
                    AudioUrl = AudioUrlTrack10,
                    DurationInSeconds = 172,
                    UploaderId = DefaultUserId,
                    GenreId = 1,
                    CreatedOn = new DateTime(2025, 4, 8, 19, 0, 0, DateTimeKind.Utc)
                },

                new Track
                {
                    Id = 11,
                    PublicId = Guid.Parse("e1f2a3b4-c5d6-7890-1234-f12345678901"),
                    Title = "Electric Dreams",
                    ArtistName = "Neon Bloom",
                    AudioUrl = AudioUrlTrack11,
                    DurationInSeconds = 160,
                    UploaderId = DefaultUserId,
                    GenreId = 4,
                    CreatedOn = new DateTime(2025, 4, 15, 12, 10, 0, DateTimeKind.Utc)
                },
                new Track
                {
                    Id = 12,
                    PublicId = Guid.Parse("f2a3b4c5-d6e7-8901-2345-123456789012"),
                    Title = "Mountain King",
                    ArtistName = "Stone Temple",
                    AudioUrl = AudioUrlTrack12,
                    DurationInSeconds = 205,
                    UploaderId = DefaultUserId,
                    GenreId = 3,
                    CreatedOn = new DateTime(2025, 4, 22, 17, 55, 0, DateTimeKind.Utc)
                }
            };

            return tracks;
        }
    }
}
