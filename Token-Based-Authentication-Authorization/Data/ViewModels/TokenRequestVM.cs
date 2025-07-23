using System.ComponentModel.DataAnnotations;

namespace Token_Based_Authentication_Authorization.Data.ViewModels
{
    public class TokenRequestVM
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string RefreshToken { set; get; }
    }
}
