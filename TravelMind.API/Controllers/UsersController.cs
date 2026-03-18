using Microsoft.AspNetCore.Mvc;
using TravelMind.Business.Business;
using TravelMind.DataAccess.DTO;

//to tell the compiler that when we say "User",
//we mean the User class in the TravelMind.Business.Business namespace,
//not the System.User class or any other User class that might exist.
using AppUser = TravelMind.Business.Business.User;
namespace TravelMind.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // GET /api/users
        [HttpGet]
        public ActionResult<List<UserDTO>> GetAll()
        {
            var list = AppUser.GetAll();
            if (list.Count == 0) return NoContent();
            return Ok(list);
        }

        // GET /api/users/1
        [HttpGet("{id}")]
        public ActionResult<UserDTO> GetById(int id)
        {
            User user = AppUser.Find(id);
            if (user == null)
                return NotFound($"User with ID {id} not found!");
            return Ok(user.DTO);
        }

        // PUT /api/users/1
        [HttpPut("{id}")]
        public ActionResult<UserDTO> Update(int id, UserDTO dto)
        {
            User user = AppUser.Find(id);
            if (user == null)
                return NotFound($"User with ID {id} not found!");

            user.Name = dto.Name;
            user.Email = dto.Email;
            user.PreferredLanguage = dto.PreferredLanguage;
            user.ProfileImage = dto.ProfileImage;

            if (user.Save())
                return Ok(user.DTO);

            return StatusCode(500, "Failed to update user!");
        }

        // DELETE /api/users/1
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            User user = AppUser.Find(id);
            if (user == null)
                return NotFound($"User with ID {id} not found!");

            if (AppUser.Delete(id))
                return Ok($"User with ID {id} deleted successfully!");

            return StatusCode(500, "Failed to delete user!");
        }
    }
}
