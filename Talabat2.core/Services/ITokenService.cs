using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat2.core.Entities.Identity;

namespace Talabat2.core.Services
{
    public interface ITokenService
    {
                                                   //this peremter for Get User Role To use It as A private claims
        Task<string> CreateTokenAsync(AppUser user,UserManager<AppUser> userManager);
    }
}
