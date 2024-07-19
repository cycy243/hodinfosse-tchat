using Tchat.Api.Services.Contact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Tchat.Api.Data.Repository;
using Tchat.Api.Models;
using Tchat.Api.Services.Utils;
using Moq;
using Tchat.API.Persistence;
using Tchat.Api.Data.Repository.Database.Test.Utils;
using Tchat.Api.Domain;
using Tchat.Api.Mappers;
using FluentAssertions;
using Tchat.Api.Services.Validators;

namespace Tchat.Api.Services.Contact.Tests
{
    [TestClass()]
    public class ContactServiceTests
    {
        private Mock<ApplicationDbContext> mockDbContext;
        private Mock<IMapper> fakeMapper;
        private IContactService service;
        private Mock<IEmailSender> mockedEmailSender;
        private Mock<IContactRepository> mockedContactRepository;
        private Mock<IValidator<ContactQuestionDto>> fakeDtoValidator;
        private static string validContent = "vvvvvvvvvvvvvvvvvvvvvvvvv";

        [TestInitialize]
        public void Setup()
        {
            // Arrange
            mockDbContext = DataCreator.GetMockDbContext();
            fakeMapper = new Mock<IMapper>();
            fakeMapper.Setup(f => f.Map<ContactQuestion>(It.IsAny<ContactQuestionDto>())).Returns(new ContactQuestion() { SenderEmail = "lkjmkj", SenderFirstname = "mlkjlmkj", SenderName = "mkjmlkj", Subject = "nouihh", Content = "jilmjklj" });
            fakeDtoValidator = new Mock<IValidator<ContactQuestionDto>>();
            fakeDtoValidator.Setup(udv => udv.ValidateAsync(It.IsAny<ContactQuestionDto>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(new FluentValidation.Results.ValidationResult()));
            mockedEmailSender = new Mock<IEmailSender>();
            mockedContactRepository = new Mock<IContactRepository>();

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ContactQuestionMapper());
            });
            var mapper = mockMapper.CreateMapper();

            service = new ContactService(new ContactQuestionValidator(), mockedContactRepository.Object, mapper, mockedEmailSender.Object);
        }

        [DataTestMethod]
        [DynamicData(nameof(WhenMailArgsAreNotValidThenThrowsArgumentExceptionDatas), DynamicDataSourceType.Method)]
        public async Task WhenMailArgsAreNotValidThenThrowsArgumentException(ContactQuestionDto arg)
        {
            // Act
            Func<Task> askQuestion = async () => await service.AskQuestion(arg);

            // Assert
            await askQuestion.Should().ThrowAsync<ArgumentException>();
            mockedEmailSender.Verify(es => es.SendEmail(It.IsAny<MailArg>()), Times.Never);
        }

        [TestMethod]
        public async Task WhenMailInformationsAreValidThenMailIsSavedAndSended()
        {
            // Arrange
            var mailArgs = new ContactQuestionDto { SenderEmail = "test@test.com", Subject = "dsfsfd", Content = validContent, SenderFirstname = "fdgdfg", SenderName = "ddsfsdf" };

            // Act
            await service.AskQuestion(mailArgs);

            // Assert
            mockedEmailSender.Verify(es => es.SendEmail(It.IsAny<MailArg>()), Times.Exactly(2));
        }

        private static IEnumerable<object[]> WhenMailArgsAreNotValidThenThrowsArgumentExceptionDatas()
        {
            yield return new[] { new ContactQuestionDto() };
            yield return new[] { new ContactQuestionDto { SenderEmail = "", Subject = "", Content = "", SenderFirstname = "", SenderName = "" } };
            yield return new[] { new ContactQuestionDto { SenderEmail = "test@test.com", Subject = "dsfsfd", Content = validContent, SenderFirstname = "fdgdfg", SenderName = "" } };
            yield return new[] { new ContactQuestionDto { SenderEmail = "test@test.com", Subject = "dsfsfd", Content = validContent, SenderFirstname = "", SenderName = "ddsfsdf" } };
            yield return new[] { new ContactQuestionDto { SenderEmail = "test@test.com", Subject = "dsfsfd", Content = "", SenderFirstname = "fdgdfg", SenderName = "ddsfsdf" } };
            yield return new[] { new ContactQuestionDto { SenderEmail = "test@test.com", Subject = "", Content = validContent, SenderFirstname = "fdgdfg", SenderName = "ddsfsdf" } };
            yield return new[] { new ContactQuestionDto { SenderEmail = "", Subject = "dsfsfd", Content = validContent, SenderFirstname = "fdgdfg", SenderName = "ddsfsdf" } };
            yield return new[] { new ContactQuestionDto { SenderEmail = "testgfh", Subject = "dsfsfd", Content = validContent, SenderFirstname = "fdgdfg", SenderName = "ddsfsdf" } };
            yield return new[] { new ContactQuestionDto { SenderEmail = "test@test.com", Subject = "f", Content = validContent, SenderFirstname = "fdgdfg", SenderName = "ddsfsdf" } };
            yield return new[] { new ContactQuestionDto { SenderEmail = "test@test.com", Subject = "dsfsfd", Content = validContent, SenderFirstname = "g", SenderName = "ddsfsdf" } };
            yield return new[] { new ContactQuestionDto { SenderEmail = "test@test.com", Subject = "dsfsfd", Content = "f", SenderFirstname = "fdgdfg", SenderName = "ddsfsdf" } };
            yield return new[] { new ContactQuestionDto { SenderEmail = "test@test.com", Subject = "g", Content = validContent, SenderFirstname = "fdgdfg", SenderName = "ddsfsdf" } };
        }
    }
}