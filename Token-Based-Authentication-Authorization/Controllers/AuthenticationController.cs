using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
                var tokenValue = await GenerateJWTTokenAsync(userExists);

                return Ok(tokenValue);
            }

            return Unauthorized();
        }


        private async Task<AuthResultVM> GenerateJWTTokenAsync(ApplicationUser user)
        {

            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),


                new Claim(JwtRegisteredClaimNames.Email, user.Email),

                new Claim(JwtRegisteredClaimNames.Sub, user.Email),

                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

            };


            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));


            var token =

                new JwtSecurityToken(

                issuer: _configuration["JWT:ValidIssuer"],

                audience: _configuration["JWT:ValidAudience"],

                expires: DateTime.UtcNow.AddMinutes(1),

                claims: authClaims,

                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)


                );


            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                isRevoked = false,
                UserId = user.Id,
                DateAdded = DateTime.UtcNow,
                DateExpire = DateTime.UtcNow.AddMonths(6),
                Token = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString()

            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return new AuthResultVM
            {
                Token = jwtToken,
                RefreshToken = refreshToken.Token,
                ExpiresAt = token.ValidTo
            };
        }
    }
}
