using System.ComponentModel.DataAnnotations;

namespace API_hm_3
{
    public class RegisterUser
    {
        public int UserId { get; set; }
        [EmailAddress]
        public string Mail { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Fname { get; set; }
        [Required]
        public string Lname { get; set; }
    }
}
