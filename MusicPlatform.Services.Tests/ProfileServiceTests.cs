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
    public class ProfileServiceTests
    {
        private Mock<IUserRepository> userRepositoryMock;
        private Mock<ITrackRepository> trackRepositoryMock;
        private Mock<IPlaylistRepository> playlistRepositoryMock;
        private Mock<IUserFavoriteRepository> favoriteRepositoryMock;

        private IProfileService profileService;

        private AppUser user1;
        private AppUser user2;
        private List<Track> tracks;
        private List<Playlist> playlists;
        private List<UserFavorite> favorites;

        [SetUp]
        public void Setup()
        {
            this.userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            this.trackRepositoryMock = new Mock<ITrackRepository>(MockBehavior.Strict);
            this.playlistRepositoryMock = new Mock<IPlaylistRepository>(MockBehavior.Strict);
            this.favoriteRepositoryMock = new Mock<IUserFavoriteRepository>(MockBehavior.Strict);

            this.profileService = new ProfileService(
                this.userRepositoryMock.Object,
                this.trackRepositoryMock.Object,
                this.playlistRepositoryMock.Object,
                this.favoriteRepositoryMock.Object);

            user1 = new AppUser
            {
                Id = "user1-id",
                UserName = "UserOne",
                NormalizedUserName = "USERONE"
            };
            user2 = new AppUser
            {
                Id = "user2-id",
                UserName = "UserTwo",
                NormalizedUserName = "USERTWO"
            };

            tracks = new List<Track>
            {
                new Track
                {
                    UploaderId = "user1-id",
                    Title = "UserOneTrack"
                },
                new Track
                {
                    UploaderId = "user2-id",
                    Title = "UserTwoTrack"
                }
            };

            playlists = new List<Playlist>
            {
                new Playlist
                {
                    CreatorId = "user1-id",
                    Name = "UserOnePlaylist",
                    IsPublic = true
                },
                new Playlist
                {
                    CreatorId = "user1-id",
                    Name = "UserOnePrivatePlaylist",
                    IsPublic = false
                }
            };

            favorites = new List<UserFavorite>
            {
                new UserFavorite
                {
                    UserId = "user1-id",
                    Track = new Track
                    {
                        Title = "FavoriteTrack1"
                    }
                }
            };
        }

        [Test]
        public async Task GetUserProfile_Should_ReturnNull_When_UserNotFound()
        {
            var emptyUserList = new List<AppUser>();
            var userQueryable = emptyUserList.BuildMock();
            this.userRepositoryMock
                .Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<AppUser, bool>>>()))
                .ReturnsAsync((AppUser)null);

            var result = await this.profileService
                .GetUserProfileAsync("NonExistentUser", "Tracks", 1, 10, null);

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetUserProfile_Should_SetIsCurrentUserProfile_Correctly()
        {
            var userQueryable = new List<AppUser>
            {
                user1
            }.BuildMock();

            this.userRepositoryMock
                .Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<AppUser, bool>>>()))
                .ReturnsAsync(user1);

            this.trackRepositoryMock.Setup(r => r.GetAllAsQueryable()).Returns(new List<Track>().BuildMock());

            var resultOwner = await this.profileService
                .GetUserProfileAsync("UserOne", "Tracks", 1, 10, "user1-id");

            var resultVisitor = await this.profileService
                .GetUserProfileAsync("UserOne", "Tracks", 1, 10, "user2-id");

            Assert.IsTrue(resultOwner.IsCurrentUserProfile);
            Assert.IsFalse(resultVisitor.IsCurrentUserProfile);
        }

        [Test]
        public async Task GetUserProfile_TracksTab_Should_ReturnOnlyOwnedTracks()
        {
            this.userRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<AppUser, bool>>>())).ReturnsAsync(user1);
            var tracksQueryable = tracks.BuildMock();
            this.trackRepositoryMock.Setup(r => r.GetAllAsQueryable()).Returns(tracksQueryable);

            var result = await this.profileService
                .GetUserProfileAsync("UserOne", "Tracks", 1, 10, "user1-id");

            Assert.IsNotNull(result.UploadedTracks);
            Assert.That(result.UploadedTracks.Items.Count(), Is.EqualTo(1));
            Assert.That(result.UploadedTracks.Items.First().Title, Is.EqualTo("UserOneTrack"));
        }

        [Test]
        public async Task GetUserProfile_PlaylistsTab_Should_ReturnAllPlaylists_ForOwner()
        {
            this.userRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<AppUser, bool>>>())).ReturnsAsync(user1);
            var playlistQueryable = playlists.BuildMock();
            this.playlistRepositoryMock.Setup(r => r.GetAllAsQueryable()).Returns(playlistQueryable);

            var result = await this.profileService
                .GetUserProfileAsync("UserOne", "Playlists", 1, 10, "user1-id");

            Assert.IsNotNull(result.Playlists);
            Assert.That(result.Playlists.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetUserProfile_PlaylistsTab_Should_ReturnOnlyPublicPlaylists_ForVisitor()
        {
            this.userRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<AppUser, bool>>>())).ReturnsAsync(user1);
            var playlistQueryable = playlists.BuildMock();
            this.playlistRepositoryMock.Setup(r => r.GetAllAsQueryable()).Returns(playlistQueryable);

            var result = await this.profileService
                .GetUserProfileAsync("UserOne", "Playlists", 1, 10, "user2-id");

            Assert.IsNotNull(result.Playlists);
            Assert.That(result.Playlists.Count(), Is.EqualTo(1));
            Assert.That(result.Playlists.First().Name, Is.EqualTo("UserOnePlaylist"));
        }

        [Test]
        public async Task GetUserProfile_FavoritesTab_Should_ReturnFavoritedTracks()
        {
            this.userRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<AppUser, bool>>>())).ReturnsAsync(user1);
            var favoritesQueryable = favorites.BuildMock();
            this.favoriteRepositoryMock.Setup(r => r.GetAllAsQueryable()).Returns(favoritesQueryable);

            var result = await this.profileService
                .GetUserProfileAsync("UserOne", "Favorites", 1, 10, "user1-id");

            Assert.IsNotNull(result.FavoriteTracks);
            Assert.That(result.FavoriteTracks.Items.Count(), Is.EqualTo(1));
            Assert.That(result.FavoriteTracks.Items.First().Title, Is.EqualTo("FavoriteTrack1"));
        }
    }
}
