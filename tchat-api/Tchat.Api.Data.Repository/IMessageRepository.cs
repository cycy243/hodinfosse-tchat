using Tchat.Api.Domain;

namespace Tchat.Api.Data.Repository
{
    public interface IMessageRepository
    {
        Message GetById(int id);

        /// <summary>
        /// Get the last <paramref name="count"/> messages from the repository
        /// </summary>
        /// <param name="count">Count of wanted messages</param>
        /// <returns>
        /// If <paramref name="count"/> is null, return all messages
        /// An <see cref="IEnumerable{Message}"/> containing the last <paramref name="count"/> messages
        /// </returns>
        /// <exception cref="ArgumentException">If <paramref name="count"/> if smaller or equal to 0</exception>
        Task<IEnumerable<Message>> GetAll(int? count = int.MaxValue, string? ipTchat = null);

        /// <summary>
        /// Add a message to the repository
        /// </summary>
        /// <param name="message">Message to add</param>
        /// <returns>
        /// A task containing the added message
        /// </returns>
        /// <exception cref="ArgumentException">If there some information is not well formated</exception>
        Task<Message> AddMessage(Message message);

        bool DeleteMessage(int id);

        Message UpdateMessage(Message message);
    }
}
