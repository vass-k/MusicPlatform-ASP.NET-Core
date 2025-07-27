namespace MusicPlatform.Web.ViewModels.Admin.Dashboard
{
    public class TrackUploadsChartData
    {
        public IEnumerable<string> MonthLabels { get; set; }
            = new List<string>();

        public IEnumerable<int> TrackCounts { get; set; }
            = new List<int>();
    }
}
