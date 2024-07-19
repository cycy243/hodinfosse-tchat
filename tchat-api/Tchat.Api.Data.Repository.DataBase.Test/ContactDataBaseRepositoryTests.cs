using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Tchat.Api.Data.Repository.Database.Test.Utils;
using Tchat.Api.Domain;
using Tchat.Api.Exceptions.Utils;
using Tchat.API.Args;
using Tchat.API.Persistence;

namespace Tchat.Api.Data.Repository.DataBase.Test
{
    [TestClass]
    public class ContactDataBaseRepositoryTests
    {
        private List<ContactQuestion> _messages;
        private Mock<API.Persistence.ApplicationDbContext> _mockedDbContext;
        private ApplicationDbContext _dbContext;
        private ContactDataBaseRepository _messageRepository;

        [TestInitialize]
        public void SetUp()
        {
            // Arrange
            _dbContext = DataCreator.CreateDbContext();
            _messages = new List<ContactQuestion>();
            _mockedDbContext = DataCreator.GetMockDbContext();
            _mockedDbContext.SetupGet(m => m.ContactQuestion).Returns(_messages.CreateMockDbSet().Object);

            _messageRepository = new ContactDataBaseRepository(_mockedDbContext.Object);
        }

        [TestCleanup]
        public void TearDown()
        {
            DataCreator.Destroy(_dbContext);
        }

        [TestMethod]
        public async Task AddMessageDoesNotThrowException()
        {
            // Act
            Func<Task> addResult = async () => await _messageRepository.AskQuestion(new ContactQuestion() { SenderEmail = "lkjmkj", SenderFirstname = "mlkjlmkj", SenderName = "mkjmlkj", Subject = "nouihh", Content = "jilmjklj" });

            // Assert
            await addResult.Should().NotThrowAsync();
            _messages.Should().HaveCount(1);
        }

        [TestMethod]
        public async Task WhenAddResponseToQuestionThenNotThrowsException()
        {
            // Act
            await _messageRepository.AskQuestion(new ContactQuestion() { SenderEmail = "lkjmkj", SenderFirstname = "mlkjlmkj", SenderName = "mkjmlkj", Subject = "nouihh", Content = "jilmjklj" });
            var messageToAnswer = _messages.First();
            Func<Task> answerResult = async () => await _messageRepository.AnswerQuestion(messageToAnswer.Id, "lkjlkj");

            // Assert
            await answerResult.Should().NotThrowAsync();
            _messages.First().Response.Should().Be("lkjlkj");
            _messages.First().DateRespond.Should().NotBeNull();
        }

        [TestMethod]
        public async Task WhenAddResponseToQuestionThatDoesNotExistThenThrowsRessourceNotFoundException()
        {
            // Act
            await _messageRepository.AskQuestion(new ContactQuestion() { SenderEmail = "lkjmkj", SenderFirstname = "mlkjlmkj", SenderName = "mkjmlkj", Subject = "nouihh", Content = "jilmjklj" });
            var messageToAnswer = _messages.First();
            Func<Task> answerResult = async () => await _messageRepository.AnswerQuestion(Guid.NewGuid(), "lkjlkj");

            // Assert
            await answerResult.Should().ThrowAsync<RessourceNotFound>();
        }

        [TestMethod]
        public async Task WhenGetMessagesButCountIsNullThenReturnAllTheMessages()
        {
            // Act
            var getMessagesResult = await _messageRepository.GetQuestions(new ContactQuestionSearchArgs());

            // Assert
            getMessagesResult.Should().HaveCount(_messages.Count);
        }

        [TestMethod]
        public async Task WhenGetMessagesButCountIsOneThenReturnTheLastOneMessages()
        {
            // Arrange
            _messageRepository = new ContactDataBaseRepository(_dbContext);

            var now = DateTime.Now;
            await _messageRepository.AskQuestion(new ContactQuestion() { SenderEmail = "lkjmkj", SenderFirstname = "mlkjlmkj", SenderName = "mkjmlkj", Subject = "nouihh", Content = "jilmjklj", DateSend = now.AddDays(1) });
            await _messageRepository.AskQuestion(new ContactQuestion() { SenderEmail = "lkjmkj", SenderFirstname = "mlkjlmkj", SenderName = "mkjmlkj", Subject = "nouihh", Content = "jilmjklj", DateSend = now.AddDays(-1) });

            // Act
            var getMessagesResult = await _messageRepository.GetQuestions(new ContactQuestionSearchArgs(Count: 1));

            // Assert
            getMessagesResult.Should().HaveCount(1);
            getMessagesResult.Should().OnlyContain(x => x.DateSend.Equals(now.AddDays(1)));
        }

        [TestMethod]
        public async Task WhenGetMessagesButCountIsLessThanZeroThenThrowInvalidArgumentException()
        {
            // Act
            Func<Task> getMessagesResult = async () => await _messageRepository.GetQuestions(new ContactQuestionSearchArgs(Count: -1));

            // Assert
            await getMessagesResult.Should().ThrowAsync<InvalidArgumentException>();
        }

        [TestMethod]
        public async Task WhenDeleteContactMessageThenOnlyLogicalDelete()
        {
            // Arrange
            _messageRepository = new ContactDataBaseRepository(_dbContext);

            var now = DateTime.Now;
            await _messageRepository.AskQuestion(new ContactQuestion() { SenderEmail = "lkjmkj", SenderFirstname = "mlkjlmkj", SenderName = "mkjmlkj", Subject = "nouihh", Content = "jilmjklj", DateSend = now.AddDays(1) });
            
            // Act
            var getMessagesResult = await _messageRepository.GetQuestions(new ContactQuestionSearchArgs());
            await _messageRepository.DeleteQuestion(getMessagesResult.ElementAt(0).Id);

            var messageAfterDelete = await _messageRepository.GetQuestions(new ContactQuestionSearchArgs(IsDeleted: true));

            // Assert
            messageAfterDelete.Should().HaveCount(1)
                .And.OnlyContain(x => x.IsDeleted);
        }
    }
}
