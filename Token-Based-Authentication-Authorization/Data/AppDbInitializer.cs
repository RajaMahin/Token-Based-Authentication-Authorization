using Microsoft.AspNetCore.Identity;
using Token_Based_Authentication_Authorization.Data.Helpers;

namespace Token_Based_Authentication_Authorization.Data
{
    public class AppDbInitializer
    {
        public static async Task SeedRolesToDb(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var roleManager =
                    serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                //Going to Add just two roles.

                //Manager  
                if (!await roleManager.RoleExistsAsync(UserRoles.Manager))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Manager));
                }

                //Student
                if (!await roleManager.RoleExistsAsync(UserRoles.Student))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Student));
                }
            }
        }
    }
}
