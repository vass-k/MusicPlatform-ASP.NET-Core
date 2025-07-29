namespace MusicPlatform.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using MusicPlatform.Data.Models;

    public class UserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> entity)
        {
            entity
                .HasData(this.SeedDefaultUser());
        }

        private AppUser SeedDefaultUser()
        {
            AppUser defaultUser = new AppUser
            {
                Id = "ef6eeff1-af06-48e3-8c83-703cf53d5ba9",

                UserName = "default@music.com",
                NormalizedUserName = "DEFAULT@MUSIC.COM",
                Email = "default@music.com",
                NormalizedEmail = "DEFAULT@MUSIC.COM",
                EmailConfirmed = true,

                PasswordHash = "AQAAAAIAAYagAAAAEPbvz3hUaf/q7G77Gmyk3Ha5nIzbd23lrVLVgO4iClRwqJt/GJtQducTsWrddTom3Q==",
                SecurityStamp = "QTQXHOC5V5FS6EVBCHVWL2EMYYO3MSRA",
                ConcurrencyStamp = "693da562-ddf1-4c65-8149-1cfb8222748c",

                PhoneNumber = null,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnd = null,
                LockoutEnabled = false,
                AccessFailedCount = 0
            };

            return defaultUser;
        }
    }
}
