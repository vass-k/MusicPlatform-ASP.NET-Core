namespace MusicPlatform.Services.Core.Admin
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using MusicPlatform.Data.Models;
    using MusicPlatform.Services.Core.Admin.Interfaces;
    using MusicPlatform.Web.ViewModels.Admin.UserManagement;

    using static MusicPlatform.GCommon.ApplicationConstants;

    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IEnumerable<UserManagementIndexViewModel>> GetUserManagementBoardDataAsync(string userId)
        {
            IEnumerable<UserManagementIndexViewModel> users = await this.userManager
                .Users
                .Where(u => u.Id.ToLower() != userId.ToLower())
                .Select(u => new UserManagementIndexViewModel
                {
                    Id = u.Id,
                    Email = u.Email ?? "No Email",
                    Roles = userManager.GetRolesAsync(u)
                        .GetAwaiter()
                        .GetResult()
                })
                .ToArrayAsync();

            return users;
        }

        public async Task<bool> MakeUserAdminAsync(string userId)
        {
            AppUser? user = await this.userManager
                .FindByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User does not exist!");
            }

            if (await this.userManager.IsInRoleAsync(user, UserRoleName))
            {
                await this.userManager.RemoveFromRoleAsync(user, UserRoleName);
            }

            IdentityResult result = await this.userManager
                .AddToRoleAsync(user, AdminRoleName);

            return result.Succeeded;
        }
    }
}
