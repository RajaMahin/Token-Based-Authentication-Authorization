using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Token_Based_Authentication_Authorization.Data;
using Token_Based_Authentication_Authorization.Data.Models;

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
    }
}
