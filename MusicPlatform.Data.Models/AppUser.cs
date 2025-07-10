namespace MusicPlatform.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    [Comment("This entity is a user of the application, extending the default IdentityUser.")]
    public class AppUser : IdentityUser
    {
        [Comment("A collection of tracks uploaded by the user.")]
        public virtual ICollection<Track> UploadedTracks { get; set; }
            = new HashSet<Track>();

        [Comment("A collection of playlists created by the user.")]
        public virtual ICollection<Playlist> Playlists { get; set; }
            = new HashSet<Playlist>();

        [Comment("A collection of the user's favorite tracks.")]
        public virtual ICollection<UserFavorite> FavoriteTracks { get; set; }
            = new HashSet<UserFavorite>();

        [Comment("A collection of comments posted by the user.")]
        public virtual ICollection<Comment> Comments { get; set; }
            = new HashSet<Comment>();
    }
}
