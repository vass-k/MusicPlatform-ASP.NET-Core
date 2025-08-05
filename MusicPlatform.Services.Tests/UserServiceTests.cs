namespace MusicPlatform.Services.Tests
{
    using Microsoft.AspNetCore.Identity;

    using MockQueryable;
    using Moq;

    using MusicPlatform.Data.Models;
    using MusicPlatform.Services.Core.Admin;
    using MusicPlatform.Services.Core.Admin.Interfaces;

    using static MusicPlatform.GCommon.ApplicationConstants;

    [TestFixture]
    public class UserServiceTests
    {
        private Mock<UserManager<AppUser>> userManagerMock;

        private IUserService userService;

        private AppUser adminUser;
        private AppUser regularUser;
        private List<AppUser> mockUsers;

        [SetUp]
        public void Setup()
        {
            var store = new Mock<IUserStore<AppUser>>();
            this.userManagerMock = new Mock<UserManager<AppUser>>(store.Object, null, null, null, null, null, null, null, null);

            this.userService = new UserService(this.userManagerMock.Object);

            adminUser = new AppUser
            {
                Id = "admin-id",
                Email = "admin@music.com"
            };
            regularUser = new AppUser
            {
                Id = "user-id",
                Email = "user@music.com"
            };
            mockUsers = new List<AppUser>
            {
                adminUser,
                regularUser
            };
        }

        [Test]
        public async Task GetUserManagementBoardData_Should_ExcludeCurrentAdmin()
        {
            var usersQueryable = mockUsers.BuildMock();

            this.userManagerMock.Setup(um => um.Users).Returns(usersQueryable);
            this.userManagerMock.Setup(um => um.GetRolesAsync(regularUser)).ReturnsAsync(new List<string> { "User" });

            var result = await this.userService
                .GetUserManagementBoardDataAsync(adminUser.Id);

            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Id, Is.EqualTo(regularUser.Id));
        }

        [Test]
        public async Task GetUserManagementBoardData_Should_CorrectlyMapUserData()
        {
            var usersQueryable = mockUsers.BuildMock();

            this.userManagerMock.Setup(um => um.Users).Returns(usersQueryable);
            this.userManagerMock.Setup(um => um.GetRolesAsync(regularUser)).ReturnsAsync(new List<string> { "User", "Subscriber" });

            var result = await this.userService
                .GetUserManagementBoardDataAsync(adminUser.Id);

            var resultUser = result.First();

            Assert.That(resultUser.Id, Is.EqualTo(regularUser.Id));
            Assert.That(resultUser.Email, Is.EqualTo(regularUser.Email));
            Assert.That(resultUser.Roles.Count(), Is.EqualTo(2));

            Assert.Contains("Subscriber", resultUser.Roles.ToList());
        }

        [Test]
        public void MakeUserAdmin_Should_ThrowArgumentException_When_UserNotFound()
        {
            var nonExistentId = "non-existent-id";

            this.userManagerMock.Setup(um => um.FindByIdAsync(nonExistentId)).ReturnsAsync((AppUser)null);

            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await this.userService
                .MakeUserAdminAsync(nonExistentId);
            }, "User does not exist!");
        }

        [Test]
        public async Task MakeUserAdmin_Should_RemoveUserRole_And_AddAdminRole()
        {
            this.userManagerMock.Setup(um => um.FindByIdAsync(regularUser.Id)).ReturnsAsync(regularUser);
            this.userManagerMock.Setup(um => um.IsInRoleAsync(regularUser, UserRoleName)).ReturnsAsync(true);
            this.userManagerMock.Setup(um => um.RemoveFromRoleAsync(regularUser, UserRoleName)).ReturnsAsync(IdentityResult.Success);
            this.userManagerMock.Setup(um => um.AddToRoleAsync(regularUser, AdminRoleName)).ReturnsAsync(IdentityResult.Success);

            var result = await this.userService
                .MakeUserAdminAsync(regularUser.Id);

            Assert.IsTrue(result);

            this.userManagerMock.Verify(um => um.RemoveFromRoleAsync(regularUser, UserRoleName), Times.Once);
            this.userManagerMock.Verify(um => um.AddToRoleAsync(regularUser, AdminRoleName), Times.Once);
        }

        [Test]
        public async Task MakeUserAdmin_Should_OnlyAddAdminRole_When_NotInUserRole()
        {
            this.userManagerMock.Setup(um => um.FindByIdAsync(regularUser.Id)).ReturnsAsync(regularUser);
            this.userManagerMock.Setup(um => um.IsInRoleAsync(regularUser, UserRoleName)).ReturnsAsync(false);
            this.userManagerMock.Setup(um => um.AddToRoleAsync(regularUser, AdminRoleName)).ReturnsAsync(IdentityResult.Success);

            var result = await this.userService
                .MakeUserAdminAsync(regularUser.Id);

            Assert.IsTrue(result);

            this.userManagerMock.Verify(um => um.RemoveFromRoleAsync(It.IsAny<AppUser>(), It.IsAny<string>()), Times.Never);
            this.userManagerMock.Verify(um => um.AddToRoleAsync(regularUser, AdminRoleName), Times.Once);
        }

        [Test]
        public async Task MakeUserAdmin_Should_ReturnFalse_When_AddToRoleFails()
        {
            this.userManagerMock.Setup(um => um.FindByIdAsync(regularUser.Id)).ReturnsAsync(regularUser);
            this.userManagerMock.Setup(um => um.IsInRoleAsync(regularUser, UserRoleName)).ReturnsAsync(true);
            this.userManagerMock.Setup(um => um.RemoveFromRoleAsync(regularUser, UserRoleName)).ReturnsAsync(IdentityResult.Success);
            this.userManagerMock.Setup(um => um.AddToRoleAsync(regularUser, AdminRoleName)).ReturnsAsync(IdentityResult.Failed());

            var result = await this.userService
                .MakeUserAdminAsync(regularUser.Id);

            Assert.IsFalse(result);
        }
    }
}