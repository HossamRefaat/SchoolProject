using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities.Identity;

namespace SchoolProject.Infrastructure.Seeder;

public static class UserSeeder
{
    public static async Task SeedAsync(UserManager<User> userManager)
    {
        var usersCount = await userManager.Users.CountAsync();

        if (usersCount == 0)
        {
            // Seed admin user
            var defaultAdminUser = new User()
            {
                UserName = "admin",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                FullName = "Admin User",
                PhoneNumber = "1234567890",
                PhoneNumberConfirmed = true,
                Country = "EG",
                Address = "Egypt",
            };
            await userManager.CreateAsync(defaultAdminUser, "Admin@123");  
            await userManager.AddToRoleAsync(defaultAdminUser, "Admin");
        }
    }
}
