using AutoMapper;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tchat.Api.Data.Repository;
using Tchat.Api.Domain;
using Tchat.Api.Models;
using Tchat.Api.Services.Utils;

namespace Tchat.Api.Services.Auth
{
    public class LocaleAuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IEmailSender _emailSender;
        private readonly IValidator<UserDto> _validator;

        public LocaleAuthService(IUserRepository userRepository, IMapper mapper, ITokenGenerator tokenGenerator, IValidator<UserDto> validator, IEmailSender emailSender)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _tokenGenerator = tokenGenerator;
            _validator = validator;
            _emailSender = emailSender;
        }

        public async Task<UserDto> AuthUser(UserDto userDto, bool isForLogin = false)
        {
            var user = isForLogin ? await LoginUser(userDto) : await RegisterUser(userDto);
            var resultDto = _mapper.Map<UserDto>(user);
            resultDto.Roles = await _userRepository.GetUserRoles(user);
            resultDto.Token = _tokenGenerator.GenerateToken(user, resultDto.Roles);
            return resultDto;
        }

        private async Task<Domain.User> RegisterUser(UserDto userDto)
        {
            var errors = await _validator.ValidateAsync(userDto);
            if(!errors.IsValid)
            {
               throw new ArgumentException(errors.Errors.Select(e => e.ErrorMessage).Aggregate((a, b) => $"{a}, {b}"));
            }
            if((await _userRepository.GetUserByLogin(userDto.UserName ?? userDto.Email)) != null)
            {
                throw new ArgumentException("User already exists");
            }
            var user = _mapper.Map<Domain.User>(userDto);
            _emailSender.SendEmail(new MailArg(user.Email!, "Welcome", "Welcome to our service"));
            return await _userRepository.AddUser(user);
        }

        private async Task<Domain.User> LoginUser(UserDto userDto)
        {
            if(userDto.UserName == null && userDto.Email == null)
            {
                throw new ArgumentException("UserName or Email is required");
            }
            if(userDto.Password == null)
            {
                throw new ArgumentException("Password is required");
            }
            var user = await _userRepository.FindUserByCredentials(userDto.UserName ?? userDto.Email, userDto.Password);
            return user;
        }
    }
}
