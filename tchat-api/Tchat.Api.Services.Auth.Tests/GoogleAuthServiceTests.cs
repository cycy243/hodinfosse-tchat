using AutoMapper;
using Google.Apis.Auth;
using Moq;
using Tchat.Api.Data.Repository;
using Tchat.Api.Data.Repository.Database.Test.Utils;
using Tchat.Api.Domain;
using Tchat.Api.Exceptions.Utils;
using Tchat.Api.Models;
using Tchat.Api.Service.Auth;
using Tchat.Api.Services.Args;
using Tchat.Api.Services.Utils;
using Tchat.API.Persistence;

namespace Tchat.Api.Services.Auth.Tests
{
    [TestClass]
    public class GoogleAuthServiceTests
    {
        private Mock<IDomainService<UserDto, UserSearchArgs>> mockedUserDomainService;
        private Mock<ApplicationDbContext> mockedDBContext;
        private Mock<ICredentialValidator<string, GoogleJsonWebSignature.Payload>> mockedTokenValidator;
        private Mock<IEmailSender> mockedEmailSender;
        private Mock<ITokenGenerator> mockedTokenGenerator;

        [TestInitialize]
        public void SetUp()
        {
            mockedUserDomainService = new Mock<IDomainService<UserDto, UserSearchArgs>>();
            mockedDBContext = DataCreator.GetMockDbContext();
            mockedTokenValidator = new Mock<ICredentialValidator<string, GoogleJsonWebSignature.Payload>>();
            mockedEmailSender = new Mock<IEmailSender>();
            mockedTokenGenerator = new Mock<ITokenGenerator>();
        }

        [TestMethod]
        public async Task WhenUserNotExistThenCreateUser()
        {
            // Arrange
            mockedUserDomainService.Setup(mus => mus.Search(It.IsAny<UserSearchArgs>())).ReturnsAsync(Array.Empty<UserDto>());
            mockedUserDomainService.Setup(mus => mus.Create(It.IsAny<UserDto>())).ReturnsAsync(new UserDto() { Email = "test.user@api.com", UserName = "Test User" });
            mockedTokenValidator.Setup(ftv => ftv.ValidateCredentials(It.IsAny<string>())).ReturnsAsync(new GoogleJsonWebSignature.Payload { Email = "test.user@api.com", Name = "Test User" });
            var fakeMapper = new Mock<IMapper>();
            fakeMapper.Setup(fm => fm.Map<UserDto>(It.IsAny<Tchat.Api.Domain.User>())).Returns(new UserDto() { Email = "test.user@api.com", UserName = "Test User" });
            var service = new GoogleAuthService(mockedTokenGenerator.Object, fakeMapper.Object, mockedEmailSender.Object, mockedTokenValidator.Object, mockedUserDomainService.Object);

            // Act
            var user = await service.AuthUser(new UserDto() { Token = "" });

            // Assert
            mockedUserDomainService.Verify(fakeUserRepository => fakeUserRepository.Create(It.IsAny<UserDto>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task WhenTokenCanNotBeValidatedThenThrowsInvalidArgumentExceptions()
        {
            // Arrange
            mockedTokenValidator.Setup(ftv => ftv.ValidateCredentials(It.IsAny<string>())).Throws(new InvalidArgumentException("", ""));
            var fakeMapper = new Mock<IMapper>();
            var service = new GoogleAuthService(mockedTokenGenerator.Object, fakeMapper.Object, mockedEmailSender.Object, mockedTokenValidator.Object, mockedUserDomainService.Object);

            // Act
            var user = await service.AuthUser(new UserDto() { Token = "" });
        }
    }
}