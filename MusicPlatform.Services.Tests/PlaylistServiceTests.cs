namespace MusicPlatform.Services.Tests
{
    using MockQueryable;
    using Moq;

    using MusicPlatform.Data.Models;
    using MusicPlatform.Data.Repository.Interfaces;
    using MusicPlatform.Services.Core;
    using MusicPlatform.Services.Core.Interfaces;
    using MusicPlatform.Web.ViewModels.Playlist;

    using System.Linq.Expressions;

    [TestFixture]
    public class PlaylistServiceTests
    {
        private Mock<IPlaylistRepository> playlistRepositoryMock;

        private IPlaylistService playlistService;

        private AppUser user1;
        private List<Playlist> mockPlaylists;

        [SetUp]
        public void Setup()
        {
            this.playlistRepositoryMock = new Mock<IPlaylistRepository>(MockBehavior.Strict);
            this.playlistService = new PlaylistService(this.playlistRepositoryMock.Object);

            user1 = new AppUser
            {
                Id = "user1-id",
                UserName = "UserOne"
            };

            mockPlaylists = new List<Playlist>
            {
                new Playlist
                {
                    Id = 1,
                    PublicId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "My Test Playlist",
                    CreatorId = "user1-id",
                    Creator = user1,
                    PlaylistTracks = new List<PlaylistTrack>
                    {
                        new PlaylistTrack
                        {
                            Track = new Track
                            {
                                DurationInSeconds = 180
                            }
                        }
                    }
                }
            };
        }

        [Test]
        public async Task CreatePlaylist_Should_CallRepositoryAdd()
        {
            var createModel = new PlaylistCreateViewModel
            {
                Name = "New Playlist"
            };

            var userId = "user1-id";

            this.playlistRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Playlist>()))
                .Returns(Task.CompletedTask);

            await this.playlistService
                .CreatePlaylistAsync(createModel, userId);

            this.playlistRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Playlist>()), Times.Once);
        }

        [Test]
        public async Task GetPlaylistDetails_Should_ReturnNull_When_PlaylistNotFound()
        {
            var emptyList = new List<Playlist>();
            var emptyQueryable = emptyList.BuildMock();

            this.playlistRepositoryMock
                .Setup(r => r.GetAllAsQueryable())
                .Returns(emptyQueryable);

            var result = await this.playlistService
                .GetPlaylistDetailsAsync(Guid.NewGuid(), "any-user-id");

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetPlaylistDetails_Should_ReturnCorrectlyMappedViewModel()
        {
            var playlistQueryable = mockPlaylists.BuildMock();
            this.playlistRepositoryMock
                .Setup(r => r.GetAllAsQueryable())
                .Returns(playlistQueryable);

            var result = await this.playlistService
                .GetPlaylistDetailsAsync(mockPlaylists[0].PublicId, "user1-id");

            Assert.IsNotNull(result);
            Assert.That(result.Name, Is.EqualTo("My Test Playlist"));
            Assert.That(result.CreatorUsername, Is.EqualTo("UserOne"));
            Assert.That(result.TotalDurationSec, Is.EqualTo(180));
            Assert.IsTrue(result.IsOwnedByCurrentUser);
        }

        [Test]
        public async Task GetPlaylistForEdit_Should_ReturnNull_When_UserIsNotOwner()
        {
            var playlist = mockPlaylists[0];
            var nonOwnerId = "user2-id";

            this.playlistRepositoryMock
                .Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Playlist, bool>>>()))
                .ReturnsAsync((Expression<Func<Playlist, bool>> predicate) => mockPlaylists.FirstOrDefault(predicate.Compile()));

            var result = await this.playlistService
                .GetPlaylistForEditAsync(playlist.PublicId, nonOwnerId);

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetPlaylistForEdit_Should_ReturnViewModel_When_UserIsOwner()
        {
            var playlist = mockPlaylists[0];

            this.playlistRepositoryMock
                .Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Playlist, bool>>>()))
                .ReturnsAsync(playlist);

            var result = await this.playlistService
                .GetPlaylistForEditAsync(playlist.PublicId, "user1-id");

            Assert.IsNotNull(result);
            Assert.That(result.Name, Is.EqualTo(playlist.Name));
            Assert.That(result.PublicId, Is.EqualTo(playlist.PublicId));
        }

        [Test]
        public async Task UpdatePlaylist_Should_ReturnFalse_When_UserIsNotOwner()
        {
            var editModel = new PlaylistEditViewModel { PublicId = mockPlaylists[0].PublicId, Name = "Updated Name" };
            var nonOwnerId = "user2-id";

            this.playlistRepositoryMock
                .Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Playlist, bool>>>()))
                .ReturnsAsync(mockPlaylists[0]);

            var result = await this.playlistService
                .UpdatePlaylistAsync(editModel, nonOwnerId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeletePlaylist_Should_CallRepositoryDelete_When_UserIsOwner()
        {
            var playlist = mockPlaylists[0];
            this.playlistRepositoryMock
                .Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Playlist, bool>>>()))
                .ReturnsAsync(playlist);

            this.playlistRepositoryMock
                .Setup(r => r.DeleteAsync(playlist))
                .ReturnsAsync(true);

            var result = await this.playlistService
                .DeletePlaylistAsync(playlist.PublicId, "user1-id");

            Assert.IsTrue(result);

            this.playlistRepositoryMock.Verify(r => r.DeleteAsync(playlist), Times.Once);
        }
    }
}
