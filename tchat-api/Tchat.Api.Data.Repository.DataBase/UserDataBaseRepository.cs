using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tchat.Api.Domain;
using Tchat.Api.Exceptions.User;
using Tchat.API.Persistence;

namespace Tchat.Api.Data.Repository.DataBase
{
    public class UserDataBaseRepository : IUserRepository
    {
        private ApplicationDbContext _dbContext;
        private UserManager<User> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public UserDataBaseRepository(ApplicationDbContext dbContext, UserManager<User> userManager) 
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<User> AddUser(User user)
        {
            user.Id = Guid.NewGuid().ToString();
            user.BannedOn = null;
            user.DeletedOn = null;
            user.IsBanned = false;
            user.IsDeleted = false;
            var result = await _userManager.CreateAsync(user);
            return await GetUserByLogin(user.Email);
        }

        public async Task<User> DeleteUser(Guid id)
        {
            var user = _dbContext.Users.Find(id.ToString());
            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();
            // TODO: Make a logical delete
            return user;
        }

        public async Task<User?> FindUserByCredentials(string email, string password)
        {
            User foundedUser = await _userManager.FindByNameAsync(email) ?? await _userManager.FindByEmailAsync(email);
            if(foundedUser is null || !await _userManager.CheckPasswordAsync(foundedUser, password))
            {
                throw new UserNotFoundException("Wrong credentials");
            }
            return foundedUser;
        }

        public async Task<string> GenerateResetPasswordTokenForUser(User user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IEnumerable<User>> GetAllUsers(int? count = null)
        {
            if (count == null)
            {
                return await Task.FromResult(new List<User>(_dbContext.Users));
            }
            if (count < 0)
            {
                throw new ArgumentException("The property [count] should be greater than 0");
            }
            var messages = _dbContext.Users.OrderByDescending(u => u.Email).ToList();
            var result = messages.Take(count.Value);
            return await Task.FromResult(new List<User>(result));
        }

        public async Task<User?> GetUserByLogin(string email)
        {
            return await _userManager.FindByEmailAsync(email) ?? await _userManager.FindByNameAsync(email);
        }

        public async Task<User?> GetUserById(Guid id)
        {
            return await _userManager.FindByIdAsync(id.ToString());
        }

        public async Task<bool> ResetPassword(User user, string code, string newPassword)
        {
            var result = await _userManager.ResetPasswordAsync(user, code, newPassword);
            if(!result.Succeeded)
            {
                throw new ArgumentException("An error occured while reseting the password");
            }
            return true;
        }

        public async Task<IEnumerable<string>> GetUserRoles(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public Task<User> UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
