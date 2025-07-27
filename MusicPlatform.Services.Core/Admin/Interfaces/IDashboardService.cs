namespace MusicPlatform.Services.Core.Admin.Interfaces
{
    using MusicPlatform.Web.ViewModels.Admin.Dashboard;

    public interface IDashboardService
    {
        Task<DashboardViewModel> GetDashboardDataAsync();
    }
}
