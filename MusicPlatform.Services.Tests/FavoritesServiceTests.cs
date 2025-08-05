namespace MusicPlatform.Services.Tests
{
    using MockQueryable;
    using Moq;

    using MusicPlatform.Data.Models;
    using MusicPlatform.Data.Repository.Interfaces;
    using MusicPlatform.Services.Core;
    using MusicPlatform.Services.Core.Interfaces;

    using System.Linq.Expressions;

    [TestFixture]
    public class FavoritesServiceTests
    {
        private Mock<ITrackRepository> trackRepositoryMock;
        private Mock<IUserFavoriteRepository> favoriteRepositoryMock;

        private IFavoritesService favoritesService;

        private Track track1;
        private AppUser user1;

        [SetUp]
        public void Setup()
        {
            this.trackRepositoryMock = new Mock<ITrackRepository>(MockBehavior.Strict);
            this.favoriteRepositoryMock = new Mock<IUserFavoriteRepository>(MockBehavior.Strict);

            this.favoritesService = new FavoritesService(
                this.trackRepositoryMock.Object,
                this.favoriteRepositoryMock.Object);

            track1 = new Track
            {
                Id = 1,
                PublicId = Guid.NewGuid(),
                Title = "Test Track"
            };

            user1 = new AppUser
            {
                Id = "user1-id"
            };
        }

        [Test]
        public void LikeTrack_Should_ThrowInvalidOperationException_When_TrackNotFound()
        {
            this.trackRepositoryMock
                .Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Track, bool>>>()))
                .ReturnsAsync((Track?)null);

            Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await this.favoritesService
                .LikeTrackAsync(Guid.NewGuid(), user1.Id);
            }, "Track not found.");
        }

        [Test]
        public async Task LikeTrack_Should_CallAddAsync_When_TrackIsNotFavorited()
        {
            this.trackRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Track, bool>>>())).ReturnsAsync(track1);
            this.favoriteRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<UserFavorite, bool>>>())).ReturnsAsync((UserFavorite?)null);
            this.favoriteRepositoryMock.Setup(r => r.AddAsync(It.IsAny<UserFavorite>())).Returns(Task.CompletedTask);

            var favoritesList = new List<UserFavorite>
            {
                new UserFavorite()
            }.BuildMock();

            this.favoriteRepositoryMock.Setup(r => r.GetAllAsQueryable()).Returns(favoritesList);

            await this.favoritesService
                .LikeTrackAsync(track1.PublicId, user1.Id);

            this.favoriteRepositoryMock.Verify(r => r.AddAsync(It.IsAny<UserFavorite>()), Times.Once);
        }

        [Test]
        public async Task LikeTrack_Should_NotCallAddAsync_When_TrackIsAlreadyFavorited()
        {
            var existingFavorite = new UserFavorite
            {
                TrackId = track1.Id,
                UserId = user1.Id
            };

            this.trackRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Track, bool>>>())).ReturnsAsync(track1);
            this.favoriteRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<UserFavorite, bool>>>())).ReturnsAsync(existingFavorite);

            var favoritesList = new List<UserFavorite> {
                existingFavorite
            }.BuildMock();

            this.favoriteRepositoryMock.Setup(r => r.GetAllAsQueryable()).Returns(favoritesList);

            await this.favoritesService
                .LikeTrackAsync(track1.PublicId, user1.Id);

            this.favoriteRepositoryMock.Verify(r => r.AddAsync(It.IsAny<UserFavorite>()), Times.Never);
        }

        [Test]
        public async Task UnlikeTrack_Should_ReturnFalse_When_TrackNotFound()
        {
            this.trackRepositoryMock
                .Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Track, bool>>>()))
                .ReturnsAsync((Track?)null);

            var result = await this.favoritesService
                .UnlikeTrackAsync(Guid.NewGuid(), user1.Id);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task UnlikeTrack_Should_CallHardDelete_When_FavoriteExists()
        {
            var existingFavorite = new UserFavorite
            {
                TrackId = track1.Id,
                UserId = user1.Id
            };

            this.trackRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Track, bool>>>())).ReturnsAsync(track1);
            this.favoriteRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<UserFavorite, bool>>>())).ReturnsAsync(existingFavorite);
            this.favoriteRepositoryMock.Setup(r => r.HardDeleteAsync(existingFavorite)).ReturnsAsync(true);

            var result = await this.favoritesService
                .UnlikeTrackAsync(track1.PublicId, user1.Id);

            Assert.IsTrue(result);

            this.favoriteRepositoryMock.Verify(r => r.HardDeleteAsync(existingFavorite), Times.Once);
        }

        [Test]
        public async Task UnlikeTrack_Should_ReturnTrue_When_FavoriteDoesNotExist()
        {
            this.trackRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Track, bool>>>())).ReturnsAsync(track1);
            this.favoriteRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<UserFavorite, bool>>>())).ReturnsAsync((UserFavorite?)null);

            var result = await this.favoritesService
                .UnlikeTrackAsync(track1.PublicId, user1.Id);

            Assert.IsTrue(result);

            this.favoriteRepositoryMock.Verify(r => r.HardDeleteAsync(It.IsAny<UserFavorite>()), Times.Never);
        }
    }
}
