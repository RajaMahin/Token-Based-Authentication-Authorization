using System.ComponentModel.DataAnnotations;

namespace Token_Based_Authentication_Authorization.Data.ViewModels
{
    public class RegisterVM
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
