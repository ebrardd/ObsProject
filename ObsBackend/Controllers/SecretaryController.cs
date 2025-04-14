
using Microsoft.AspNetCore.Mvc;
using ObsBackend.Data;
using ObsBackend.Model;

namespace ObsBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecretaryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SecretaryController(AppDbContext context)
        {
            _context = context;
        }

   
    }
}
