namespace MusicPlatform.Services.Tests
{
    using MockQueryable;
    using Moq;

    using MusicPlatform.Data.Models;
    using MusicPlatform.Data.Repository.Interfaces;
    using MusicPlatform.Services.Core.Admin;
    using MusicPlatform.Services.Core.Admin.Interfaces;
    using MusicPlatform.Web.ViewModels.Admin.GenreManagement;

    using System.Linq.Expressions;

    [TestFixture]
    public class GenreManagementServiceTests
    {
        private Mock<IGenreRepository> genreRepositoryMock;

        private IGenreManagementService genreManagementService;

        private List<Genre> mockGenres;

        [SetUp]
        public void Setup()
        {
            this.genreRepositoryMock = new Mock<IGenreRepository>(MockBehavior.Strict);
            this.genreManagementService = new GenreManagementService(this.genreRepositoryMock.Object);

            mockGenres = new List<Genre>
            {
                new Genre
                {
                    Id = 1,
                    PublicId = Guid.NewGuid(),
                    Name = "Existing Genre",
                    IsDeleted = false,
                    Tracks = new List<Track>()
                },
                new Genre
                {
                    Id = 2,
                    PublicId = Guid.NewGuid(),
                    Name = "Deleted Genre",
                    IsDeleted = true,
                    Tracks = new List<Track>()
                }
            };
        }

        [Test]
        public async Task GetAllGenresForManagement_Should_ReturnAllGenres_IncludingDeleted()
        {
            var genreQueryable = mockGenres.BuildMock();
            this.genreRepositoryMock
                .Setup(r => r.GetAllAsQueryable())
                .Returns(genreQueryable);

            var result = await this.genreManagementService
                .GetAllGenresForManagementAsync();

            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.IsTrue(result.Any(g => g.Name == "Deleted Genre" && g.IsDeleted));
        }

        [Test]
        public async Task AddGenre_Should_ReturnFalse_When_NameExists()
        {
            var model = new GenreManagementAddViewModel
            {
                Name = "Existing Genre"
            };

            this.genreRepositoryMock
                .Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Genre, bool>>>()))
                .ReturnsAsync(mockGenres[0]);

            var result = await this.genreManagementService
                .AddGenreAsync(model);

            Assert.IsFalse(result);

            this.genreRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Genre>()), Times.Never);
        }

        [Test]
        public async Task AddGenre_Should_ReturnTrue_ForNewGenre()
        {
            var model = new GenreManagementAddViewModel
            {
                Name = "New Genre"
            };

            this.genreRepositoryMock
                .Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Genre, bool>>>()))
                .ReturnsAsync((Genre?)null);
            this.genreRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Genre>()))
                .Returns(Task.CompletedTask);

            var result = await this.genreManagementService
                .AddGenreAsync(model);

            Assert.IsTrue(result);

            this.genreRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Genre>()), Times.Once);
        }

        [Test]
        public async Task EditGenre_Should_ReturnFalse_When_NewNameIsDuplicate()
        {
            var genreToEdit = new Genre
            {
                PublicId = Guid.NewGuid(),
                Name = "Original Name"
            };

            var model = new GenreEditViewModel
            {
                PublicId = genreToEdit.PublicId,
                Name = "Existing Genre"
            };

            this.genreRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Genre, bool>>>())).ReturnsAsync(genreToEdit);
            this.genreRepositoryMock.Setup(r => r.GetAllAsQueryable()).Returns(mockGenres.BuildMock());

            var result = await this.genreManagementService
                .EditGenreAsync(model);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteOrRestore_Should_DeleteAnActiveGenre()
        {
            var activeGenre = mockGenres.First(g => !g.IsDeleted);
            var genreQueryable = mockGenres.BuildMock();

            this.genreRepositoryMock.Setup(r => r.GetAllAsQueryable()).Returns(genreQueryable);
            this.genreRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Genre>())).ReturnsAsync(true);

            var result = await this.genreManagementService
                .DeleteOrRestoreGenreAsync(activeGenre.PublicId);

            Assert.IsTrue(result.Item1);
            Assert.IsFalse(result.Item2);

            this.genreRepositoryMock.Verify(r => r.UpdateAsync(It.Is<Genre>(g => g.IsDeleted == true)), Times.Once);
        }

        [Test]
        public async Task DeleteOrRestore_Should_RestoreADeletedGenre()
        {
            var deletedGenre = mockGenres.First(g => g.IsDeleted);
            var genreQueryable = mockGenres.BuildMock();

            this.genreRepositoryMock.Setup(r => r.GetAllAsQueryable()).Returns(genreQueryable);
            this.genreRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Genre>())).ReturnsAsync(true);

            var result = await this.genreManagementService
                .DeleteOrRestoreGenreAsync(deletedGenre.PublicId);

            Assert.IsTrue(result.Item1);
            Assert.IsTrue(result.Item2);

            this.genreRepositoryMock.Verify(r => r.UpdateAsync(It.Is<Genre>(g => g.IsDeleted == false)), Times.Once);
        }

        [Test]
        public async Task DeleteOrRestore_Should_ReturnFalse_When_GenreNotFound()
        {
            var nonExistentId = Guid.NewGuid();
            var genreQueryable = mockGenres.BuildMock();

            this.genreRepositoryMock.Setup(r => r.GetAllAsQueryable()).Returns(genreQueryable);

            var result = await this.genreManagementService
                .DeleteOrRestoreGenreAsync(nonExistentId);

            Assert.IsFalse(result.Item1);
        }
    }
}
