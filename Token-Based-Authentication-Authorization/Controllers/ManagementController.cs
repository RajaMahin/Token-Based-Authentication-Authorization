using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Token_Based_Authentication_Authorization.Data.Helpers;

namespace SchoolApp.API.Controllers
{
    [ApiController]
    [Authorize(Roles = UserRoles.Manager)]
    [Route("[controller]")]
    public class ManagementController : ControllerBase
    {
        public ManagementController()
        {
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Welcome to ManagementController");
        }
    }
}
