using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TaskMangement.Application.Helpers;
using TaskMangement.Domain.Models;

namespace TaskMangement.Infrastructure.Persistance.Authentication.Seeders
{
    public class AdminSeeder
    {
        public static async System.Threading.Tasks.Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var roleManager =scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            var userManager =scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            // Roles
            if (!await roleManager.RoleExistsAsync(Roles.Admin))
                await roleManager.CreateAsync(new IdentityRole<Guid>(Roles.Admin));

            if (!await roleManager.RoleExistsAsync(Roles.User))
                await roleManager.CreateAsync(new IdentityRole<Guid>(Roles.User));

            // Admin User
            var adminEmail = "admin@anyware.com";
            var admin = await userManager.FindByEmailAsync(adminEmail);

            if (admin is null)
            {
                admin = new User(name: "System Admin", email: adminEmail)
                {
                    UserName=adminEmail
                };
                var result = await userManager.CreateAsync(admin, "Admin@123");
                if (!result.Succeeded)
                {
                    var errors = string.Join(
                        ", ",
                        result.Errors.Select(e => e.Description));

                    throw new InvalidOperationException(
                        $"Failed to create admin user: {errors}");
                }
                await userManager.AddToRoleAsync(admin, Roles.Admin);

            }
        }
    }
}
