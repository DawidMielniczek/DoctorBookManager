using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using System.Security.Cryptography;
using System;

namespace DoctorBookManager.Data
{
    public static class Seeder
    {
        public static async Task SeedData(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;

                try
                {
                    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                    var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();


                    await AddRoleIfNotExists(roleManager, DoctorBookManageConsts.AdminRoleName);
                    await AddRoleIfNotExists(roleManager, DoctorBookManageConsts.UserRoleName);
                    await AddRoleIfNotExists(roleManager, DoctorBookManageConsts.DoctorRoleName);
                    await AddAdminUserIfNotExists(userManager);
                    

                    var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    
                    Console.WriteLine("Błąd podczas inicjalizacji danych: " + ex.Message);
                }
            }
        }

        private static async Task AddRoleIfNotExists(RoleManager<IdentityRole> roleManager, string roleName)
        {
            var roleExists = await roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
        private static async Task AddAdminUserIfNotExists(UserManager<IdentityUser> userManager)
        {
            var adminEmail = DoctorBookManageConsts.AdminDefaultEmail;
            var adminPassword = DoctorBookManageConsts.AdminDefaultPassword;

            var adminUsers = await userManager.GetUsersInRoleAsync(DoctorBookManageConsts.AdminRoleName);

            if (!adminUsers.Any())
            {
                var adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    
                    await userManager.AddToRoleAsync(adminUser, DoctorBookManageConsts.AdminRoleName);
                }
                else
                {
                    
                    Console.WriteLine("Błąd podczas tworzenia konta administratora: " + string.Join(", ", result.Errors));
                }
            }
        }
    }








}
