namespace MusicPlatform.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using MusicPlatform.Data.Models;

    using System.Reflection;

    public class MusicPlatformDbContext : IdentityDbContext<AppUser>
    {
        public MusicPlatformDbContext(DbContextOptions<MusicPlatformDbContext> options)
            : base(options)
        { }

        public new virtual DbSet<AppUser> Users { get; set; } = null!;

        public virtual DbSet<Genre> Genres { get; set; } = null!;

        public virtual DbSet<Track> Tracks { get; set; } = null!;

        public virtual DbSet<Playlist> Playlists { get; set; } = null!;

        public virtual DbSet<PlaylistTrack> PlaylistTracks { get; set; } = null!;

        public virtual DbSet<UserFavorite> UserFavorites { get; set; } = null!;

        public virtual DbSet<Comment> Comments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder
                .ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
