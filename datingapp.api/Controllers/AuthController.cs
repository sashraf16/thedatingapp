using System.Threading.Tasks;
using datingapp.api.Data;
using datingapp.api.Models;
using Microsoft.AspNetCore.Mvc;

namespace datingapp.api.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthRepository _repo;
        public AuthController (IAuthRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(string username, string password)
        {
            //validate request

            username = username.ToLower();

            if(await _repo.UserExists(username))
                return BadRequest ("Username is already taken");

            var userToCreate = new user
            {
                username = username
            };

            var createUser = await _repo.Register(userToCreate, password);

            return StatusCode(201);
        }
    }
}