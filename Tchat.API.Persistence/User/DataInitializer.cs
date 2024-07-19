using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tchat.API.Data.Persistence.User
{
    public class DataInitializer
    {
        public static void SeedRole(RoleManager<IdentityRole> roleManager)
        {
            if (roleManager.RoleExistsAsync(nameof(UserRoleEnum.Admin)).Result == false)
            {
                IdentityRole admin = new IdentityRole() { Name = nameof(UserRoleEnum.Admin) };
                var resultAdmin = roleManager.CreateAsync(admin);
                resultAdmin.Wait();
            }
            if (roleManager.RoleExistsAsync(nameof(UserRoleEnum.User)).Result == false)
            {
                IdentityRole user = new IdentityRole() { Name = nameof(UserRoleEnum.User) };
                var resultUser = roleManager.CreateAsync(user);
                resultUser.Wait();
            }
        }
        public static void Seed(UserManager<Api.Domain.User> userManager)
        {
            IEnumerable<(Api.Domain.User, string, string[])> users = new List<(Api.Domain.User, string, string[])>()
            {
                (
                    new Api.Domain.User
                    {
                        Email = "touka_ki@example.com",
                        LastName = "Kirishima",
                        FirstName = "Touka",
                        UserName = "touka_ki",
                        PicturePath = "url"
                    },
                    "Password123@",
                    new string[] { nameof(UserRoleEnum.User) }
                ),
                (
                    new Api.Domain.User
                    {
                        Email = "cyrilauquier@gmail.com",
                        LastName = "Auquier",
                        FirstName = "Cyril",
                        UserName = "cyril_auquier",
                        PicturePath = "url"
                    },
                    "Password123@",
                    new string[] { nameof(UserRoleEnum.User), nameof(UserRoleEnum.Admin) }
                )
            };
            foreach (var (user, password, roles) in users)
            {
                var resultUser = userManager.CreateAsync(user, password).Result;
                if (resultUser.Succeeded)
                {
                    foreach (var role in roles)
                    {
                        var resultAddUserToProd = userManager.AddToRoleAsync(user, role).Result;
                    }
                }
            }
        }

    }
}
