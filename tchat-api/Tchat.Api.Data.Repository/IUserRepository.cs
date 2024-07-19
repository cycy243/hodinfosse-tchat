using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tchat.Api.Domain;

namespace Tchat.Api.Data.Repository
{
    public interface IUserRepository
    {
        Task<User?> GetUserById(Guid id);

        Task<User?> GetUserByLogin(string email);

        Task<User> AddUser(User user);

        Task<User> UpdateUser(User user);

        Task<User> DeleteUser(Guid id);

        Task<IEnumerable<User>> GetAllUsers(int? count = null);

        Task<User?> FindUserByCredentials(string email, string password);
        Task<string> GenerateResetPasswordTokenForUser(User user);
        Task<bool> ResetPassword(User user, string code, string newPassword);
        
        /// <summary>
        /// Get the rules of a given user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<IEnumerable<string>> GetUserRoles(User user);
    }
}
