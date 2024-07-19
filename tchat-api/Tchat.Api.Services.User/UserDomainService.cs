using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tchat.Api.Data.Repository;
using Tchat.Api.Exceptions.Utils;
using Tchat.Api.Models;
using Tchat.Api.Services.Args;

namespace Tchat.Api.Services.User
{
    public class UserDomainService : IDomainService<UserDto, UserSearchArgs>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserDomainService(IUserRepository userRepository, IMapper mapper) 
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> Create(UserDto element)
        {
            var user = await _userRepository.GetUserByLogin(element.Email) ?? await _userRepository.GetUserByLogin(element.UserName);
            if (user != null)
            {
                throw new RessourceAlreadyExists($"A user with the email {element.Email} or the username {element.UserName} already exists");
            }
            return _mapper.Map<UserDto>(await _userRepository.AddUser(_mapper.Map<Domain.User>(element)));
        }

        public async Task Delete(Guid id)
        {
            if(await _userRepository.GetUserById(id) == null)
            {
                throw new RessourceNotFound($"There wasn't any user found with the id: {id}");
            }
            await _userRepository.DeleteUser(id);
        }

        public async Task<IEnumerable<UserDto>> GetAll()
        {
            return (await _userRepository.GetAllUsers()).Select(_mapper.Map<UserDto>);
        }

        public async Task<UserDto> GetById(Guid id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                throw new RessourceNotFound($"There wasn't any user found with the id: {id}");
            }
            return _mapper.Map<UserDto>(user);
        }

        public async Task<IEnumerable<UserDto>> Search(UserSearchArgs search)
        {
            var result = new List<Domain.User>();
            var dtos = new List<UserDto>();
            var users = await _userRepository.GetAllUsers();
            if (search.Id != Guid.Empty)
            {
                var searchResult = users.FirstOrDefault(u => u.Id.Contains(search.Id.ToString()));
                if (searchResult != null)
                {
                    result.Add(searchResult);
                }
                return [];
            }
            else
            {
                if (search.Name != null)
                {
                    users = users.Where(u => u.LastName.Contains(search.Name));
                }
                if (search.Firstname != null)
                {
                    users = users.Where(u => u.FirstName.Contains(search.Firstname));
                }
                if (search.Email != null)
                {
                    users = users.Where(u => u.Email.Contains(search.Email));
                }
            }
            foreach (var user in users)
            {
                UserDto mappedUser = _mapper.Map<UserDto>(user);
                mappedUser.Roles = await _userRepository.GetUserRoles(user);
                dtos.Add(mappedUser);
            }
            return dtos;
        }

        public async Task<UserDto> Update(UserDto element)
        {
            if (await _userRepository.GetUserById(Guid.Parse(element.Id)) == null)
            {
                throw new RessourceNotFound($"There wasn't any user found with the id: {element.Id}");
            }
            return _mapper.Map<UserDto>(await _userRepository.UpdateUser(_mapper.Map<Domain.User>(element)));
        }
    }
}
