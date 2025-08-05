namespace MusicPlatform.Services.Tests
{
    using Moq;

    using MusicPlatform.Data.Models;
    using MusicPlatform.Data.Repository.Interfaces;
    using MusicPlatform.Services.Core;
    using MusicPlatform.Services.Core.Interfaces;
    using MusicPlatform.Web.ViewModels.Comment;

    using System.Linq.Expressions;

    [TestFixture]
    public class CommentServiceTests
    {
        private Mock<ICommentRepository> commentRepositoryMock;
        private Mock<ITrackRepository> trackRepositoryMock;

        private ICommentService commentService;

        private Track existingTrack;
        private AppUser user1;

        [SetUp]
        public void Setup()
        {
            this.commentRepositoryMock = new Mock<ICommentRepository>(MockBehavior.Strict);
            this.trackRepositoryMock = new Mock<ITrackRepository>(MockBehavior.Strict);

            this.commentService = new CommentService(
                this.commentRepositoryMock.Object,
                this.trackRepositoryMock.Object);

            existingTrack = new Track
            {
                Id = 1,
                PublicId = Guid.NewGuid()
            };

            user1 = new AppUser
            {
                Id = "user1-id"
            };
        }

        [Test]
        public void CreateComment_Should_ThrowInvalidOperationException_When_TrackNotFound()
        {
            var model = new AddCommentViewModel
            {
                TrackPublicId = Guid.NewGuid(),
                Content = "Test comment"
            };

            this.trackRepositoryMock
                .Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Track, bool>>>()))
                .ReturnsAsync((Track)null);

            Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await this.commentService
                .CreateCommentAsync(model, user1.Id);
            }, "Cannot comment on a track that does not exist.");
        }

        [Test]
        public async Task CreateComment_Should_CallRepositoryAdd_When_TrackExists()
        {
            var model = new AddCommentViewModel
            {
                TrackPublicId = existingTrack.PublicId,
                Content = "Test comment"
            };

            this.trackRepositoryMock
                .Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Track, bool>>>()))
                .ReturnsAsync(existingTrack);
            this.commentRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Comment>()))
                .Returns(Task.CompletedTask);

            await this.commentService
                .CreateCommentAsync(model, user1.Id);

            this.commentRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Comment>()), Times.Once);
        }

        [Test]
        public async Task CreateComment_Should_ReturnCorrectlyMappedViewModel()
        {
            var model = new AddCommentViewModel
            {
                TrackPublicId = existingTrack.PublicId,
                Content = "Great track!"
            };

            this.trackRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Track, bool>>>())).ReturnsAsync(existingTrack);
            this.commentRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Comment>())).Returns(Task.CompletedTask);

            var result = await this.commentService
                .CreateCommentAsync(model, user1.Id);

            Assert.IsNotNull(result);
            Assert.That(result.Content, Is.EqualTo("Great track!"));
            Assert.That(result.AuthorUsername, Is.EqualTo("You"));
            Assert.That(result.PostedOn, Is.EqualTo("Just now"));
        }

        [Test]
        public async Task DeleteComment_Should_ReturnFalse_When_CommentNotFound()
        {
            var nonExistentCommentId = 999;

            this.commentRepositoryMock
                .Setup(r => r.GetByIdAsync(nonExistentCommentId))
                .ReturnsAsync((Comment?)null);

            var result = await this.commentService
                .DeleteCommentAsync(nonExistentCommentId, user1.Id);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteComment_Should_ReturnFalse_When_UserIsNotOwner()
        {
            var commentId = 1;
            var nonOwnerId = "user2-id";
            var comment = new Comment
            {
                Id = commentId,
                UserId = user1.Id
            };

            this.commentRepositoryMock
                .Setup(r => r.GetByIdAsync(commentId))
                .ReturnsAsync(comment);

            var result = await this.commentService
                .DeleteCommentAsync(commentId, nonOwnerId);

            Assert.IsFalse(result);

            this.commentRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Comment>()), Times.Never);
        }

        [Test]
        public async Task DeleteComment_Should_ReturnTrueAndCallDelete_When_UserIsOwner()
        {
            var commentId = 1;
            var comment = new Comment
            {
                Id = commentId,
                UserId = user1.Id
            };

            this.commentRepositoryMock
                .Setup(r => r.GetByIdAsync(commentId))
                .ReturnsAsync(comment);
            this.commentRepositoryMock
                .Setup(r => r.DeleteAsync(comment))
                .ReturnsAsync(true);

            var result = await this.commentService
                .DeleteCommentAsync(commentId, user1.Id);

            Assert.IsTrue(result);

            this.commentRepositoryMock.Verify(r => r.DeleteAsync(comment), Times.Once);
        }
    }
}
