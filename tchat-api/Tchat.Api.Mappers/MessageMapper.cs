using AutoMapper;
using Tchat.Api.Domain;
using Tchat.Api.Models;

namespace Tchat.Api.Mappers
{
    public class MessageMapper: Profile
    {
        public MessageMapper()
        {
            CreateMap<Message, MessageDto>()
                .ReverseMap();
        }
    }
}
