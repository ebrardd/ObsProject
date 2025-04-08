using Microsoft.AspNetCore.Mvc;
using Model.LoginRequest;
using ObsBackend.Data;

namespace ObsBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == request.Email && u.Password == request.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            return Ok(new { message = "Login successful", role = user.Role });
        }
    }
}
//iş