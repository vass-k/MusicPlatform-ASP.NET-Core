namespace MusicPlatform.Services.Core.Admin
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using MusicPlatform.Data.Models;
    using MusicPlatform.Data.Repository.Interfaces;
    using MusicPlatform.Services.Core.Admin.Interfaces;
    using MusicPlatform.Web.ViewModels.Admin.Dashboard;

    using System.Globalization;

    using static MusicPlatform.GCommon.ApplicationConstants;

    public class DashboardService : IDashboardService
    {
        private readonly ITrackRepository trackRepository;
        private readonly IPlaylistRepository playlistRepository;
        private readonly UserManager<AppUser> userManager;

        public DashboardService(
            ITrackRepository trackRepository,
            IPlaylistRepository playlistRepository,
            UserManager<AppUser> userManager)
        {
            this.trackRepository = trackRepository;
            this.playlistRepository = playlistRepository;
            this.userManager = userManager;
        }

        public async Task<DashboardViewModel> GetDashboardDataAsync()
        {
            int tracksCount = await this.trackRepository.CountAsync();
            int playlistsCount = await this.playlistRepository.CountAsync();
            int usersCount = await this.userManager.Users.CountAsync();

            TrackUploadsChartData chartData = await this
                .GetLastThreeMonthsTrackUploadsAsync();

            return new DashboardViewModel
            {
                TotalTracksCount = tracksCount,
                TotalPlaylistsCount = playlistsCount,
                TotalUsersCount = usersCount,
                ChartData = chartData
            };
        }

        private async Task<TrackUploadsChartData> GetLastThreeMonthsTrackUploadsAsync()
        {
            DateTime today = DateTime.UtcNow;
            DateTime threeMonthsAgo = today.AddMonths(-2).Date;

            var monthlyUploads = await this.trackRepository
                .GetAllAsQueryable()
                .AsNoTracking()
                .Where(t => t.CreatedOn >= threeMonthsAgo)
                .GroupBy(t => new
                {
                    t.CreatedOn.Year,
                    t.CreatedOn.Month
                })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    Count = g.Count()
                })
                .ToListAsync();

            List<string> monthLabels = new List<string>();
            List<int> trackCounts = new List<int>();

            for (int months = 2; months >= 0; months--)
            {
                DateTime date = today.AddMonths(-months);
                monthLabels.Add(date.ToString(DateTimeMonthFormat, CultureInfo.InvariantCulture));

                var monthData = monthlyUploads
                    .FirstOrDefault(m => m.Year == date.Year
                                      && m.Month == date.Month);

                trackCounts.Add(monthData?.Count ?? 0);
            }

            return new TrackUploadsChartData
            {
                MonthLabels = monthLabels,
                TrackCounts = trackCounts
            };
        }
    }
}
