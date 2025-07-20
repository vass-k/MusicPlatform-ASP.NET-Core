namespace MusicPlatform.Web.ViewModels.Genre
{
    public class GenreIndexViewModel
    {
        public Guid PublicId { get; set; }

        public string Name { get; set; } = null!;

        public int TrackCount { get; set; }
    }
}
