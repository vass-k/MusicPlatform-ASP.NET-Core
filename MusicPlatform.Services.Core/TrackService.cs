namespace MusicPlatform.Services.Core
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    using MusicPlatform.Data.Models;
    using MusicPlatform.Data.Repository.Interfaces;
    using MusicPlatform.Services.Core.Interfaces;
    using MusicPlatform.Web.ViewModels.Comment;
    using MusicPlatform.Web.ViewModels.Track;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    using static MusicPlatform.GCommon.ApplicationConstants;

    public class TrackService : ITrackService
    {
        private readonly ITrackRepository trackRepository;
        private readonly IGenreRepository genreRepository;
        private readonly ICloudinaryService cloudinaryService;

        public TrackService(
            ITrackRepository trackRepository,
            IGenreRepository genreRepository,
            ICloudinaryService cloudinaryService)
        {
            this.trackRepository = trackRepository;
            this.genreRepository = genreRepository;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<IEnumerable<TrackIndexViewModel>> GetAllTracksForIndexAsync()
        {
            List<TrackIndexViewModel> trackViewModels = await this.trackRepository
                .GetAllAsQueryable()
                .AsNoTracking()
                .Include(t => t.UserFavorites)
                .Select(t => new TrackIndexViewModel
                {
                    PublicId = t.PublicId,
                    Title = t.Title,
                    ArtistName = t.ArtistName,
                    ImageUrl = t.ImageUrl,
                    Plays = t.Plays,
                    FavoritesCount = t.UserFavorites.Count
                })
                .ToListAsync();

            foreach (var track in trackViewModels)
            {
                if (string.IsNullOrEmpty(track.ImageUrl))
                {
                    track.ImageUrl = DefaultTrackImageUrl;
                }
            }

            return trackViewModels;
        }

        public async Task<TrackAddViewModel> GetTrackAddViewModelAsync()
        {
            var genres = await this.genreRepository.GetAllAsync();

            return new TrackAddViewModel
            {
                Genres = genres.Select(g => new SelectListItem(g.Name, g.Id.ToString()))
            };
        }

        public async Task AddTrackAsync(TrackAddViewModel model, string uploaderId)
        {
            ValidateFile(model.AudioFile, FileValidationConstants.MaxAudioFileSize, FileValidationConstants.AllowedAudioExtensions);
            if (model.ImageFile != null)
            {
                ValidateFile(model.ImageFile, FileValidationConstants.MaxImageFileSize, FileValidationConstants.AllowedImageExtensions);
            }

            string audioUrl = await this.cloudinaryService.UploadAudioAsync(model.AudioFile);
            string? imageUrl = await this.cloudinaryService.UploadImageAsync(model.ImageFile);

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

        public async Task<TrackDetailsViewModel?> GetTrackDetailsAsync(Guid publicId)
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
                    ImageUrl = t.ImageUrl,
                    AudioUrl = t.AudioUrl,
                    Plays = t.Plays,
                    DurationInSeconds = t.DurationInSeconds,
                    FavoritesCount = t.UserFavorites.Count(),

                    ReleasedDate = t.CreatedOn.ToString("MMMM dd, yyyy"),

                    Comments = t.Comments
                                .OrderByDescending(c => c.CreatedOn)
                                .Select(c => new CommentViewModel()
                                {
                                    AuthorUsername = c.User.UserName!,
                                    Content = c.Content,
                                    PostedOn = c.CreatedOn.ToString("MMMM dd, yyyy")
                                })
                })
                .FirstOrDefaultAsync();

            if (trackDetails != null && string.IsNullOrEmpty(trackDetails.ImageUrl))
            {
                trackDetails.ImageUrl = DefaultTrackImageUrl;
            }

            return trackDetails;
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

