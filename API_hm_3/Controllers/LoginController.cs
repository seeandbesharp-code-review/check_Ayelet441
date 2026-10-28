using Microsoft.AspNetCore.Mvc;
//fix: Removed "using Microsoft.OpenApi;" and "using System.Reflection.PortableExecutable;" - they were added by mistake and are not used
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API_hm_3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //This controller holds the whole users API (sign up, get by id, login, update), not only login,
    //so a name like UsersController fits better. The route would become api/Users, so update the fetch URLs in wwwroot too
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
                        //fix: Reading a user doesn't create anything, so it returns Ok (200) and not CreatedAtAction (201)
                        return Ok(user);
                }
            }
            return NotFound();
        }

        //fix: The path never changes, so it can be readonly
        readonly string filePath = "./Users.txt";

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
        //The assignment asks a wrong login to return NoContent() (204)
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
                        //Login doesn't create a new user, so Ok(user) fits better than CreatedAtAction
                        return CreatedAtAction(nameof(Get), user);
                }
            }
            return NotFound("שם משתמש או סיסמא אינם נכונים");
        }

        // PUT api/<LoginController>/5
        [HttpPut("{id}")]
        //The assignment asks PUT to return Ok(): return Ok() at the end instead of CreatedAtAction (an update doesn't create a user)
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
                    {
                        textToReplace = currentUserInFile;
                        //fix: Found- break
                        break;
                    }
                }
            }
            if (textToReplace != string.Empty)
            {
                //fix: Keep the id, otherwise the updated user is saved without it
                userToUpdate.UserId = id;
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
