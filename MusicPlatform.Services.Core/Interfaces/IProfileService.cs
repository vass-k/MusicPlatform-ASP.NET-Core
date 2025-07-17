namespace MusicPlatform.Services.Core.Interfaces
{
    using MusicPlatform.Web.ViewModels.Profile;

    public interface IProfileService
    {
        Task<ProfileViewModel?> GetUserProfileAsync(string username, string activeTab, int pageNumber, int ItemsPerPage, string? currentUserId);
    }
}
