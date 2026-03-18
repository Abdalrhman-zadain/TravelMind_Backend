using Microsoft.AspNetCore.Mvc;
using TravelMind.Business.Business;
using TravelMind.DataAccess.DTO;
using BCrypt.Net;

//to tell the compiler that when we say "User",
//we mean the User class in the TravelMind.Business.Business namespace,
//not the System.User class or any other User class that might exist.
using AppUser = TravelMind.Business.Business.User;
namespace TravelMind.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // POST /api/auth/register
        [HttpPost("register")]
        public ActionResult Register(UserDTO dto)
        {
            if (string.IsNullOrEmpty(dto.Name) ||
                string.IsNullOrEmpty(dto.Email) ||
                string.IsNullOrEmpty(dto.PasswordHash))
                return BadRequest("Name, Email and Password are required!");

            // Check if email already exists
            var existingUser = AppUser.FindByEmail(dto.Email);
            if (existingUser != null)
                return BadRequest("Email already exists!");

            // Hash the password
            dto.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash);

            var user = new AppUser(dto, AppUser.enMode.AddNew);
            if (user.Save())
                return Ok(new
                {
                    message = "User registered successfully!",
                    userId = user.Id,
                    name = user.Name,
                    email = user.Email
                });

            return StatusCode(500, "Failed to register user!");
        }

        // POST /api/auth/login
        [HttpPost("login")]
        public ActionResult Login(UserDTO dto)
        {
            if (string.IsNullOrEmpty(dto.Email) ||
                string.IsNullOrEmpty(dto.PasswordHash))
                return BadRequest("Email and Password are required!");

            // Find user by email
            var user = AppUser.FindByEmail(dto.Email);
            if (user == null)
                return Unauthorized("Invalid email or password!");

            // Verify password
            bool isValid = BCrypt.Net.BCrypt.Verify(dto.PasswordHash, user.PasswordHash);
            if (!isValid)
                return Unauthorized("Invalid email or password!");

            return Ok(new
            {
                message = "Login successful!",
                userId = user.Id,
                name = user.Name,
                email = user.Email,
                language = user.PreferredLanguage
            });
        }
    }
}
