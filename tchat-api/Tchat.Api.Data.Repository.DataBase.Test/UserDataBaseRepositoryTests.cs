using FluentAssertions;
using Moq;
using Tchat.Api.Data.Repository.Database.Test.Utils;
using Tchat.Api.Domain;
using Tchat.Api.Exceptions.User;

namespace Tchat.Api.Data.Repository.DataBase.Test
{
    [TestClass]
    public class UserDataBaseRepositoryTests
    {
        public static readonly List<User> Users = new List<User>()
        {
            new() { Id = "2F8AB7D3-0E7F-4979-AB5D-2B9C7C0FBF4B", Email = "2F8AB7D3-0E7F-4979-AB5D-2B9C7C0FBF4B", UserName = "2F8AB7D3-0E7F-4979-AB5D-2B9C7C0FBF4B", PasswordHash = "pwd1" }
        };

        [DataTestMethod]
        [DataRow("2F8AB7D3-0E7F-4979-AB5D-2B9C7C0FBF4B", true)]
        [DataRow("2F8AB7D3-0E7F-4979-AB5D-2B9C7C0FBF48", false)]
        public async Task GetUserByIdTests(string guid, bool hasToBeFound)
        {
            // Arrange
            var mockDbContext = DataCreator.GetMockDbContext();
            var mockUserManager = MockedUserManager.GetUserManagerMock<User>(mockDbContext.Object);
            mockUserManager.Setup(mum => mum.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(Users.FirstOrDefault(u => u.Id.ToString() == guid));

            var messageRepository = new UserDataBaseRepository(mockDbContext.Object, mockUserManager.Object);

            // Act
            var user = await messageRepository.GetUserById(Guid.Parse(guid));

            // Assert
            if (hasToBeFound)
            {
                Assert.IsNotNull(user);
            }
            else
            {
                Assert.IsNull(user);
            }
        }

        [DataTestMethod]
        [DataRow("login1", "pwd1", false)]
        [DataRow("2F8AB7D3-0E7F-4979-AB5D-2B9C7C0FBF4B", "pwd2", false)]
        [DataRow("2F8AB7D3-0E7F-4979-AB5D-2B9C7C0FBF4B", "pwd1", true)]
        public async Task GetUserByCredentialsWhenNotUserWithGivenCredentialsFoundTests(string login, string password, bool shouldBeFound)
        {
            // Arrange
            var mockDbContext = DataCreator.GetMockDbContext();
            var mockUserManager = MockedUserManager.GetUserManagerMock<User>(mockDbContext.Object);
            mockUserManager.Setup(mum => mum.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(Users.FirstOrDefault(u => (u.UserName == login || u.Email == login) && u.PasswordHash == password));
            mockUserManager.Setup(mum => mum.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(shouldBeFound);
            mockUserManager.Setup(mum => mum.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(Users.FirstOrDefault(u => (u.UserName == login || u.Email == login) && u.PasswordHash == password));

            var messageRepository = new UserDataBaseRepository(mockDbContext.Object, mockUserManager.Object);

            // Act
            User result = null;
            Func<Task> act = async () => result = await messageRepository.FindUserByCredentials(login, password);

            // Assert
            if(shouldBeFound)
            {
                await act.Should()
                    .NotThrowAsync<UserNotFoundException>();
                result.Should()
                    .NotBeNull()
                    .And
                    .Match<User>(u => u.UserName == login || u.Email == login);
            }
            else
            {
                await act.Should()
                    .ThrowAsync<UserNotFoundException>();
            }
        }
    }
}
