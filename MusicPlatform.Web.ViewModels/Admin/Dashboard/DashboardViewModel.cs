namespace MusicPlatform.Web.ViewModels.Admin.Dashboard
{
    public class DashboardViewModel
    {
        public int TotalTracksCount { get; set; }

        public int TotalUsersCount { get; set; }

        public int TotalPlaylistsCount { get; set; }

        public TrackUploadsChartData ChartData { get; set; }
            = new TrackUploadsChartData();
    }
}
