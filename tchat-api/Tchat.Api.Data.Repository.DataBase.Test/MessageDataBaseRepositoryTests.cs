using Moq;
using Tchat.Api.Data.Repository.Database.Test.Utils;
using Tchat.Api.Domain;

namespace Tchat.Api.Data.Repository.DataBase.Test
{
    [TestClass]
    public class MessageDataBaseRepositoryTests
    {
        private readonly DateTime _dateTime = new DateTime(2021, 1, 1);

        [DataTestMethod]
        [DataRow(10, 10, 10, "0.0.0.0", "When get 10 items and there is 10 items then it should returns 10 items")]
        [DataRow(10, 5, 5, "0.0.0.0", "When get 5 items and there is 10 items then it should returns 5 items")]
        [DataRow(3, 10, 3, "0.0.0.0", "When get 10 items and there is 3 items then it should returns 3 items")]
        [DataRow(10, null, 10, "0.0.0.0", "When get null for items and there is 10 items then it should returns all the items")]
        [DataRow(2, 0, 0, null, "When get 0 items and there is 2 items then it should returns the 0 items")]
        [DataRow(2, 2, 0, "1.1.1.1", "When get 0 items and there is 2 items then it should returns the 0 items")]
        public async Task GetAllMessageTest(int itemCount, int? toGetCount, int expectedCount, string? ipTchat, string message) {
            // Arrange
            var dbContextMock = DataCreator.GetMockDbContext();
            dbContextMock.Setup(x => x.Messages).Returns(DataCreator.GetMockedMessageDbSet(itemCount, "0.0.0.0").Object);

            var messageRepository = new MessageDataBaseRepository(dbContextMock.Object);

            // Act
            var result = await messageRepository.GetAll(toGetCount, ipTchat);

            // Assert
            Assert.AreEqual(expectedCount, result.Count(), toGetCount is null ? $"When get {toGetCount} items and there is {itemCount} items then it should returns the {expectedCount} items" : "When get null for items it should returns all the items");
        }

        [DataTestMethod]
        [DynamicData(nameof(WhenMessageNotWellFormattedThenThrowArgumentExceptionData), DynamicDataSourceType.Method)]
        [ExpectedException(typeof(ArgumentException))]
        public async Task WhenMessageNotWellFormattedThenThrowArgumentException(string message, DateTime dateTime)
        {
            // Arrange
            var dbContextMock = DataCreator.GetMockDbContext();

            var messageRepository = new MessageDataBaseRepository(dbContextMock.Object);

            // Act
            var result = await messageRepository.AddMessage(new Message() { SendDateTime = dateTime, Content = message});
        }

        private static IEnumerable<object[]> WhenMessageNotWellFormattedThenThrowArgumentExceptionData()
        {
            yield return new object[] { "", new DateTime() };
            yield return new object[] { null, new DateTime() };
            yield return new object[] { "null", null };
        }
    }
}