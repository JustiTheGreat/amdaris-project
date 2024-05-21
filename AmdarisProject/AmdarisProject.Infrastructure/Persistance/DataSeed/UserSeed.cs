using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.RequestDTOs.CreateDTOs;
using AmdarisProject.Application.Dtos.RequestDTOs;
using AmdarisProject.Domain.Models.CompetitorModels;
using AmdarisProject.Presentation;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AmdarisProject.Infrastructure.Persistance.DataSeed
{
    internal class UserSeed
    {
        public static async Task Seed(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!userManager.Users.Any())
            {
                var administrator = new IdentityUser()
                {
                    UserName = "string",
                    Email = "user@example.com",
                };

                await userManager.CreateAsync(administrator, "string");

                string roleName = nameof(UserRole.Administrator);
                var role = await roleManager.FindByNameAsync(roleName);

                if (role is null)
                {
                    role = new IdentityRole(roleName);
                    await roleManager.CreateAsync(role);
                }

                await userManager.AddToRoleAsync(administrator, roleName);
            }
        }
    }
}
