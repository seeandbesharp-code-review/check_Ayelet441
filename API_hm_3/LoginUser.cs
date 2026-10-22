using System.ComponentModel.DataAnnotations;

namespace API_hm_3
{
    public class LoginUser
    {
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
