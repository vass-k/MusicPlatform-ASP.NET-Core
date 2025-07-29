namespace MusicPlatform.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using MusicPlatform.Data.Models;

    public class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> entity)
        {
            entity
                .HasQueryFilter(e => !e.IsDeleted);

            entity
                .HasData(this.SeedGenres());
        }

        private IEnumerable<Genre> SeedGenres()
        {
            List<Genre> genres = new List<Genre>()
           {
                new Genre
                {
                    Id = 1,
                    PublicId = Guid.Parse("c0a9e70e-6e8a-4b0d-9b0d-0343a8b4e47a"),
                    Name = "Electronic"
                },
                new Genre
                {
                    Id = 2,
                    PublicId = Guid.Parse("d1b8e81f-7f9b-4e1a-9a9b-0454b9c5f58b"),
                    Name = "Hip-Hop"
                },
                new Genre
                {
                    Id = 3,
                    PublicId = Guid.Parse("e2c7f92f-8a0c-4f2b-8b0c-0565cae6e69c"),
                    Name = "Rock"
                },
                new Genre
                {
                    Id = 4,
                    PublicId = Guid.Parse("f3d6ea3f-9b1d-4e3c-7c1d-0676dbf7f7ad"),
                    Name = "Pop"
                },
                new Genre
                {
                    Id = 5,
                    PublicId = Guid.Parse("04e5fb4f-a02e-4d4d-6d2e-0787ec08e8be"),
                    Name = "Classical"
                },
                new Genre
                {
                    Id = 6,
                    PublicId = Guid.Parse("15f40a5f-b13f-4f5e-5e3f-0898fd19f9cf"),
                    Name = "Jazz"
                },
                new Genre
                {
                    Id = 7,
                    PublicId = Guid.Parse("26031b6f-c24a-4e6f-4f4a-09a90e2a0ad0"),
                    Name = "Lo-Fi"
                },
                new Genre
                {
                    Id = 8,
                    PublicId = Guid.Parse("37122c7f-d35b-4d7f-3f5b-0ab91f3b1be1"),
                    Name = "Ambient"
                },
                new Genre
                {
                    Id = 9,
                    PublicId = Guid.Parse("48213d8f-e46c-4c8f-2c6c-0bca204c2cf2"),
                    Name = "Other"
                }
            };

            return genres;
        }
    }
}
