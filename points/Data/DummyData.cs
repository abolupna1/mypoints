using Microsoft.AspNetCore.Identity;
using points.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace points.Data
{
    public class DummyData
    {
        
        public static async Task Initialize(ApplicationDbContext context,
                                           UserManager<AppUser> userManager,
                                           RoleManager<AppRole> roleManager)
        {
            context.Database.EnsureCreated();
            int adminID = 0;
            string roleAdmin = "Admin";
            string roleAdminAr = "مدير النظام";

            string roleAgent = "Agent";
            string roleAgentAr = "وكيل";

            string roleDean = "Dean";
            string roleDeanAr = "عميد";

            if (await roleManager.FindByNameAsync(roleAdmin) == null)
            {
                var resault = await roleManager.CreateAsync(new AppRole(roleAdmin, roleAdminAr));
            }

            if (await roleManager.FindByNameAsync(roleAgent) == null)
            {
                var resault = await roleManager.CreateAsync(new AppRole(roleAgent, roleAgentAr));
            }
            if (await roleManager.FindByNameAsync(roleDean) == null)
            {
                var resault = await roleManager.CreateAsync(new AppRole(roleDean, roleDeanAr));

            }
            string password = "2329472589";

            if (await userManager.FindByNameAsync("abolupna1@gmail.com") == null)
            {
                var user = new AppUser
                {
                    UserName = "abolupna1@gmail.com",
                    Email = "abolupna1@gmail.com",
                    FullName = "ايدوم ابراهيم"

                };
            
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {

                    await userManager.AddToRoleAsync(user, roleAdmin);
                 
                }
                adminID = user.Id;
            }

        }
    }
}
