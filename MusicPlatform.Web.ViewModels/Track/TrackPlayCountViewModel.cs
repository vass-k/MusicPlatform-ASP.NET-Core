namespace MusicPlatform.Web.ViewModels.Track
{
    using System.ComponentModel.DataAnnotations;

    public class TrackPlayCountViewModel
    {
        [Required]
        public Guid TrackId { get; set; }
    }
}
