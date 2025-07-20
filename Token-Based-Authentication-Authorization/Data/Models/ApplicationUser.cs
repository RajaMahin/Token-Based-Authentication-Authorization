using Microsoft.AspNetCore.Identity;

namespace Token_Based_Authentication_Authorization.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? Custom { set; get; }
    }
}
