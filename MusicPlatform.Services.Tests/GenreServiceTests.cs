namespace MusicPlatform.Services.Tests
{
    using MockQueryable;
    using Moq;

    using MusicPlatform.Data.Models;
    using MusicPlatform.Data.Repository.Interfaces;
    using MusicPlatform.Services.Core;
    using MusicPlatform.Services.Core.Interfaces;

    [TestFixture]
    public class GenreServiceTests
    {
        private Mock<IGenreRepository> genreRepositoryMock;
        private Mock<ITrackRepository> trackRepositoryMock;

        private IGenreService genreService;

        [SetUp]
        public void Setup()
        {
            this.genreRepositoryMock = new Mock<IGenreRepository>(MockBehavior.Strict);
            this.trackRepositoryMock = new Mock<ITrackRepository>(MockBehavior.Strict);
            this.genreService = new GenreService(
                this.genreRepositoryMock.Object,
                this.trackRepositoryMock.Object);
        }

        [Test]
        public void PassAlways()
        {
            Assert.Pass();
        }

        [Test]
        public async Task GetAllGenres_Should_ReturnEmptyCollection_When_NoGenresExist()
        {
            var emptyGenreList = new List<Genre>();
            var emptyGenreQueryable = emptyGenreList.BuildMock();

            this.genreRepositoryMock
                .Setup(r => r.GetAllAsQueryable())
                .Returns(emptyGenreQueryable);

            var result = await this.genreService
                .GetAllGenresWithTrackCountAsync();

            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetAllGenres_Should_ReturnCorrectCount_When_GenresExist()
        {
            var genres = new List<Genre>
            {
                new Genre { Name = "Rock", Tracks = new List<Track>() },
                new Genre { Name = "Pop", Tracks = new List<Track>() }
            };

            var genreQueryable = genres.BuildMock();

            this.genreRepositoryMock
                .Setup(r => r.GetAllAsQueryable())
                .Returns(genreQueryable);

            var result = await this.genreService
                .GetAllGenresWithTrackCountAsync();

            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetAllGenres_Should_CorrectlyMapDataToViewModel()
        {
            var genres = new List<Genre>
            {
                new Genre { Name = "Rock", PublicId = Guid.Parse("7915b191-3fa3-4419-bcf5-5e555fba4b52"), Tracks = new List<Track> { new Track(), new Track() } },
                new Genre { Name = "Pop", PublicId = Guid.Parse("7915b191-3fa3-4419-bcf5-5e555fba4b53"), Tracks = new List<Track> { new Track() } }
            };

            var genreQueryable = genres.BuildMock();

            this.genreRepositoryMock
                .Setup(r => r.GetAllAsQueryable())
                .Returns(genreQueryable);

            var result = await this.genreService
                .GetAllGenresWithTrackCountAsync();

            var rockGenre = result.First(g => g.Name == "Rock");
            var popGenre = result.First(g => g.Name == "Pop");

            Assert.That(rockGenre.TrackCount, Is.EqualTo(2));
            Assert.That(popGenre.TrackCount, Is.EqualTo(1));
            Assert.That(rockGenre.PublicId, Is.EqualTo(genres[0].PublicId));
        }

        [Test]
        public async Task GetGenreDetails_Should_ReturnNull_When_GuidIsEmpty()
        {
            var result = await this.genreService
                .GetGenreDetailsAsync(Guid.Empty, 1, 10);

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetGenreDetails_Should_ReturnNull_When_GenreNotFound()
        {
            var nonExistentId = Guid.Parse("7915b191-3fa3-4419-bcf5-5e555fba4b52");
            var emptyGenreList = new List<Genre>();
            var emptyGenreQueryable = emptyGenreList.BuildMock();

            this.genreRepositoryMock
                .Setup(r => r.GetAllAsQueryable())
                .Returns(emptyGenreQueryable);

            var result = await this.genreService
                .GetGenreDetailsAsync(nonExistentId, 1, 10);

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetGenreDetails_Should_ReturnCorrectData_When_GenreExists()
        {
            var genreId = 1;
            var genrePublicId = Guid.Parse("7915b191-3fa3-4419-bcf5-5e555fba4b52");
            var genre = new List<Genre> { new Genre { Id = genreId, PublicId = genrePublicId, Name = "Test Rock" } };
            var genreQueryable = genre.BuildMock();

            var tracks = new List<Track>
            {
                new Track
                {
                    Id = 101,
                    GenreId = genreId,
                    Title = "Track 1",
                    CreatedOn = new DateTime(2025, 5, 10)
                },
                new Track
                {
                    Id = 102,
                    GenreId = genreId,
                    Title = "Track 2",
                    CreatedOn = new DateTime(2025, 5, 9)
                },
                new Track
                {
                    Id = 103,
                    GenreId = 2,
                    Title = "Wrong Genre Track",
                    CreatedOn = new DateTime(2025, 5, 11)
                }
            };

            var trackQueryable = tracks.BuildMock();

            this.genreRepositoryMock
                .Setup(r => r.GetAllAsQueryable())
                .Returns(genreQueryable);

            this.trackRepositoryMock
                .Setup(r => r.GetAllAsQueryable())
                .Returns(trackQueryable);

            var result = await this.genreService
                .GetGenreDetailsAsync(genrePublicId, 1, 10);

            Assert.IsNotNull(result);
            Assert.That(result.GenreName, Is.EqualTo("Test Rock"));
            Assert.That(result.PagedTracks.Items.Count(), Is.EqualTo(2));
            Assert.That(result.PagedTracks.Items.First().Title, Is.EqualTo("Track 1"));
        }
    }
}
