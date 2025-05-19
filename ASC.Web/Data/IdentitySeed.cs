using ASC.Model.BaseTypes;
using ASC.Solution.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace ASC.Web.Data
{
    public class IdentitySeed : IIdentitySeed
    {
        public async Task Seed(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager,
            IOptions<ApplicationSettings> options)
        {
            var roles = options.Value.Roles.Split(new char[] { ',' });

            // Create roles if they don't exist
            foreach (var role in roles)
            {
                try
                {
                    if (!roleManager.RoleExistsAsync(role).Result)
                    {
                        IdentityRole storageRole = new IdentityRole
                        {
                            Name = role
                        };
                        IdentityResult roleResult = await roleManager.CreateAsync(storageRole);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            // ... (Your existing code for role creation) ...

            // Create admin if he doesn't exist
            var admin = await userManager.FindByEmailAsync(options.Value.AdminEmail);
            if (admin == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = options.Value.AdminName,
                    Email = options.Value.AdminEmail,
                    EmailConfirmed = true
                };

                // Await the result of user creation
                IdentityResult result = await userManager.CreateAsync(user, options.Value.AdminPassword);

                // Add claims only if user creation is successful
                if (result.Succeeded)
                {

                    await userManager.AddClaimAsync(user, new System.Security.Claims.Claim("https://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", options.Value.AdminEmail));
                    await userManager.AddClaimAsync(user, new System.Security.Claims.Claim("IsActive", "True"));

                    // Add Admin to Admin roles
                    await userManager.AddToRoleAsync(user, Roles.Admin.ToString());
                }
                else
                {
                    // Handle user creation failure (e.g., log the error)
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error creating user: {error.Description}");
                    }
                }
            }

            var engineer = await userManager.FindByEmailAsync(options.Value.EngineerEmail);
            if (engineer == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = options.Value.EngineerName,
                    Email = options.Value.EngineerEmail,
                    EmailConfirmed = true,
                    LockoutEnabled = false
                };

                // Await the result of user creation
                IdentityResult result = await userManager.CreateAsync(user, options.Value.EngineerPassword);

                // Add claims only if user creation is successful
                if (result.Succeeded)
                {

                    await userManager.AddClaimAsync(user, new System.Security.Claims.Claim("https://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", options.Value.EngineerEmail));
                    await userManager.AddClaimAsync(user, new System.Security.Claims.Claim("IsActive", "True"));

                    // Add Admin to Admin roles
                    await userManager.AddToRoleAsync(user, Roles.Engineer.ToString());
                }
                else
                {
                    // Handle user creation failure (e.g., log the error)
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error creating user: {error.Description}");
                    }
                }
            }

        }
    }
}