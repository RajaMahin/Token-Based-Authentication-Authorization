﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Token_Based_Authentication_Authorization.Data.Models
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }
        public string Token { get; set; }
        public string JwtId { get; set; }

        public bool isRevoked { get; set; }
        public DateTime DateAdded { get; set; }

        public DateTime DateExpire { get; set; }
        public string UserId { get; set; }


        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }
    }
}
