namespace MusicPlatform.Web.ViewModels.Track
{
    public class TrackIndexViewModel
    {
        public Guid PublicId { get; set; }

        public string Title { get; set; } = null!;

        public string ArtistName { get; set; } = null!;

        public int Plays { get; set; }

        public int FavoritesCount { get; set; }
    }
}
