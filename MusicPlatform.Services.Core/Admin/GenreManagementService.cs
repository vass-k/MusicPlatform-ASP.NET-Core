namespace MusicPlatform.Services.Core.Admin
{
    using Microsoft.EntityFrameworkCore;

    using MusicPlatform.Data.Models;
    using MusicPlatform.Data.Repository.Interfaces;
    using MusicPlatform.Services.Core.Admin.Interfaces;
    using MusicPlatform.Web.ViewModels.Admin.GenreManagement;

    public class GenreManagementService : IGenreManagementService
    {
        private readonly IGenreRepository genreRepository;

        public GenreManagementService(IGenreRepository genreRepository)
        {
            this.genreRepository = genreRepository;
        }

        public async Task<IEnumerable<GenreManagementIndexViewModel>> GetAllGenresForManagementAsync()
        {
            IEnumerable<GenreManagementIndexViewModel> allGenres = await this.genreRepository
                .GetAllAsQueryable()
                .AsNoTracking()
                .IgnoreQueryFilters()
                .OrderBy(g => g.Name)
                .Select(g => new GenreManagementIndexViewModel
                {
                    PublicId = g.PublicId,
                    Name = g.Name,
                    TrackCount = g.Tracks.Count(),
                    IsDeleted = g.IsDeleted
                })
                .ToListAsync();

            return allGenres;
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

        public async Task<GenreEditViewModel?> GetGenreForEditAsync(Guid publicId)
        {
            Genre? genreToEdit = await this.genreRepository
                .FirstOrDefaultAsync(g => g.PublicId == publicId);

            if (genreToEdit == null)
            {
                return null;
            }

            return new GenreEditViewModel
            {
                PublicId = genreToEdit.PublicId,
                Name = genreToEdit.Name
            };
        }

        public async Task<bool> EditGenreAsync(GenreEditViewModel model)
        {
            Genre? genreToEdit = await this.genreRepository
                .FirstOrDefaultAsync(g => g.PublicId == model.PublicId);

            if (genreToEdit == null)
            {
                return false;
            }

            bool nameExists = await this.genreRepository
                .GetAllAsQueryable()
                .AnyAsync(g => g.Name.ToUpper() == model.Name.ToUpper() && g.PublicId != model.PublicId);

            if (nameExists)
            {
                return false;
            }

            genreToEdit.Name = model.Name;

            return await this.genreRepository.UpdateAsync(genreToEdit);
        }

        public async Task<Tuple<bool, bool>> DeleteOrRestoreGenreAsync(Guid publicId)
        {
            bool result = false;
            bool isRestored = false;

            Genre? genre = await this.genreRepository
                .GetAllAsQueryable()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(g => g.PublicId == publicId);

            if (genre != null)
            {
                if (genre.IsDeleted)
                {
                    isRestored = true;
                }

                genre.IsDeleted = !genre.IsDeleted;
                genre.DeletedOn = genre.IsDeleted ? DateTime.UtcNow : null;

                result = await this.genreRepository.UpdateAsync(genre);
            }

            return new Tuple<bool, bool>(result, isRestored);
        }
    }
}
