using Tchat.Api.Data.Repository;
using Tchat.Api.Models;
using Tchat.Api.Services.Messages.Utils;
using PusherServer;
using Tchat.Api.Domain;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Tchat.Api.Services.Utils.Extensions;

namespace Tchat.Api.Services.Messages
{
    public class PusherMessageService: IMessageService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IPusher _pusher;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PusherMessageService(IPusher pusher, IUserRepository userRepository, IMessageRepository messageRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _messageRepository = messageRepository;
            _pusher = pusher;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<MessageDto>> GetMessages(int count = int.MaxValue)
        {
            var users = await _userRepository.GetAllUsers();
            string userIp = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? throw new ArgumentNullException("The sender ip shouldn't be null");
            return (await _messageRepository.GetAll(count, userIp == "::1" ? "localhost" : userIp.Replace(":", "_"))).Select(_mapper.Map<MessageDto>).Select(m =>
            {
                m.UserName = users.FirstOrDefault(u => u.Id == m.UserId.ToString())?.UserName;
                return m;
            });
        }

        public async Task<MessageDto> SendMessage(MessageDto messageDto)
        {
            var user = await _userRepository.GetUserById(messageDto.UserId);

            if(user == null)
            {
                throw new ArgumentException("No user found for the given id");
            }

            var msgToAdd = _mapper.Map<Message>(messageDto);
            string userIp = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? throw new ArgumentNullException("The sender ip shouldn't be null");
            msgToAdd.TchatIp = userIp == "::1" ? "localhost" : userIp.Replace(":", "_");
            var message = await _messageRepository.AddMessage(msgToAdd);

            var result = await _pusher.TriggerAsync(
                $"global-tchat-{msgToAdd.TchatIp}",
                "send-message",
                new
                {
                    id = message.Id,
                    message = messageDto.Content,
                    uid = user.Id,
                    username = user.UserName,
                    sendDateTime = DateTime.Now
                }
            );

            return _mapper.Map<MessageDto>(message);
        }
    }
}
