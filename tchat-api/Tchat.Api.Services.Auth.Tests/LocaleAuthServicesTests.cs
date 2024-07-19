using AutoMapper;
using Google.Apis.Auth;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tchat.Api.Data.Repository.Database.Test.Utils;
using Tchat.Api.Data.Repository;
using Tchat.Api.Models;
using Tchat.Api.Service.Auth;
using Tchat.Api.Services.Utils;
using FluentAssertions;
using Tchat.API.Persistence;
using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;
using FluentValidation;

namespace Tchat.Api.Services.Auth.Tests
{
    [TestClass]
    public class LocaleAuthServicesTests
    {
        private Mock<ApplicationDbContext> mockDbContext;
        private Mock<UserManager<Domain.User>> mockUserManager;
        private Mock<IAuthService> mockedService;
        private Mock<ITokenGenerator> fakeTokenGenerator;
        private Mock<IMapper> fakeMapper;
        private Mock<IUserRepository> fakeUserRepository;
        private IAuthService service;
        private Mock<IEmailSender> mockedEmailSender;
        private Mock<IValidator<UserDto>> userDtoValidator;

        [TestInitialize]
        public void Setup()
        {
            // Arrange
            mockDbContext = DataCreator.GetMockDbContext();
            mockUserManager = MockedUserManager.GetUserManagerMock<Tchat.Api.Domain.User>(mockDbContext.Object);
            mockedService = new Mock<IAuthService>();
            fakeTokenGenerator = new Mock<ITokenGenerator>();
            fakeMapper = new Mock<IMapper>();
            fakeMapper.Setup(fm => fm.Map<UserDto>(It.IsAny<Tchat.Api.Domain.User>())).Returns(new UserDto() { Email = "test.user@api.com", UserName = "Test User" });
            fakeMapper.Setup(fm => fm.Map<Domain.User>(It.IsAny<UserDto>())).Returns(new Domain.User() { Email = "test.user@api.com", UserName = "Test User" });
            fakeUserRepository = new Mock<IUserRepository>();
            fakeUserRepository.Setup(fur => fur.AddUser(It.IsAny<Tchat.Api.Domain.User>())).ReturnsAsync(new Tchat.Api.Domain.User() { Email = "test.user@api.com", UserName = "Test User" });
            fakeUserRepository.Setup(fur => fur.GetUserByLogin(It.IsAny<string>())).ReturnsAsync((Tchat.Api.Domain.User?)null);
            userDtoValidator = new Mock<IValidator<UserDto>>();
            userDtoValidator.Setup(udv => udv.ValidateAsync(It.IsAny<UserDto>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(new FluentValidation.Results.ValidationResult()));
            mockedEmailSender = new Mock<IEmailSender>();

            service = new LocaleAuthService(fakeUserRepository.Object, fakeMapper.Object, fakeTokenGenerator.Object, userDtoValidator.Object, mockedEmailSender.Object);
        }

        [TestMethod]
        public async Task WhenUserNotExistThenCreateUser()
        {
            // Act
            var user = await service.AuthUser(new UserDto() { Email = "test.user@api.com", UserName = "Test User", Token = "", Password = "" });

            // Assert
            fakeUserRepository.Verify(fakeUserRepository => fakeUserRepository.AddUser(It.IsAny<Tchat.Api.Domain.User>()), Times.Once);
        }

        [TestMethod]
        public async Task WhenAddUserIsCalledWithValidDtoThenCreateUser()
        {
            // Arrange
            fakeUserRepository.Setup(fur => fur.GetUserByLogin(It.IsAny<string>())).ReturnsAsync((Tchat.Api.Domain.User?)null);

            // Act
            UserDto result = null;
            Func<Task> act = async () => result = await service.AuthUser(new UserDto() { Email = "test.user@api.com", UserName = "Test User", Token = "", Password = "" });

            // Assert
            await act.Should().NotThrowAsync();
            result.Should().NotBeNull();
            mockedEmailSender.Verify(mes => mes.SendEmail(It.IsAny<MailArg>()), Times.Once);
        }

        [DataTestMethod]
        [DynamicData(nameof(WhenDtoHasInvalidDataThenThrowsArgumentExceptionsArguments), DynamicDataSourceType.Method)]
        public async Task WhenDtoHasInvalidDataThenThrowsArgumentExceptions(UserDto dto)
        {
            // Arrange
            service = new LocaleAuthService(fakeUserRepository.Object, fakeMapper.Object, fakeTokenGenerator.Object, new Validators.UserValidator(), mockedEmailSender.Object);

            // Act
            Func<Task> act = async () => await service.AuthUser(dto);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
        }

        private static IEnumerable<object[]> WhenDtoHasInvalidDataThenThrowsArgumentExceptionsArguments()
        {
            yield return new object[] { new UserDto() { Email = "test.user@api.com", UserName = "Test User", Password = "fdhggfhbgfbhh", LastName = "Test", FirstName = "User", Birthdate = "" } };
            yield return new object[] { new UserDto() { Email = "test.user@api.com", UserName = "Test User", Password = "dfgbhfbhb", LastName = "Test", FirstName = "", Birthdate = "25/06/2001" } };
            yield return new object[] { new UserDto() { Email = "test.user@api.com", UserName = "Test User", Password = "fbghfbfg", LastName = "", FirstName = "User", Birthdate = "25/06/2001" } };
            yield return new object[] { new UserDto() { Email = "test.user@api.com", UserName = "", Password = "dfxvgfdgb", LastName = "", FirstName = "User", Birthdate = "25/06/2001" } };
            yield return new object[] { new UserDto() { Email = "", UserName = "Test User", Password = "dfxvgfdgb", LastName = "", FirstName = "User", Birthdate = "25/06/2001" } };
            yield return new object[] { new UserDto() { Email = "", UserName = "Test User", Password = "dfxvgfdgb", LastName = "", FirstName = "User", Birthdate = "dsfdsf" } };
        }
    }
}
