using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tchat.Api.Models;

namespace Tchat.Api.Services.Validators
{
    public class ContactQuestionValidator: AbstractValidator<ContactQuestionDto>
    {
        public ContactQuestionValidator() 
        {
            RuleFor(question => question.SenderEmail).NotEmpty().EmailAddress();
            RuleFor(question => question.SenderName).NotEmpty().MinimumLength(4);
            RuleFor(question => question.SenderFirstname).NotEmpty().MinimumLength(4);
            RuleFor(question => question.Subject).NotEmpty().MinimumLength(4);
            RuleFor(question => question.Content).NotEmpty().MinimumLength(25);
        }
    }
}
