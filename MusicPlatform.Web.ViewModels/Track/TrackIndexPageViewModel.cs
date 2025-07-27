namespace MusicPlatform.Web.ViewModels.Track
{
    public class TrackIndexPageViewModel
    {
        public PagedResult<TrackIndexViewModel> PagedTracks { get; set; } = new();

        public string? SearchString { get; set; }
    }
}
