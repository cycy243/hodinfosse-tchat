using System.Linq;
using Tchat.Api.Models;
using Tchat.Api.Domain;
using Tchat.Api.Services;
using Google.Apis.Auth;
using Tchat.Api.Data.Repository;
using Tchat.Api.Services.Utils;
using Microsoft.AspNetCore.Identity;
using Tchat.Api.Mappers;
using AutoMapper;
using System.ComponentModel;
using Tchat.Api.Exceptions.Utils;
using Google.Apis.Auth.OAuth2;
using Google.Apis.PeopleService.v1.Data;
using Google.Apis.PeopleService.v1;
using Google.Apis.Services;
using System.Net;
using Tchat.Api.Services.Args;

namespace Tchat.Api.Service.Auth
{
    public class GoogleAuthService : IAuthService
    {
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;
        private readonly ICredentialValidator<string, GoogleJsonWebSignature.Payload> _tokenValidator;
        private readonly IDomainService<UserDto, UserSearchArgs> _userDomainService;

        public GoogleAuthService(ITokenGenerator tokenGenerator, IMapper mapper, IEmailSender emailSender, ICredentialValidator<string, GoogleJsonWebSignature.Payload> tokenValidator, IDomainService<UserDto, UserSearchArgs> userDomainService) 
        {
            _tokenGenerator = tokenGenerator;
            _emailSender = emailSender;
            _mapper = mapper;
            _tokenValidator = tokenValidator;
            _userDomainService = userDomainService;
        }

        public async Task<UserDto> AuthUser(UserDto userDto, bool isForLogin = false)
        {
            var response = await _tokenValidator.ValidateCredentials(userDto.Token);
            if (response.Email == null)
            {
                throw new ArgumentException("The given credentials aren't valid");
            }
            var user = (await _userDomainService.Search(new UserSearchArgs() { Email = response.Email })).FirstOrDefault();
            // If the user doesn't exist, we create it
            if (user == null)
            {
                user = await _userDomainService.Create(DtoFromGooglePayload(response));
                _emailSender.SendEmail(new MailArg(
                    user.Email,
                    "Welcome to Tchat",
                    "Welcome to Tchat, we hope you will enjoy our services.\n"
                ));
            }

            User domainEntity = _mapper.Map<Domain.User>(user);
            user.Token = _tokenGenerator.GenerateToken(domainEntity, user.Roles!);
            return user;
        }

        private UserDto DtoFromGooglePayload(GoogleJsonWebSignature.Payload payload)
        {
            return new UserDto
            {
                UserName = payload.Name.ToLower().Replace(" ", "_"),
                FirstName = payload.GivenName,
                LastName = payload.FamilyName,
                PicturePath = payload.Picture,
                Email = payload.Email,
            };
        }
    }

    public class GoogleTokenValidator: ICredentialValidator<string, GoogleJsonWebSignature.Payload>
    {
        public async Task<GoogleJsonWebSignature.Payload> ValidateCredentials(string credentials)
        {
            try
            {
                return await GoogleJsonWebSignature.ValidateAsync(credentials);
            }
            catch(InvalidJwtException e)
            {
                throw new InvalidArgumentException(e.Message, "The provided token isn't valid");
            }
        }
    }
}
