namespace MusicPlatform.Services.Tests
{
    using Microsoft.AspNetCore.Identity;

    using MockQueryable;
    using Moq;

    using MusicPlatform.Data.Models;
    using MusicPlatform.Data.Repository.Interfaces;
    using MusicPlatform.Services.Core.Admin;
    using MusicPlatform.Services.Core.Admin.Interfaces;

    using System.Globalization;

    using static MusicPlatform.GCommon.ApplicationConstants;

    [TestFixture]
    public class DashboardServiceTests
    {
        private Mock<ITrackRepository> trackRepositoryMock;
        private Mock<IPlaylistRepository> playlistRepositoryMock;
        private Mock<UserManager<AppUser>> userManagerMock;

        private IDashboardService dashboardService;

        [SetUp]
        public void Setup()
        {
            this.trackRepositoryMock = new Mock<ITrackRepository>(MockBehavior.Strict);
            this.playlistRepositoryMock = new Mock<IPlaylistRepository>(MockBehavior.Strict);

            var store = new Mock<IUserStore<AppUser>>();
            this.userManagerMock = new Mock<UserManager<AppUser>>(store.Object, null, null, null, null, null, null, null, null);

            this.dashboardService = new DashboardService(
                this.trackRepositoryMock.Object,
                this.playlistRepositoryMock.Object,
                this.userManagerMock.Object);
        }

        [Test]
        public async Task GetDashboardData_Should_ReturnCorrectTotalCounts()
        {
            var mockUsers = new List<AppUser>
            {
                new AppUser(),
                new AppUser()
            }.BuildMock();

            this.trackRepositoryMock.Setup(r => r.CountAsync()).ReturnsAsync(12);
            this.playlistRepositoryMock.Setup(r => r.CountAsync()).ReturnsAsync(5);
            this.userManagerMock.Setup(um => um.Users).Returns(mockUsers);

            var emptyTracks = new List<Track>().BuildMock();
            this.trackRepositoryMock.Setup(r => r.GetAllAsQueryable()).Returns(emptyTracks);

            var result = await this.dashboardService
                .GetDashboardDataAsync();

            Assert.IsNotNull(result);
            Assert.That(result.TotalTracksCount, Is.EqualTo(12));
            Assert.That(result.TotalPlaylistsCount, Is.EqualTo(5));
            Assert.That(result.TotalUsersCount, Is.EqualTo(2));
        }

        [Test]
        public async Task GetDashboardData_Chart_Should_HaveCorrectMonthLabels()
        {
            this.trackRepositoryMock.Setup(r => r.CountAsync()).ReturnsAsync(0);
            this.playlistRepositoryMock.Setup(r => r.CountAsync()).ReturnsAsync(0);
            this.userManagerMock.Setup(um => um.Users).Returns(new List<AppUser>().BuildMock());
            this.trackRepositoryMock.Setup(r => r.GetAllAsQueryable()).Returns(new List<Track>().BuildMock());

            var result = await this.dashboardService
                .GetDashboardDataAsync();

            var today = DateTime.UtcNow;
            var expectedLabels = new[]
            {
                today.AddMonths(-2).ToString(DateTimeMonthFormat, CultureInfo.InvariantCulture),
                today.AddMonths(-1).ToString(DateTimeMonthFormat, CultureInfo.InvariantCulture),
                today.ToString(DateTimeMonthFormat, CultureInfo.InvariantCulture)
            };

            Assert.IsNotNull(result.ChartData);
            CollectionAssert.AreEqual(expectedLabels, result.ChartData.MonthLabels);
        }

        [Test]
        public async Task GetDashboardData_Chart_Should_ContainAllZeros_When_NoRecentUploads()
        {
            this.trackRepositoryMock.Setup(r => r.CountAsync()).ReturnsAsync(10);
            this.playlistRepositoryMock.Setup(r => r.CountAsync()).ReturnsAsync(5);
            this.userManagerMock.Setup(um => um.Users).Returns(new List<AppUser>().BuildMock());

            var oldTracks = new List<Track>
            {
                new Track
                {
                    CreatedOn = DateTime.UtcNow.AddMonths(-4)
                }
            }.BuildMock();

            this.trackRepositoryMock.Setup(r => r.GetAllAsQueryable()).Returns(oldTracks);

            var result = await this.dashboardService
                .GetDashboardDataAsync();

            var expectedCounts = new[] { 0, 0, 0 };

            Assert.IsNotNull(result.ChartData);
            CollectionAssert.AreEqual(expectedCounts, result.ChartData.TrackCounts);
        }
    }
}