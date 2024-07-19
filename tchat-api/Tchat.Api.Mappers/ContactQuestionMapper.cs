using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tchat.Api.Domain;
using Tchat.Api.Models;

namespace Tchat.Api.Mappers
{
    public class ContactQuestionMapper: Profile
    {
        public ContactQuestionMapper()
        {
            CreateMap<ContactQuestion, ContactQuestionDto>()
                    .ReverseMap();
        }
    }
}
