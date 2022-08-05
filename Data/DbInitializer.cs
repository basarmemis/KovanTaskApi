
using Microsoft.AspNetCore.Identity;
using Models.AuthenticationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class DbInitializer
    {
        public static async Task Initialize(DatabaseContext context, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {


            var roles = context.Roles;
            if (roles != null)
            {
                var checkAdminRole = roles.Any(rol => rol.Name == "Admin");
                if (!checkAdminRole)
                {
                    var adminRole = new IdentityRole
                    {
                        Name = "Admin",
                        NormalizedName = "ADMIN"
                    };
                    await context.Roles.AddAsync(adminRole);
                }
                var checkMemberRole = roles.Any(rol => rol.Name == "Member");
                if (!checkMemberRole)
                {
                    var memberRole = new IdentityRole
                    {
                        Name = "Member",
                        NormalizedName = "MEMBER"
                    };
                    await context.Roles.AddAsync(memberRole);
                }
            }

            var users = context.Users;
            if (users != null)
            {
                var checkAdminUser = users.Any(r => r.UserName == "admin");
                if (!checkAdminUser)
                {
                    var user = new User()
                    {
                        UserName = "admin",
                        Email = @"admin@admin.com",
                    };
                    await userManager.CreateAsync(user, "Secret_12345");
                    await userManager.AddToRolesAsync(user, new[] { "Member", "Admin" });
                }
            }
            await context.SaveChangesAsync();
        }
    }
}
