namespace MusicPlatform.Web
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    using MusicPlatform.Data;
    using MusicPlatform.Data.Models;
    using MusicPlatform.Data.Repository;
    using MusicPlatform.Data.Repository.Interfaces;
    using MusicPlatform.Data.Seeding;
    using MusicPlatform.Data.Seeding.Interfaces;
    using MusicPlatform.Services.Common.Interfaces;
    using MusicPlatform.Services.Core;
    using MusicPlatform.Services.Core.Admin;
    using MusicPlatform.Services.Core.Admin.Interfaces;
    using MusicPlatform.Services.Core.Interfaces;
    using MusicPlatform.Web.Infrastructure.Configuration;
    using MusicPlatform.Web.Infrastructure.Extensions;
    using MusicPlatform.Web.Infrastructure.Services;

    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Services to container
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<MusicPlatformDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<AppUser>(options =>
            {
                ConfigureIdentity(builder.Configuration, options);
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<MusicPlatformDbContext>();

            builder.Services.AddTransient<IIdentitySeeder, IdentitySeeder>();

            builder.Services.AddScoped<ITrackRepository, TrackRepository>();
            builder.Services.AddScoped<IPlaylistRepository, PlaylistRepository>();
            builder.Services.AddScoped<IGenreRepository, GenreRepository>();
            builder.Services.AddScoped<ICommentRepository, CommentRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserFavoriteRepository, UserFavoriteRepository>();
            builder.Services.AddScoped<IPlaylistTrackRepository, PlaylistTrackRepository>();

            builder.Services.AddScoped<ITrackService, TrackService>();
            builder.Services.AddScoped<ICloudStorageService, CloudinaryService>();
            builder.Services.AddScoped<ICommentService, CommentService>();
            builder.Services.AddScoped<IProfileService, ProfileService>();
            builder.Services.AddScoped<IPlaylistService, PlaylistService>();
            builder.Services.AddScoped<IGenreService, GenreService>();
            builder.Services.AddScoped<IFavoritesService, FavoritesService>();
            builder.Services.AddScoped<IPlaylistTracksService, PlaylistTracksService>();
            builder.Services.AddScoped<IGenreManagementService, GenreManagementService>();
            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

            builder.Services.AddControllersWithViews();

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithRedirects("/Home/Error?statusCode={0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.SeedDefaultIdentity();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area}/{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }

        private static void ConfigureIdentity(IConfiguration configuration, IdentityOptions identityOptions)
        {
            identityOptions.SignIn.RequireConfirmedEmail =
                configuration.GetValue<bool>("IdentityConfig:SignIn:RequireConfirmedEmail");
            identityOptions.Password.RequiredLength =
                configuration.GetValue<int>("IdentityConfig:Password:RequiredLength");
            identityOptions.Password.RequireNonAlphanumeric =
                configuration.GetValue<bool>("IdentityConfig:Password:RequireNonAlphanumeric");

            identityOptions.Password.RequiredLength =
                configuration.GetValue<int>($"IdentityConfig:Password:RequiredLength");
            identityOptions.Password.RequireNonAlphanumeric =
                configuration.GetValue<bool>($"IdentityConfig:Password:RequireNonAlphanumeric");
            identityOptions.Password.RequireDigit =
                configuration.GetValue<bool>($"IdentityConfig:Password:RequireDigit");
            identityOptions.Password.RequireLowercase =
                configuration.GetValue<bool>($"IdentityConfig:Password:RequireLowercase");
            identityOptions.Password.RequireUppercase =
                configuration.GetValue<bool>($"IdentityConfig:Password:RequireUppercase");
            identityOptions.Password.RequiredUniqueChars =
                configuration.GetValue<int>($"IdentityConfig:Password:RequiredUniqueChars");
        }
    }
}
