using Tchat.Api.Models;

namespace Tchat.Api.Services
{
    public interface IMessageService
    {

        Task<MessageDto> SendMessage(MessageDto messageDto);

        Task<IEnumerable<MessageDto>> GetMessages(int count = int.MaxValue);

    }
}
