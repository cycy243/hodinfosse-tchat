using FluentValidation;
using Tchat.Api.Models;

namespace Tchat.Api.Services.Validators
{
    public class UserValidator: AbstractValidator<UserDto>
    {
        public UserValidator()
        {
            RuleFor(user => user.Email).NotEmpty().EmailAddress();
            RuleFor(user => user.UserName).NotEmpty().MinimumLength(4);
            RuleFor(user => user.FirstName).NotEmpty().MinimumLength(4);
            RuleFor(user => user.LastName).NotEmpty().MinimumLength(4);
            RuleFor(user => user.Password).NotEmpty().MinimumLength(8);
            RuleFor(user => user.PicturePath).Matches("^(http(s?):)([/|.|\\w|\\s|-])*\\.(?:jpg|gif|png)$");
            RuleFor(user => user.Bio).MaximumLength(250);
            RuleFor(user => user.Birthdate).NotEmpty().Matches("[0-9]{2}/[0-9]{2}/[0-9]{4}");
        }
    }
}
