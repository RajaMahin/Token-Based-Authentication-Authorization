using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Token_Based_Authentication_Authorization.Data;
using Token_Based_Authentication_Authorization.Data.Models;
using Token_Based_Authentication_Authorization.Data.ViewModels;

namespace Token_Based_Authentication_Authorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthenticationController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            AppDbContext context,
            IConfiguration configuration

            )
        {


            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register-user")]
        public async Task<IActionResult> Register([FromBody] RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please, provide all the required fields.");
            }

            var userExists = await _userManager.FindByEmailAsync(registerVM.EmailAddress);


            if (userExists != null)
            {
                return BadRequest($"User {registerVM.EmailAddress} already exists.");
            }

            var user = new ApplicationUser
            {
                UserName = registerVM.UserName,
                Email = registerVM.EmailAddress,
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, registerVM.Password);

            if (result.Succeeded)
            {
                return Ok($"User {registerVM.EmailAddress} registered successfully.");
            }
            else
            {
                return BadRequest($"Error occurred while registering user {registerVM.EmailAddress}.");
            }
        }


        [HttpPost("login-user")]
        public async Task<IActionResult> Login([FromBody] LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please, provide all the required fields.");
            }

            var userExists = await _userManager.FindByEmailAsync(loginVM.EmailAddress);

            if (userExists != null && await _userManager.CheckPasswordAsync(userExists, loginVM.Password))
            {
                return Ok("User Signed In");
            }

            return Unauthorized();
        }

    }
}
