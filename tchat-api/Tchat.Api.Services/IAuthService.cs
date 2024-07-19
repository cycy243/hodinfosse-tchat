using Tchat.Api.Models;

namespace Tchat.Api.Services
{
    public interface IAuthService
    {
        /// <summary>
        /// Authenticate a user base an a given set of informations
        /// If <paramref name="isForLogin"/> is <code>true</code> then it will log in the user.
        /// Otherwise, it will create a new user.
        /// </summary>
        /// <param name="userDto">Information of the user</param>
        /// <param name="isForLogin">If the user has to be logged in or registered</param>
        /// <returns>
        /// The data of the authenticated user.
        /// </returns>
        Task<UserDto> AuthUser(UserDto userDto, bool isForLogin = false);
    }

    public interface ICredentialValidator<D, R>
    {
        Task<R> ValidateCredentials(D credentials);
    }

    public enum AuthSource {
        Local,
        Google
    }
}
