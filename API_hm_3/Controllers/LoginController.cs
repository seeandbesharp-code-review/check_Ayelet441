using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi;
using System.Reflection.PortableExecutable;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API_hm_3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        // GET: api/<LoginController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<LoginController>/5
        [HttpGet("{id}")]
        public ActionResult<RegisterUser> Get(int id)
        {
            using (StreamReader reader = System.IO.File.OpenText(filePath))
            {
                string? currentUserInFile;
                while ((currentUserInFile = reader.ReadLine()) != null)
                {
                    RegisterUser user = JsonSerializer.Deserialize<RegisterUser>(currentUserInFile);
                    if (user.UserId == id)
                        return CreatedAtAction(nameof(Get), user);
                }
            }
            return NotFound();
        }

        string filePath = "./Users.txt";

        // POST api/<LoginController>
        [HttpPost]
        public IActionResult Post([FromBody] RegisterUser registerUser)
        {
            int numberOfUsers = System.IO.File.ReadLines(filePath).Count();
            registerUser.UserId = numberOfUsers + 1;
            string userJson = JsonSerializer.Serialize(registerUser);
            System.IO.File.AppendAllText(filePath, userJson + Environment.NewLine);
            return CreatedAtAction(nameof(Get), new { id = registerUser.UserId }, registerUser);
        }

        // POST api/<LoginController>
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginUser loginUser)
        {
            using (StreamReader reader = System.IO.File.OpenText(filePath))
            {
                string? currentUserInFile;
                while ((currentUserInFile = reader.ReadLine()) != null)
                {
                    RegisterUser user = JsonSerializer.Deserialize<RegisterUser>(currentUserInFile);
                    if (user.Mail == loginUser.Email && user.Password == loginUser.Password)
                        return CreatedAtAction(nameof(Get), user);
                }
            }
            return NotFound("שם משתמש או סיסמא אינם נכונים");
        }

        // PUT api/<LoginController>/5
        [HttpPut("{id}")]
        public ActionResult<RegisterUser> Put(int id, [FromBody] RegisterUser userToUpdate)
        {
            string textToReplace = string.Empty;
            using (StreamReader reader = System.IO.File.OpenText(filePath))
            {
                string currentUserInFile;
                while ((currentUserInFile = reader.ReadLine()) != null)
                {
                    RegisterUser user = JsonSerializer.Deserialize<RegisterUser>(currentUserInFile);
                    if (user.UserId == id)
                        textToReplace = currentUserInFile;
                }
            }
            if (textToReplace != string.Empty)
            {
                string text = System.IO.File.ReadAllText(filePath);
                text = text.Replace(textToReplace, JsonSerializer.Serialize(userToUpdate));
                System.IO.File.WriteAllText(filePath, text);
            }
            return CreatedAtAction(nameof(Get), userToUpdate);
        }

        // DELETE api/<LoginController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
