namespace MusicPlatform.Web.ViewModels.Profile
{
    public class ProfileTrackViewModel
    {
        public Guid PublicId { get; set; }

        public string Title { get; set; } = null!;

        public string ArtistName { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public string AudioUrl { get; set; } = null!;
    }
}
