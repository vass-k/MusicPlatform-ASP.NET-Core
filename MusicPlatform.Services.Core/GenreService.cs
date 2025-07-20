namespace MusicPlatform.Services.Core
{
    using Microsoft.EntityFrameworkCore;

    using MusicPlatform.Data.Repository.Interfaces;
    using MusicPlatform.Services.Core.Interfaces;
    using MusicPlatform.Web.ViewModels;
    using MusicPlatform.Web.ViewModels.Genre;
    using MusicPlatform.Web.ViewModels.Track;

    using static MusicPlatform.GCommon.ApplicationConstants;

    public class GenreService : IGenreService
    {
        private readonly IGenreRepository genreRepository;
        private readonly ITrackRepository trackRepository;

        public GenreService(IGenreRepository genreRepository, ITrackRepository trackRepository)
        {
            this.genreRepository = genreRepository;
            this.trackRepository = trackRepository;
        }

        public async Task<IEnumerable<GenreIndexViewModel>> GetAllGenresWithTrackCountAsync()
        {
            IEnumerable<GenreIndexViewModel> allGenres = await this.genreRepository
                .GetAllAsQueryable()
                .AsNoTracking()
                .OrderBy(g => g.Name)
                .Select(g => new GenreIndexViewModel
                {
                    PublicId = g.PublicId,
                    Name = g.Name,
                    TrackCount = g.Tracks.Count()
                })
                .ToListAsync();

            return allGenres;
        }

        public async Task<GenreDetailsViewModel?> GetGenreDetailsAsync(Guid publicId, int pageNumber, int pageSize)
        {
            GenreDetailsViewModel? genreDetails = null;

            if (publicId != Guid.Empty)
            {
                var genre = await this.genreRepository
                    .GetAllAsQueryable()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(g => g.PublicId == publicId);

                if (genre != null)
                {
                    var tracksQuery = this.trackRepository
                                          .GetAllAsQueryable()
                                          .AsNoTracking()
                                          .Where(t => t.GenreId == genre.Id);

                    var totalCount = await tracksQuery.CountAsync();
                    var pagedTracks = await tracksQuery
                        .OrderByDescending(t => t.CreatedOn)
                        .ThenBy(t => t.Id)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .Select(t => new TrackIndexViewModel
                        {
                            PublicId = t.PublicId,
                            Title = t.Title,
                            ArtistName = t.ArtistName,
                            ImageUrl = t.ImageUrl ?? DefaultTrackImageUrl,
                            Plays = t.Plays,
                            FavoritesCount = t.UserFavorites.Count()
                        })
                        .ToListAsync();

                    var pagedResult = new PagedResult<TrackIndexViewModel>
                    {
                        Items = pagedTracks,
                        PageNumber = pageNumber,
                        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
                    };

                    genreDetails = new GenreDetailsViewModel
                    {
                        GenreName = genre.Name,
                        PagedTracks = pagedResult
                    };
                }
            }

            return genreDetails;
        }
    }
}
