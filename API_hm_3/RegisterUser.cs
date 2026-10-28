using System.ComponentModel.DataAnnotations;

namespace API_hm_3
{
    //Full names are clearer than abbreviations: FirstName, LastName instead of Fname, Lname,
    //and Email (as in LoginUser) instead of Mail.
    //If you rename them, Users.txt still has the old names, so read it with
    //new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
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
