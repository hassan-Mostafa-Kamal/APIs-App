using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Talabat2.core.Entities.Identity;
using Talabat2.core.Services;

namespace Talabat2.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> userManager)
        {
            // create the Privete claims (User Defiend Claims)

            var AuthClaims = new List<Claim>()
            {                    //claimType   //claimValue
                new Claim(ClaimTypes.GivenName,user.DisplayName),
                new Claim(ClaimTypes.Email,user.Email),

            };

            //add the user Roles as a Privete Claims
            var userRoles = await userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                AuthClaims.Add(new Claim(ClaimTypes.Role,role));
            }

            //============================================================

            // create the Key
            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));


            //============================================================

            //jenerit the Token Object & Add the Privete claims & the Key to him And Create the Registered Claims & Header to it Directe

            var token = new JwtSecurityToken(

                //Create Registered Claims                   
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.Now.AddDays(double.Parse( _configuration["JWT:ExpiedAfter"])),
                //add Private Claims to the Token
                claims:AuthClaims,

                //add Key Claims to the Token And add the Header
                signingCredentials: new SigningCredentials(authKey,SecurityAlgorithms.HmacSha256Signature)

                );

            //create the Token It salf (lsd584pou)
           return  new JwtSecurityTokenHandler().WriteToken(token);


        }
    }
}
