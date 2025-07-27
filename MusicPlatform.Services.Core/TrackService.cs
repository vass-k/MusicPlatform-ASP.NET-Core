namespace MusicPlatform.Services.Core
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    using MusicPlatform.Data.Models;
    using MusicPlatform.Data.Repository.Interfaces;
    using MusicPlatform.Services.Common.Interfaces;
    using MusicPlatform.Services.Core.Interfaces;
    using MusicPlatform.Web.ViewModels;
    using MusicPlatform.Web.ViewModels.Comment;
    using MusicPlatform.Web.ViewModels.Track;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    using static MusicPlatform.GCommon.ApplicationConstants;

    public class TrackService : ITrackService
    {
        private readonly ITrackRepository trackRepository;
        private readonly IGenreRepository genreRepository;
        private readonly ICloudStorageService cloudStorageService;

        public TrackService(
            ITrackRepository trackRepository,
            IGenreRepository genreRepository,
            ICloudStorageService cloudStorageService)
        {
            this.trackRepository = trackRepository;
            this.genreRepository = genreRepository;
            this.cloudStorageService = cloudStorageService;
        }

        public async Task<PagedResult<TrackIndexViewModel>> GetAllTracksForIndexAsync(int pageNumber, int pageSize)
        {
            var totalCount = await this.trackRepository
                .GetAllAsQueryable()
                .AsNoTracking()
                .CountAsync();

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            List<TrackIndexViewModel> trackViewModels = await this.trackRepository
                .GetAllAsQueryable()
                .AsNoTracking()
                .OrderByDescending(t => t.CreatedOn)
                .ThenBy(t => t.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TrackIndexViewModel()
                {
                    PublicId = t.PublicId,
                    Title = t.Title,
                    ArtistName = t.ArtistName,
                    ImageUrl = t.ImageUrl ?? DefaultTrackImageUrl,
                    Plays = t.Plays,
                    FavoritesCount = t.UserFavorites.Count
                })
                .ToListAsync();

            PagedResult<TrackIndexViewModel> pagedResult = new PagedResult<TrackIndexViewModel>
            {
                Items = trackViewModels,
                PageNumber = pageNumber,
                TotalPages = totalPages
            };

            return pagedResult;
        }

        public async Task<TrackIndexPageViewModel> GetTracksForIndexPageAsync(string? searchString, int pageNumber, int pageSize)
        {
            var trackQuery = this.trackRepository
                .GetAllAsQueryable()
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchString))
            {
                trackQuery = trackQuery.Where(t => t.Title.Contains(searchString) ||
                                                   t.ArtistName.Contains(searchString));
            }

            int totalCount = await trackQuery.CountAsync();

            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            List<TrackIndexViewModel> trackViewModels = await trackQuery
                .OrderByDescending(t => t.CreatedOn)
                .ThenBy(t => t.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TrackIndexViewModel()
                {
                    PublicId = t.PublicId,
                    Title = t.Title,
                    ArtistName = t.ArtistName,
                    ImageUrl = t.ImageUrl ?? DefaultTrackImageUrl,
                    Plays = t.Plays,
                    FavoritesCount = t.UserFavorites.Count
                })
                .ToListAsync();

            TrackIndexPageViewModel pageViewModel = new TrackIndexPageViewModel
            {
                SearchString = searchString,
                PagedTracks = new PagedResult<TrackIndexViewModel>
                {
                    Items = trackViewModels,
                    PageNumber = pageNumber,
                    TotalPages = totalPages
                }
            };

            return pageViewModel;
        }

        public async Task<TrackAddViewModel> GetTrackAddViewModelAsync()
        {
            return new TrackAddViewModel
            {
                Genres = await this.GetGenresForSelectAsync()
            };
        }

        public async Task AddTrackAsync(TrackAddViewModel model, string uploaderId)
        {
            ValidateFile(model.AudioFile, FileValidationConstants.MaxAudioFileSize, FileValidationConstants.AllowedAudioExtensions);
            if (model.ImageFile != null)
            {
                ValidateFile(model.ImageFile, FileValidationConstants.MaxImageFileSize, FileValidationConstants.AllowedImageExtensions);
            }

            string audioUrl = await this.cloudStorageService.UploadAudioAsync(model.AudioFile);
            string? imageUrl = model.ImageFile != null
                ? await this.cloudStorageService.UploadImageAsync(model.ImageFile)
                : null;

            Track newTrack = new Track()
            {
                PublicId = Guid.NewGuid(),
                Title = model.Title,
                ArtistName = model.ArtistName,
                GenreId = model.GenreId,
                AudioUrl = audioUrl,
                ImageUrl = imageUrl,
                UploaderId = uploaderId,
                CreatedOn = DateTime.UtcNow
            };

            await this.trackRepository.AddAsync(newTrack);
        }

        public async Task<TrackDetailsViewModel?> GetTrackDetailsAsync(Guid publicId, string? currentUserId)
        {
            TrackDetailsViewModel? trackDetails = await this.trackRepository
                .GetAllAsQueryable()
                .AsNoTracking()
                .Where(t => t.PublicId == publicId)
                .Select(t => new TrackDetailsViewModel()
                {
                    PublicId = t.PublicId,
                    Title = t.Title,
                    ArtistName = t.ArtistName,
                    UploaderName = t.Uploader.UserName!,
                    ImageUrl = t.ImageUrl ?? DefaultTrackImageUrl,
                    AudioUrl = t.AudioUrl,
                    Plays = t.Plays,
                    DurationInSeconds = t.DurationInSeconds,
                    FavoritesCount = t.UserFavorites.Count(),
                    IsLikedByCurrentUser = t.UserFavorites
                                                .Any(f => f.UserId == currentUserId),

                    ReleasedDate = t.CreatedOn.ToString("MMMM dd, yyyy"),

                    Comments = t.Comments
                                .OrderByDescending(c => c.CreatedOn)
                                .Select(c => new CommentViewModel()
                                {
                                    Id = c.Id,
                                    AuthorUsername = c.User.UserName!,
                                    Content = c.Content,
                                    PostedOn = c.CreatedOn.ToString("MMMM dd, yyyy"),
                                    IsOwnedByCurrentUser = (c.UserId == currentUserId)
                                })
                })
                .FirstOrDefaultAsync();

            return trackDetails;
        }

        public async Task<TrackEditViewModel?> GetTrackForEditAsync(Guid publicId, string currentUserId)
        {
            var track = await this
                .FindTrackByPublicIdAsync(publicId);

            if (track == null || track.UploaderId != currentUserId)
            {
                return null;
            }

            return new TrackEditViewModel
            {
                PublicId = track.PublicId,
                Title = track.Title,
                ArtistName = track.ArtistName,
                GenreId = track.GenreId,
                Genres = await this.GetGenresForSelectAsync(track.GenreId)
            };
        }

        public async Task<bool> UpdateTrackAsync(TrackEditViewModel model, string currentUserId)
        {
            var track = await this
                .FindTrackByPublicIdAsync(model.PublicId);

            if (track == null || track.UploaderId != currentUserId)
            {
                return false;
            }

            track.Title = model.Title;
            track.ArtistName = model.ArtistName;
            track.GenreId = model.GenreId;

            return await this.trackRepository.UpdateAsync(track);
        }

        public async Task<TrackDeleteViewModel?> GetTrackForDeleteAsync(Guid publicId, string currentUserId)
        {
            var track = await this
                .FindTrackByPublicIdAsync(publicId);

            if (track == null || track.UploaderId != currentUserId)
            {
                return null;
            }

            return new TrackDeleteViewModel
            {
                PublicId = track.PublicId,
                Title = track.Title,
                ArtistName = track.ArtistName,
                ImageUrl = track.ImageUrl ?? DefaultTrackImageUrl
            };
        }

        public async Task<bool> DeleteTrackAsync(Guid publicId, string currentUserId)
        {
            var track = await this
                .FindTrackByPublicIdAsync(publicId);

            if (track == null || track.UploaderId != currentUserId)
            {
                return false;
            }

            // TODO - rename DeleteAsync to SoftDeleteAsync
            return await this.trackRepository.DeleteAsync(track);
        }

        public async Task<IEnumerable<SelectListItem>> GetGenresForSelectAsync(int? selectedGenreId = null)
        {
            var genres = await this.genreRepository.GetAllAsync();

            return genres.Select(g => new SelectListItem
            {
                Text = g.Name,
                Value = g.Id.ToString(),
                Selected = (selectedGenreId.HasValue && selectedGenreId.Value == g.Id)
            });
        }

        private async Task<Track?> FindTrackByPublicIdAsync(Guid publicId)
        {
            return await this.trackRepository
                .FirstOrDefaultAsync(t => t.PublicId == publicId);
        }

        private void ValidateFile(IFormFile file, int maxSizeInBytes, string[] allowedExtensions)
        {
            if (file.Length > maxSizeInBytes)
            {
                throw new InvalidOperationException($"File size exceeds the limit of {maxSizeInBytes / 1024 / 1024} MB.");
            }

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
            {
                throw new InvalidOperationException($"Invalid file type. Allowed types are: {string.Join(", ", allowedExtensions)}");
            }
        }
    }
}

