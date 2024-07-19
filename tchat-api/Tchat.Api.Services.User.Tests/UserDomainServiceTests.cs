using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tchat.Api.Data.Repository;
using Tchat.Api.Exceptions.Utils;
using Tchat.Api.Mappers;
using Tchat.Api.Models;
using Tchat.Api.Services.Args;

namespace Tchat.Api.Services.User.Tests
{
    [TestClass]
    public class UserDomainServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private UserDomainService _userDomainService;

        [TestInitialize]
        public void Initialize()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _userDomainService = new UserDomainService(_userRepositoryMock.Object, _mapperMock.Object);
        }

        [TestMethod]
        public async Task Create_ValidUserDto_ReturnsCreatedUserDto()
        {
            // Arrange
            var userDto = new UserDto
            {
                Email = "test@example.com",
                UserName = "testuser"
            };
            var user = new Domain.User
            {
                Email = "test@example.com",
                UserName = "testuser"
            };

            _userRepositoryMock.Setup(repo => repo.GetUserByLogin(userDto.Email)).ReturnsAsync((Domain.User)null);
            _userRepositoryMock.Setup(repo => repo.GetUserByLogin(userDto.UserName)).ReturnsAsync((Domain.User)null);
            _userRepositoryMock.Setup(repo => repo.AddUser(It.IsAny<Domain.User>())).ReturnsAsync(user);
            _mapperMock.Setup(mapper => mapper.Map<UserDto>(user)).Returns(userDto);

            // Act
            var result = await _userDomainService.Create(userDto);

            // Assert
            Assert.AreEqual(userDto, result);
            _userRepositoryMock.Verify(repo => repo.GetUserByLogin(userDto.Email), Times.Once);
            _userRepositoryMock.Verify(repo => repo.GetUserByLogin(userDto.UserName), Times.Once);
            _userRepositoryMock.Verify(repo => repo.AddUser(It.IsAny<Domain.User>()), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<UserDto>(user), Times.Once);
        }

        [TestMethod]
        public async Task Create_UserWithEmailAlreadyExists_ThrowsRessourceAlreadyExistsException()
        {
            // Arrange
            var userDto = new UserDto
            {
                Email = "test@example.com",
                UserName = "testuser"
            };
            var existingUser = new Domain.User
            {
                Email = "test@example.com",
                UserName = "existinguser"
            };

            _userRepositoryMock.Setup(repo => repo.GetUserByLogin(userDto.Email)).ReturnsAsync(existingUser);

            // Act and Assert
            await Assert.ThrowsExceptionAsync<RessourceAlreadyExists>(() => _userDomainService.Create(userDto));
            _userRepositoryMock.Verify(repo => repo.GetUserByLogin(userDto.Email), Times.Once);
            _userRepositoryMock.Verify(repo => repo.GetUserByLogin(userDto.UserName), Times.Never);
            _userRepositoryMock.Verify(repo => repo.AddUser(It.IsAny<Domain.User>()), Times.Never);
            _mapperMock.Verify(mapper => mapper.Map<UserDto>(It.IsAny<Domain.User>()), Times.Never);
        }

        [TestMethod]
        public async Task Create_UserWithUserNameAlreadyExists_ThrowsRessourceAlreadyExistsException()
        {
            // Arrange
            var userDto = new UserDto
            {
                Email = "test@example.com",
                UserName = "testuser"
            };
            var existingUser = new Domain.User
            {
                Email = "existing@example.com",
                UserName = "testuser"
            };

            _userRepositoryMock.Setup(repo => repo.GetUserByLogin(userDto.Email)).ReturnsAsync((Domain.User)null);
            _userRepositoryMock.Setup(repo => repo.GetUserByLogin(userDto.UserName)).ReturnsAsync(existingUser);

            // Act and Assert
            await Assert.ThrowsExceptionAsync<RessourceAlreadyExists>(() => _userDomainService.Create(userDto));
            _userRepositoryMock.Verify(repo => repo.GetUserByLogin(userDto.Email), Times.Once);
            _userRepositoryMock.Verify(repo => repo.GetUserByLogin(userDto.UserName), Times.Once);
            _userRepositoryMock.Verify(repo => repo.AddUser(It.IsAny<Domain.User>()), Times.Never);
            _mapperMock.Verify(mapper => mapper.Map<UserDto>(It.IsAny<Domain.User>()), Times.Never);
        }

        [TestMethod]
        public async Task Delete_ExistingUserId_DeletesUser()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _userRepositoryMock.Setup(repo => repo.GetUserById(userId)).ReturnsAsync(new Domain.User());

            // Act
            await _userDomainService.Delete(userId);

            // Assert
            _userRepositoryMock.Verify(repo => repo.GetUserById(userId), Times.Once);
            _userRepositoryMock.Verify(repo => repo.DeleteUser(userId), Times.Once);
        }

        [TestMethod]
        public async Task Delete_NonExistingUserId_ThrowsRessourceNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _userRepositoryMock.Setup(repo => repo.GetUserById(userId)).ReturnsAsync((Domain.User)null);

            // Act and Assert
            await Assert.ThrowsExceptionAsync<RessourceNotFound>(() => _userDomainService.Delete(userId));
            _userRepositoryMock.Verify(repo => repo.GetUserById(userId), Times.Once);
            _userRepositoryMock.Verify(repo => repo.DeleteUser(userId), Times.Never);
        }

        [TestMethod]
        public async Task GetAll_ReturnsAllUsers()
        {
            // Arrange
            var users = new List<Domain.User>
            {
                new Domain.User { Id = Guid.NewGuid().ToString(), Email = "user1@example.com", UserName = "user1" },
                new Domain.User { Id = Guid.NewGuid().ToString(), Email = "user2@example.com", UserName = "user2" },
                new Domain.User { Id = Guid.NewGuid().ToString(), Email = "user3@example.com", UserName = "user3" }
            };
            var expectedUserDtos = users.Select(user => new UserDto { Id = user.Id.ToString(), Email = user.Email, UserName = user.UserName });

            _userRepositoryMock.Setup(repo => repo.GetAllUsers(It.IsAny<int?>())).ReturnsAsync(users);
            _mapperMock.Setup(mapper => mapper.Map<UserDto>(It.IsAny<Domain.User>())).Returns((Domain.User user) => new UserDto { Id = user.Id.ToString(), Email = user.Email, UserName = user.UserName });

            // Act
            var result = await _userDomainService.GetAll();

            // Assert
            Assert.AreEqual(expectedUserDtos.ToList().Count, result.ToList().Count);
            _userRepositoryMock.Verify(repo => repo.GetAllUsers(It.IsAny<int?>()), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<UserDto>(It.IsAny<Domain.User>()), Times.Exactly(users.Count));
        }

        [TestMethod]
        public async Task GetById_ExistingUserId_ReturnsUserDto()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new Domain.User { Id = userId.ToString(), Email = "test@example.com", UserName = "testuser" };
            var expectedUserDto = new UserDto { Id = userId.ToString(), Email = user.Email, UserName = user.UserName };

            _userRepositoryMock.Setup(repo => repo.GetUserById(userId)).ReturnsAsync(user);
            _mapperMock.Setup(mapper => mapper.Map<UserDto>(user)).Returns(expectedUserDto);

            // Act
            var result = await _userDomainService.GetById(userId);

            // Assert
            Assert.AreEqual(expectedUserDto, result);
            _userRepositoryMock.Verify(repo => repo.GetUserById(userId), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<UserDto>(user), Times.Once);
        }

        [TestMethod]
        public async Task GetById_NonExistingUserId_ThrowsRessourceNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _userRepositoryMock.Setup(repo => repo.GetUserById(userId)).ReturnsAsync((Domain.User)null);

            // Act and Assert
            await Assert.ThrowsExceptionAsync<RessourceNotFound>(() => _userDomainService.GetById(userId));
            _userRepositoryMock.Verify(repo => repo.GetUserById(userId), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<UserDto>(It.IsAny<Domain.User>()), Times.Never);
        }

        [TestMethod]
        public async Task Search_Should_ReturnMatchingUsers_When_SearchArgsProvided()
        {
            // Arrange
            var _userRepositoryMock = new Mock<IUserRepository>();
            var _mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<UserMapper>();
            }).CreateMapper();
            var searchArgs = new UserSearchArgs
            {
                Name = "Doe",
                Firstname = "John",
                Email = "john.doe@example.com"
            };

            var users = new List<Domain.User>
            {
                new Domain.User { Id = Guid.NewGuid().ToString(), LastName = "Doe", FirstName = "John", Email = "john.doe@example.com" },
                new Domain.User { Id = Guid.NewGuid().ToString(), LastName = "Smith", FirstName = "Jane", Email = "jane.smith@example.com" },
                new Domain.User { Id = Guid.NewGuid().ToString(), LastName = "Doe", FirstName = "Alice", Email = "alice.doe@example.com" }
            };

            _userRepositoryMock.Setup(repo => repo.GetAllUsers(null)).ReturnsAsync(users);

            var userDomainService = new UserDomainService(_userRepositoryMock.Object, _mapper);

            // Act
            var result = await userDomainService.Search(searchArgs);

            // Assert
            result.Should().HaveCount(1);
        }

        [TestMethod]
        public async Task Search_Should_ReturnEmptyList_When_NoMatchingUsersFound()
        {
            // Arrange
            var _userRepositoryMock = new Mock<IUserRepository>();
            var _mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<UserMapper>();
            }).CreateMapper();
            var searchArgs = new UserSearchArgs
            {
                Id = Guid.NewGuid(),
                Name = "Smith",
                Firstname = "Jane",
                Email = "jane.smith@example.com"
            };

            var users = new List<Domain.User>
            {
                new Domain.User { Id = Guid.NewGuid().ToString(), LastName = "Doe", FirstName = "John", Email = "john.doe@example.com" },
                new Domain.User { Id = Guid.NewGuid().ToString(), LastName = "Doe", FirstName = "Alice", Email = "alice.doe@example.com" }
            };

            _userRepositoryMock.Setup(repo => repo.GetAllUsers(null)).ReturnsAsync(users);

            var userDomainService = new UserDomainService(_userRepositoryMock.Object, _mapper);

            // Act
            var result = await userDomainService.Search(searchArgs);

            // Assert
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public async Task Update_ExistingUserDto_ReturnsUpdatedUserDto()
        {
            // Arrange
            var userDto = new UserDto
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@example.com",
                UserName = "testuser"
            };
            var user = new Domain.User
            {
                Id = userDto.Id,
                Email = "test@example.com",
                UserName = "testuser"
            };

            _userRepositoryMock.Setup(repo => repo.GetUserById(Guid.Parse(userDto.Id))).ReturnsAsync(user);
            _userRepositoryMock.Setup(repo => repo.UpdateUser(It.IsAny<Domain.User>())).ReturnsAsync(user);
            _mapperMock.Setup(mapper => mapper.Map<UserDto>(user)).Returns(userDto);

            // Act
            var result = await _userDomainService.Update(userDto);

            // Assert
            Assert.AreEqual(userDto, result);
            _userRepositoryMock.Verify(repo => repo.GetUserById(Guid.Parse(userDto.Id)), Times.Once);
            _userRepositoryMock.Verify(repo => repo.UpdateUser(It.IsAny<Domain.User>()), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<UserDto>(user), Times.Once);
        }

        [TestMethod]
        public async Task Update_NonExistingUserDto_ThrowsRessourceNotFoundException()
        {
            // Arrange
            var userDto = new UserDto
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@example.com",
                UserName = "testuser"
            };

            _userRepositoryMock.Setup(repo => repo.GetUserById(Guid.Parse(userDto.Id))).ReturnsAsync((Domain.User)null);

            // Act and Assert
            await Assert.ThrowsExceptionAsync<RessourceNotFound>(() => _userDomainService.Update(userDto));
            _userRepositoryMock.Verify(repo => repo.GetUserById(Guid.Parse(userDto.Id)), Times.Once);
            _userRepositoryMock.Verify(repo => repo.UpdateUser(It.IsAny<Domain.User>()), Times.Never);
            _mapperMock.Verify(mapper => mapper.Map<UserDto>(It.IsAny<Domain.User>()), Times.Never);
        }
    }
}
