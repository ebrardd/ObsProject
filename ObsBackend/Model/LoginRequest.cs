
using System.ComponentModel.DataAnnotations;

namespace Model.LoginRequest
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress] // İstersen, opsiyonel
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
