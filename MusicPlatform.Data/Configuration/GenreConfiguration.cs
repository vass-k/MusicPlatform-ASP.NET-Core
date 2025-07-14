namespace MusicPlatform.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using MusicPlatform.Data.Models;

    public class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> entity)
        {
            entity.HasQueryFilter(e => !e.IsDeleted);

            entity.HasData(
                new Genre { Id = 1, PublicId = Guid.NewGuid(), Name = "Electronic" },
                new Genre { Id = 2, PublicId = Guid.NewGuid(), Name = "Hip-Hop" },
                new Genre { Id = 3, PublicId = Guid.NewGuid(), Name = "Rock" },
                new Genre { Id = 4, PublicId = Guid.NewGuid(), Name = "Pop" },
                new Genre { Id = 5, PublicId = Guid.NewGuid(), Name = "Classical" },
                new Genre { Id = 6, PublicId = Guid.NewGuid(), Name = "Jazz" },
                new Genre { Id = 7, PublicId = Guid.NewGuid(), Name = "Lo-Fi" },
                new Genre { Id = 8, PublicId = Guid.NewGuid(), Name = "Ambient" },
                new Genre { Id = 9, PublicId = Guid.NewGuid(), Name = "Other" }
            );
        }
    }
}
