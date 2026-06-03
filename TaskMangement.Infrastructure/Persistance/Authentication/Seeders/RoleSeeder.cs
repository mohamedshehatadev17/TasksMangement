using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TaskMangement.Infrastructure.Persistance.Authentication.Seed
{
    using Microsoft.AspNetCore.Identity;
    using TaskMangement.Application.Helpers;

    public static class RoleSeeder
    {
        public static async Task SeedAsync(
            RoleManager<IdentityRole<Guid>> roleManager)
        {
            string[] roles =
            {
                Roles.Admin,Roles.User
            };

            foreach (var role in roles)
            {
                var roleExists = await roleManager.RoleExistsAsync(role);
                if (!roleExists)
                {
                    await roleManager.CreateAsync(
                        new IdentityRole<Guid>
                        {
                            Name = role
                        });
                }
            }
        }
    }
}
