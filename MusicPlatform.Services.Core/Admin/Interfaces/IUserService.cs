namespace MusicPlatform.Services.Core.Admin.Interfaces
{
    using MusicPlatform.Web.ViewModels.Admin.UserManagement;

    public interface IUserService
    {
        Task<IEnumerable<UserManagementIndexViewModel>> GetUserManagementBoardDataAsync(string userId);
    }
}
