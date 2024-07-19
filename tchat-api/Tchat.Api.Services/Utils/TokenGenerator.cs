using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Tchat.Api.Domain;

namespace Tchat.Api.Services.Utils
{
    public class TokenGenerator: ITokenGenerator
    {
        private readonly string _jwtSecret;
        private readonly string _audience;
        private readonly string _issuer;
        private readonly int _validityHours;

        public TokenGenerator(JwtConfiguration jwtConfiguration)
        {
            _jwtSecret = jwtConfiguration.Secret;
            _audience = jwtConfiguration.Audience;
            _issuer = jwtConfiguration.Issuer;
            _validityHours = jwtConfiguration.ValidityHours;
        }

        /// <summary>
        /// Generate a token for the given user.
        /// </summary>
        /// <param name="user">User for wich we want to create a token</param>
        /// <returns>
        /// A Task that contains the token of the user.
        /// </returns>
        public string GenerateToken(Tchat.Api.Domain.User user, IEnumerable<string> roles)
        {

            // Set the claims of the user
            // like its name, uid and an id for the token
            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim("user_id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            // Add as claims the user's role
            foreach (var userRole in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            // Create a key encryption
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
            // Create signing credentials with the generated key
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            // Create a token (JwtSecurityToken)
            var token = new JwtSecurityToken(
            claims: authClaims, // Set the user's claims to the token's claims 
                expires: DateTime.UtcNow.AddHours(_validityHours), // Add an expiring date
                audience: _audience, // set the audience
                issuer: _issuer, // set the issuer
                signingCredentials: cred // set the signingcredentials
            );
            // Create and return a string as a token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
