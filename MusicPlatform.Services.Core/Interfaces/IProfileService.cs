namespace MusicPlatform.Services.Core.Interfaces
{
    using MusicPlatform.Web.ViewModels.Profile;

    public interface IProfileService
    {
        Task<ProfileViewModel?> GetUserProfileAsync(string username, int pageNumber, int pageSize, string? currentUserId);
    }
}
