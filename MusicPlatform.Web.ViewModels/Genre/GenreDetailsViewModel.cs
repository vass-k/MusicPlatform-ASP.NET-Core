namespace MusicPlatform.Web.ViewModels.Genre
{
    using MusicPlatform.Web.ViewModels.Track;

    public class GenreDetailsViewModel
    {
        public string GenreName { get; set; } = null!;

        public PagedResult<TrackIndexViewModel> PagedTracks { get; set; } = null!;
    }
}
