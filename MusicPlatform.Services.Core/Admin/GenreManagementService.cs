namespace MusicPlatform.Services.Core.Admin
{
    using MusicPlatform.Data.Models;
    using MusicPlatform.Data.Repository.Interfaces;
    using MusicPlatform.Services.Core.Admin.Interfaces;
    using MusicPlatform.Web.ViewModels.Genre;

    public class GenreManagementService : IGenreManagementService
    {
        private readonly IGenreRepository genreRepository;

        public GenreManagementService(IGenreRepository genreRepository)
        {
            this.genreRepository = genreRepository;
        }

        public async Task<bool> AddGenreAsync(GenreManagementAddViewModel model)
        {
            Genre? existingGenre = await this.genreRepository
                .FirstOrDefaultAsync(g => g.Name.ToUpper() == model.Name.ToUpper());

            if (existingGenre != null)
            {
                return false;
            }

            Genre newGenre = new Genre()
            {
                PublicId = Guid.NewGuid(),
                Name = model.Name
            };

            await this.genreRepository.AddAsync(newGenre);

            return true;
        }
    }
}
