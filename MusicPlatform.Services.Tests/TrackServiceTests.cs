namespace MusicPlatform.Services.Tests
{
    using Microsoft.AspNetCore.Http;

    using MockQueryable;
    using Moq;

    using MusicPlatform.Data.Models;
    using MusicPlatform.Data.Repository.Interfaces;
    using MusicPlatform.Services.Common.Interfaces;
    using MusicPlatform.Services.Core;
    using MusicPlatform.Services.Core.Interfaces;
    using MusicPlatform.Web.ViewModels.Track;

    using System.Linq.Expressions;

    using static MusicPlatform.GCommon.ApplicationConstants;

    [TestFixture]
    public class TrackServiceTests
    {
        private Mock<ITrackRepository> trackRepositoryMock;
        private Mock<IGenreRepository> genreRepositoryMock;
        private Mock<ICloudStorageService> cloudStorageServiceMock;

        private ITrackService trackService;

        private List<Track> mockTracks;
        private AppUser user1;
        private List<Genre> mockGenres;

        [SetUp]
        public void Setup()
        {
            this.trackRepositoryMock = new Mock<ITrackRepository>(MockBehavior.Strict);
            this.genreRepositoryMock = new Mock<IGenreRepository>(MockBehavior.Strict);
            this.cloudStorageServiceMock = new Mock<ICloudStorageService>(MockBehavior.Strict);

            this.trackService = new TrackService(
                this.trackRepositoryMock.Object,
                this.genreRepositoryMock.Object,
                this.cloudStorageServiceMock.Object);

            user1 = new AppUser
            {
                Id = "user1-id",
                UserName = "UserOne"
            };

            mockTracks = new List<Track>
            {
                new Track
                {
                    Id = 1,
                    PublicId = Guid.NewGuid(),
                    Title = "First Song",
                    ArtistName = "Artist A",
                    UploaderId = "user1-id",
                    UserFavorites = new List<UserFavorite>()
                },
                new Track
                {
                    Id = 2,
                    PublicId = Guid.NewGuid(),
                    Title = "Second Tune",
                    ArtistName = "Artist B",
                    UploaderId = "user2-id",
                    UserFavorites = new List<UserFavorite>()
                },
                new Track
                {
                    Id = 3,
                    PublicId = Guid.NewGuid(),
                    Title = "Another Song",
                    ArtistName = "Artist A",
                    UploaderId = "user1-id",
                    UserFavorites = new List<UserFavorite>()
                }
            };

            mockGenres = new List<Genre>
            {
                new Genre
                {
                    Id = 1,
                    Name = "Rock" },
                new Genre
                {
                    Id = 2,
                    Name = "Pop"
                }
            };
        }

        [Test]
        public async Task GetTracksForIndexPage_Should_ReturnAllTracks_When_SearchStringIsNull()
        {
            var tracksQueryable = mockTracks.BuildMock();

            this.trackRepositoryMock.Setup(r => r.GetAllAsQueryable()).Returns(tracksQueryable);

            var result = await this.trackService
                .GetTracksForIndexPageAsync(null, 1, 10);

            Assert.That(result.PagedTracks.Items.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task GetTracksForIndexPage_Should_FilterByTitle_When_SearchStringIsProvided()
        {
            var tracksQueryable = mockTracks.BuildMock();
            this.trackRepositoryMock.Setup(r => r.GetAllAsQueryable()).Returns(tracksQueryable);

            var result = await this.trackService
                .GetTracksForIndexPageAsync("Song", 1, 10);

            Assert.That(result.PagedTracks.Items.Count(), Is.EqualTo(2));
            Assert.IsTrue(result.PagedTracks.Items.All(t => t.Title.Contains("Song")));
        }

        [Test]
        public async Task GetTracksForIndexPage_Should_FilterByArtistName_When_SearchStringIsProvided()
        {
            var tracksQueryable = mockTracks.BuildMock();

            this.trackRepositoryMock.Setup(r => r.GetAllAsQueryable()).Returns(tracksQueryable);

            var result = await this.trackService
                .GetTracksForIndexPageAsync("Artist A", 1, 10);

            Assert.That(result.PagedTracks.Items.Count(), Is.EqualTo(2));
            Assert.IsTrue(result.PagedTracks.Items.All(t => t.ArtistName == "Artist A"));
        }

        [Test]
        public async Task AddTrackAsync_Should_CallDependenciesCorrectly()
        {
            var mockAudioFile = new Mock<IFormFile>();

            mockAudioFile.Setup(f => f.Length).Returns(1024);
            mockAudioFile.Setup(f => f.FileName).Returns("test-track.mp3");

            var mockImageFile = new Mock<IFormFile>();
            mockImageFile.Setup(f => f.Length).Returns(512);
            mockImageFile.Setup(f => f.FileName).Returns("test-image.png");

            var model = new TrackAddViewModel
            {
                Title = "New Test Track",
                ArtistName = "Test Artist",
                GenreId = 1,
                AudioFile = mockAudioFile.Object,
                ImageFile = mockImageFile.Object
            };

            this.cloudStorageServiceMock.Setup(s => s.UploadAudioAsync(mockAudioFile.Object)).ReturnsAsync("http://audio.url");
            this.cloudStorageServiceMock.Setup(s => s.UploadImageAsync(mockImageFile.Object)).ReturnsAsync("http://image.url");
            this.trackRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Track>())).Returns(Task.CompletedTask);

            await this.trackService
                .AddTrackAsync(model, "uploader-id");

            this.cloudStorageServiceMock.Verify(s => s.UploadAudioAsync(mockAudioFile.Object), Times.Once);
            this.cloudStorageServiceMock.Verify(s => s.UploadImageAsync(mockImageFile.Object), Times.Once);
            this.trackRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Track>()), Times.Once);
        }

        [Test]
        public async Task GetTrackForEdit_Should_ReturnNull_When_UserIsNotOwner()
        {
            var track = mockTracks.First(t => t.UploaderId == "user1-id");

            var nonOwnerId = "user2-id";

            this.trackRepositoryMock
                .Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Track, bool>>>()))
                .ReturnsAsync(track);

            var result = await this.trackService
                .GetTrackForEditAsync(track.PublicId, nonOwnerId);

            Assert.IsNull(result);
        }

        [Test]
        public async Task UpdateTrack_Should_ReturnFalse_When_UserIsNotOwner()
        {
            var track = mockTracks.First(t => t.UploaderId == "user1-id");
            var model = new TrackEditViewModel
            {
                PublicId = track.PublicId,
                Title = "Updated"
            };

            var nonOwnerId = "user2-id";

            this.trackRepositoryMock
                .Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Track, bool>>>()))
                .ReturnsAsync(track);

            var result = await this.trackService
                .UpdateTrackAsync(model, nonOwnerId);

            Assert.IsFalse(result);

            this.trackRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Track>()), Times.Never);
        }

        [Test]
        public async Task DeleteTrack_Should_ReturnFalse_When_UserIsNotOwner()
        {
            var track = mockTracks.First(t => t.UploaderId == "user1-id");

            var nonOwnerId = "user2-id";

            this.trackRepositoryMock
                .Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Track, bool>>>()))
                .ReturnsAsync(track);

            var result = await this.trackService
                .DeleteTrackAsync(track.PublicId, nonOwnerId);

            Assert.IsFalse(result);
            this.trackRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Track>()), Times.Never);
        }

        [Test]
        public async Task IncrementPlayCount_Should_CallRepositoryMethod()
        {
            var trackId = Guid.NewGuid();

            this.trackRepositoryMock.Setup(r => r.IncrementPlayCountAsync(trackId)).ReturnsAsync(1);

            await this.trackService
                .IncrementdPlayCountAsync(trackId);

            this.trackRepositoryMock.Verify(r => r.IncrementPlayCountAsync(trackId), Times.Once);
        }

        [Test]
        public async Task GetTopTracks_Should_CorrectlyMapData()
        {
            var topTracksData = new List<Track>
            {
                new Track
                {
                    Title = "Top Hit 1",
                    Plays = 100,
                    UserFavorites = new List<UserFavorite>()
                },
                new Track
                {
                    Title = "Top Hit 2",
                    Plays = 200,
                    UserFavorites = new List<UserFavorite>
                    {
                        new UserFavorite()
                    }
                }
            };

            this.trackRepositoryMock.Setup(r => r.GetMostFavoritesTracksAsync(2)).ReturnsAsync(topTracksData);

            var result = await this.trackService
                .GetTopTracksAsync(2);

            var firstTrack = result.First();
            var secondTrack = result.Last();

            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(firstTrack.Title, Is.EqualTo("Top Hit 1"));
            Assert.That(secondTrack.FavoritesCount, Is.EqualTo(1));
        }

        [Test]
        public async Task GetGenresForSelect_Should_ReturnAllGenres_With_NoneSelected()
        {
            this.genreRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(mockGenres);

            var result = await this.trackService
                .GetGenresForSelectAsync(null);

            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.IsFalse(result.Any(g => g.Selected));
        }

        [Test]
        public async Task GetGenresForSelect_Should_MarkCorrectGenreAsSelected()
        {
            var selectedGenreId = 2;

            this.genreRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(mockGenres);

            var result = await this.trackService
                .GetGenresForSelectAsync(selectedGenreId);

            var popGenre = result.First(g => g.Value == selectedGenreId.ToString());

            Assert.IsTrue(popGenre.Selected);
            Assert.That(result.Count(g => g.Selected), Is.EqualTo(1));
        }

        [Test]
        public async Task GetTrackForDelete_Should_ReturnNull_When_UserIsNotOwner()
        {
            var track = mockTracks.First(t => t.UploaderId == "user1-id");

            var nonOwnerId = "user2-id";

            this.trackRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Track, bool>>>())).ReturnsAsync(track);

            var result = await this.trackService
                .GetTrackForDeleteAsync(track.PublicId, nonOwnerId);

            Assert.IsNull(result);
        }

        [Test]
        public void AddTrack_Should_ThrowInvalidOperationException_For_OversizedFile()
        {
            var mockLargeFile = new Mock<IFormFile>();

            mockLargeFile.Setup(f => f.Length).Returns(FileValidationConstants.MaxAudioFileSize + 1);
            mockLargeFile.Setup(f => f.FileName).Returns("large-file.mp3");
            var model = new TrackAddViewModel
            {
                AudioFile = mockLargeFile.Object
            };

            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await this.trackService
                    .AddTrackAsync(model, "user-id");
            });

            Assert.That(ex.Message, Does.Contain("File size exceeds the limit"));
        }

        [Test]
        public void AddTrack_Should_ThrowInvalidOperationException_For_InvalidExtension()
        {
            var mockInvalidFile = new Mock<IFormFile>();

            mockInvalidFile.Setup(f => f.Length).Returns(1024);
            mockInvalidFile.Setup(f => f.FileName).Returns("song.zip");

            var model = new TrackAddViewModel
            {
                AudioFile = mockInvalidFile.Object
            };

            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await this.trackService
                    .AddTrackAsync(model, "user-id");
            });
            Assert.That(ex.Message, Does.Contain("Invalid file type"));
        }
    }
}
