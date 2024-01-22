using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat2.core.Entities.Identity;

namespace Talabat2.Repository.Identity
{
    public static  class AppIdentityDbContextSeed
    {
        // this class(UserManager)  is Responsible for add & Update & delete & ... any user
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any())
            {
                var user = new AppUser()
                {
                    DisplayName = "Hassan Mostafa",
                    Email = "hassanmostfa01094977@gmail.com",
                    UserName = "hassan.mostafa",
                    PhoneNumber = "01094977497"
                };
                await userManager.CreateAsync(user,"Hassan123#");
               
            }
            
        }


    }
}
