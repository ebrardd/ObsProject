using Microsoft.AspNetCore.Mvc;
using Model.LoginRequest;
using ObsBackend.Service;


namespace ObsBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthenticationController(IAuthService authService)
        {
            _authService = authService;
        }
      /* [HttpGet]
        public IActionResult Index()
        {
            return View(); 
        } */
      
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _authService.Authenticate(request.Email, request.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            return Ok(new
            {
                message = "Login successful",
                role = user.Role,
                userId = user.Id,
                redirect = user.Role switch
                {
                    "student" => "/student/home",
                    "instructor" => "/instructor/home",
                    "secretary" => "/secretary/home",
                    _ => "/"
                }
            }); 
  
      
     /* [HttpPost("login")]
      public IActionResult Login([FromBody] LoginRequest request)
      {
          if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
          {
              return BadRequest(new { message = "Email and password are required." });
          }

          var user = _authService.Authenticate(request.Email, request.Password);

          if (user == null)
          {
              return Unauthorized(new { message = "Invalid email or password." });
          }
          
          string redirectUrl = user.Role switch
          {
              "student" => "/student/home.html",
              "instructor" => "/instructor/home.html",
              "secretary" => "/secretary/home", 
              _ => "/"
          };


          return Ok(new
          {
              message = "Login successful",
              role = user.Role,
              userId = user.Id,
              redirect = redirectUrl
          }); */

      }



        }
    }
