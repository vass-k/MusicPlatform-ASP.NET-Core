namespace MusicPlatform.Web.ViewModels.Comment
{
    public class CommentViewModel
    {
        public string AuthorUsername { get; set; } = null!;

        public string Content { get; set; } = null!;

        public string AuthorAvatarUrl { get; set; } = "images/musicplatform-hero.jpg";

        public string PostedOn { get; set; } = null!;
    }
}
