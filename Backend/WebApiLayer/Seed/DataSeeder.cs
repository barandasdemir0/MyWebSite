using EntityLayer.Entities;
using Microsoft.AspNetCore.Identity;

namespace WebApiLayer.Seed;

public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var userManager = services.GetRequiredService<UserManager<AppUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        var configuration = services.GetRequiredService<IConfiguration>();

        string[] roles ={
            RoleConsts.Admin,RoleConsts.User,RoleConsts.Editor
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>
                {
                    Name = role
                });
            }
        }

        var adminEmail = configuration["Seed:AdminEmail"];
        var adminPassword = configuration["Seed:AdminPassword"];
        if (string.IsNullOrEmpty(adminEmail) || string.IsNullOrEmpty(adminPassword))
        {
            return;
        }
        var admin = await userManager.FindByEmailAsync(adminEmail);
        if (admin == null)
        {
            admin = new AppUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                Name = "Baran",
                Surname = "Daşdemir",
                EmailConfirmed = true,
                IsApproved = true
            };

            await userManager.CreateAsync(admin, adminPassword);
            await userManager.AddToRoleAsync(admin, RoleConsts.Admin);
        }
    }


}
