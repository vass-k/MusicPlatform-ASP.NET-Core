namespace MusicPlatform.Web.ViewModels.Track
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Rendering;

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static MusicPlatform.Data.Common.EntityConstants.Track;
    using static MusicPlatform.Web.ViewModels.ValidationMessages.Track;

    public class TrackAddViewModel
    {
        [Required(ErrorMessage = TitleRequired)]
        [StringLength(TitleMaxLength,
            MinimumLength = TitleMinLength,
            ErrorMessage = TitleLength)]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = ArtistNameRequired)]
        [StringLength(ArtistNameMaxLength,
            MinimumLength = ArtistNameMinLength,
            ErrorMessage = ArtistNameLength)]
        [Display(Name = "Artist Name")]
        public string ArtistName { get; set; } = null!;

        [Required(ErrorMessage = GenreRequired)]
        [Range(1, int.MaxValue, ErrorMessage = GenreRequired)]
        [Display(Name = "Genre")]
        public int GenreId { get; set; }

        [Required(ErrorMessage = AudioFileRequired)]
        [Display(Name = "Audio File (MP3, WAV, FLAC)")]
        public IFormFile AudioFile { get; set; } = null!;

        [Display(Name = "Cover Art (Optional)")]
        public IFormFile? ImageFile { get; set; }

        public IEnumerable<SelectListItem> Genres { get; set; }
            = new List<SelectListItem>();
    }
}
