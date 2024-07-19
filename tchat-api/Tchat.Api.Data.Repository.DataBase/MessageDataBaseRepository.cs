using System.Linq;
using Tchat.Api.Domain;
using Tchat.API.Persistence;

namespace Tchat.Api.Data.Repository.DataBase
{
    public class MessageDataBaseRepository : IMessageRepository
    {
        ApplicationDbContext _dbContext;

        public MessageDataBaseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Message> AddMessage(Message message)
        {
            if(message.Content == null || message.Content == string.Empty)
            {
                throw new ArgumentException("The property [content] should not be null or empty");
            }
            if(message.SendDateTime == DateTime.MinValue)
            {
                throw new ArgumentException("The property [sendDateTime] should not be null");
            }
            var entity = _dbContext.Messages.Add(message);
            await _dbContext.SaveChangesAsync();
            return entity.Entity;
        }

        public bool DeleteMessage(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Message>> GetAll(int? count = int.MaxValue, string? ipTchat = null)
        {
            if(count is null)
            {
                return GetMessageByTchatIp(ipTchat);
            }
            if (count < 0)
            {
                throw new ArgumentException("The property [messageCount] should be greater than 0");
            }
            var messages = GetMessageByTchatIp(ipTchat);
            var result = messages.Take(count.Value);
            return await Task.FromResult(new List<Message>(result));
        }

        private IEnumerable<Message> GetMessageByTchatIp(string? ipTchat = null)
        {
            var gettedMsg = ipTchat is null ? _dbContext.Messages : _dbContext.Messages.Where(m => m.TchatIp == ipTchat);
            return gettedMsg.OrderByDescending(m => m.SendDateTime).ToList();
        }

        public Message GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Message UpdateMessage(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
