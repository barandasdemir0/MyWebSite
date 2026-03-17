using EntityLayer.Entities;
using Microsoft.AspNetCore.Identity;

namespace WebApiLayer.Seed;

public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var userManager = services.GetRequiredService<UserManager<AppUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>
            {
                Name = "Admin"
            });
        }

        if (!await roleManager.RoleExistsAsync("User"))
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>
            {
                Name = "User"
            });
        }

        if (!await roleManager.RoleExistsAsync("Editor"))
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>
            {
                Name = "Editor"
            });
        }

        var adminEmail = "barandasdemir.bd@gmail.com";
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

            await userManager.CreateAsync(admin, "CoC4CDpTht,6t&Rw.");
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }


}
