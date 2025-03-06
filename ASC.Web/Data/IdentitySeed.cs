using ASC.Model.BaseTypes;
using ASC.Solution.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ASC.Web.Data
{
    public class IdentitySeed : IIdentitySeed
    {
        public async Task Seed(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager,
            IOptions<ApplicationSettings> options)
        {
            // Kiểm tra options không null
            if (options?.Value == null)
            {
                throw new ArgumentNullException(nameof(options), "ApplicationSettings cannot be null.");
            }

            var roles = options.Value.Roles?.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();

            // Tạo roles nếu chưa có
            foreach (var role in roles)
            {
                try
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        IdentityRole storageRole = new IdentityRole
                        {
                            Name = role
                        };

                        IdentityResult roleResult = await roleManager.CreateAsync(storageRole);
                        if (!roleResult.Succeeded)
                        {
                            Console.WriteLine($"Failed to create role '{role}': {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating role '{role}': {ex.Message}");
                }
            }

            // Tạo Admin
            await CreateUser(userManager, options.Value.AdminEmail, options.Value.AdminName, options.Value.AdminPassword, Roles.Admin.ToString());

            // Tạo Engineer
            await CreateUser(userManager, options.Value.EngineerEmail, options.Value.EngineerName, options.Value.EngineerPassword, Roles.Engineer.ToString());
        }

        private async Task CreateUser(UserManager<IdentityUser> userManager, string email, string userName, string password, string role)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine($"Skipping user creation: Missing required fields for {role}");
                return;
            }

            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                IdentityUser newUser = new IdentityUser
                {
                    UserName = userName,
                    Email = email,
                    EmailConfirmed = true,
                    LockoutEnabled = false
                };

                IdentityResult result = await userManager.CreateAsync(newUser, password);

                if (result.Succeeded)
                {
                    await userManager.AddClaimAsync(newUser, new System.Security.Claims.Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", email));
                    await userManager.AddClaimAsync(newUser, new System.Security.Claims.Claim("IsActive", "True"));
                    await userManager.AddToRoleAsync(newUser, role);
                }
                else
                {
                    Console.WriteLine($"Failed to create user {email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}
