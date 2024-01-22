using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.Apis2.Dtos;
using Talabat.Apis2.ErrorsResponseHandling;
using Talabat2.core.Entities.Identity;
using Talabat2.core.Services;

namespace Talabat.Apis2.Controllers
{

    public class AccountsController : ApiControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("login")]
        //Email & password
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) { return Unauthorized(new ApiErrorResponse(401)); }

            var resulte = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!resulte.Succeeded) { return Unauthorized(new ApiErrorResponse(401)); }

            return Ok(new UserDto()
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            });
        }


        [HttpPost("register")]

        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            if (CheckEmailExists(model.Email).Result.Value)
            {
                return BadRequest(new ApiValidationErrorResponse()
                {
                    Errors = new string[] { "this email is alredy in use" }
                });
            };

            var user = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
                PhoneNumber = model.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) { return BadRequest(new ApiErrorResponse(400)); }
            return Ok(new UserDto()
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            });

        }


        [Authorize]
        [HttpGet]               // we will need this EndPoint at angular
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            //this builtIn property(User) Used it to get the email of the login user use his claims

            var email = User.FindFirstValue(ClaimTypes.Email);

            var user = await _userManager.FindByEmailAsync(email);

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,              //her we create new Token for this user after that we will git his current token from DB
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            });
        }


        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAdress()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var user = await _userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Email == email);

            var adress = _mapper.Map<Address, AddressDto>(user.Address);

            return Ok(adress);

        }


        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAdress(AddressDto updateAdress)
        {

            var email = User.FindFirstValue(ClaimTypes.Email);

            var userWitheAdress = await _userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Email == email);

            var address = _mapper.Map<AddressDto, Address>(updateAdress);

            address.Id = userWitheAdress.Address.Id;

            userWitheAdress.Address = address;

            var result = await _userManager.UpdateAsync(userWitheAdress);
            if (!result.Succeeded)
            {
                return BadRequest(new ApiErrorResponse(400));
            }


            return Ok(updateAdress);


        }


        [HttpGet("emailExists")] // i use this EndPoint at register EndPoint And the frontEnd will need it 
        public async Task<ActionResult< bool>> CheckEmailExists(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }


    }
}
