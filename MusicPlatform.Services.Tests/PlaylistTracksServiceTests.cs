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
    public class PlaylistTracksServiceTests
    {
        private Mock<IPlaylistRepository> playlistRepositoryMock;
        private Mock<ITrackRepository> trackRepositoryMock;
        private Mock<IPlaylistTrackRepository> playlistTrackRepositoryMock;

        private IPlaylistTracksService playlistTracksService;

        private AppUser user1;
        private Track track1;
        private Playlist playlist1;
        private Playlist playlist2;

        [SetUp]
        public void Setup()
        {
            this.playlistRepositoryMock = new Mock<IPlaylistRepository>(MockBehavior.Strict);
            this.trackRepositoryMock = new Mock<ITrackRepository>(MockBehavior.Strict);
            this.playlistTrackRepositoryMock = new Mock<IPlaylistTrackRepository>(MockBehavior.Strict);

            this.playlistTracksService = new PlaylistTracksService(
                this.playlistRepositoryMock.Object,
                this.trackRepositoryMock.Object,
                this.playlistTrackRepositoryMock.Object);

            user1 = new AppUser
            {
                Id = "user1-id",
                UserName = "UserOne"
            };
            track1 = new Track
            {
                Id = 101,
                PublicId = Guid.NewGuid(),
                Title = "Test Track"
            };
            playlist1 = new Playlist
            {
                Id = 1,
                PublicId = Guid.NewGuid(),
                Name = "Playlist A",
                CreatorId = "user1-id"
            };
            playlist2 = new Playlist
            {
                Id = 2,
                PublicId = Guid.NewGuid(),
                Name = "Playlist B",
                CreatorId = "user1-id"
            };
        }

        [Test]
        public async Task GetUserPlaylistsForTrack_Should_CorrectlyMarkExistingPlaylist()
        {
            var playlists = new List<Playlist>
            {
                playlist1,
                playlist2
            };

            var playlistTracks = new List<PlaylistTrack>
            {
                new PlaylistTrack
                {
                    PlaylistId = 1,
                    TrackId = 101
                }
            };

            this.trackRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Track, bool>>>())).ReturnsAsync(track1);
            this.playlistTrackRepositoryMock.Setup(r => r.GetAllAsQueryable()).Returns(playlistTracks.BuildMock());
            this.playlistRepositoryMock.Setup(r => r.GetAllAsQueryable()).Returns(playlists.BuildMock());

            var result = await this.playlistTracksService
                .GetUserPlaylistsAsync(track1.PublicId, "user1-id");
            var playlistA = result.First(p => p.PlaylistName == "Playlist A");
            var playlistB = result.First(p => p.PlaylistName == "Playlist B");

            Assert.IsTrue(playlistA.IsTrackAlreadyInPlaylist);
            Assert.IsFalse(playlistB.IsTrackAlreadyInPlaylist);
        }

        [Test]
        public void AddTrackToPlaylist_Should_Throw_When_UserIsNotOwner()
        {
            var nonOwnerId = "user2-id";

            this.playlistRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Playlist, bool>>>())).ReturnsAsync(playlist1);
            this.trackRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Track, bool>>>())).ReturnsAsync(track1);

            Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
            {
                await this.playlistTracksService
                .AddTrackToPlaylistAsync(track1.PublicId, playlist1.PublicId, nonOwnerId);
            });
        }

        [Test]
        public async Task AddTrackToPlaylist_Should_NotAddDuplicateTrack()
        {
            var existingEntry = new PlaylistTrack
            {
                PlaylistId = playlist1.Id,
                TrackId = track1.Id
            };

            this.playlistRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Playlist, bool>>>())).ReturnsAsync(playlist1);
            this.trackRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Track, bool>>>())).ReturnsAsync(track1);
            this.playlistTrackRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<PlaylistTrack, bool>>>())).ReturnsAsync(existingEntry);

            await this.playlistTracksService
                .AddTrackToPlaylistAsync(track1.PublicId, playlist1.PublicId, "user1-id");

            this.playlistTrackRepositoryMock.Verify(r => r.AddAsync(It.IsAny<PlaylistTrack>()), Times.Never);
        }

        [Test]
        public async Task AddTrackToPlaylist_Should_CallRepositoryAdd_ForNewTrack()
        {
            this.playlistRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Playlist, bool>>>())).ReturnsAsync(playlist1);
            this.trackRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Track, bool>>>())).ReturnsAsync(track1);

            this.playlistTrackRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<PlaylistTrack, bool>>>())).ReturnsAsync((PlaylistTrack?)null);
            this.playlistTrackRepositoryMock.Setup(r => r.AddAsync(It.IsAny<PlaylistTrack>())).Returns(Task.CompletedTask);

            await this.playlistTracksService
                .AddTrackToPlaylistAsync(track1.PublicId, playlist1.PublicId, "user1-id");

            this.playlistTrackRepositoryMock.Verify(r => r.AddAsync(It.IsAny<PlaylistTrack>()), Times.Once);
        }

        [Test]
        public void RemoveTrackFromPlaylist_Should_Throw_When_UserIsNotOwner()
        {
            var nonOwnerId = "user2-id";

            this.playlistRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Playlist, bool>>>())).ReturnsAsync(playlist1);
            this.trackRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Track, bool>>>())).ReturnsAsync(track1);

            Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
            {
                await this.playlistTracksService
                .RemoveTrackFromPlaylistAsync(track1.PublicId, playlist1.PublicId, nonOwnerId);
            });
        }

        [Test]
        public async Task RemoveTrackFromPlaylist_Should_ReturnFalse_When_TrackOrPlaylistNotFound()
        {
            this.playlistRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Playlist, bool>>>())).ReturnsAsync((Playlist?)null);
            this.trackRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Track, bool>>>())).ReturnsAsync(track1);

            var result = await this.playlistTracksService
                .RemoveTrackFromPlaylistAsync(track1.PublicId, playlist1.PublicId, "user1-id");

            Assert.IsFalse(result);
        }

        [Test]
        public async Task RemoveTrackFromPlaylist_Should_CallRepositoryHardDelete_When_EntryExists()
        {
            var existingEntry = new PlaylistTrack { PlaylistId = playlist1.Id, TrackId = track1.Id };
            this.playlistRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Playlist, bool>>>())).ReturnsAsync(playlist1);
            this.trackRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Track, bool>>>())).ReturnsAsync(track1);
            this.playlistTrackRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<PlaylistTrack, bool>>>())).ReturnsAsync(existingEntry);
            this.playlistTrackRepositoryMock.Setup(r => r.HardDeleteAsync(existingEntry)).ReturnsAsync(true);

            await this.playlistTracksService
                .RemoveTrackFromPlaylistAsync(track1.PublicId, playlist1.PublicId, "user1-id");

            this.playlistTrackRepositoryMock.Verify(r => r.HardDeleteAsync(existingEntry), Times.Once);
        }
    }
}
