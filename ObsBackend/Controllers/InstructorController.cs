using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ObsBackend.Data;

namespace ObsBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstructorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InstructorController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("courses/{instructorId}")]
        public IActionResult GetCourses(int instructorId)
        {
            var courses = _context.Courses
                .Include(c => c.Instructor)
                .Where(c => c.instructorId == instructorId)
                .Select(c => new
                {
                    c.Code,
                    c.Name,
                    c.instructorId,
                    Instructor = new
                    {
                        c.Instructor.Name,
                        c.Instructor.Surname
                    }
                })
                .ToList();

            return Ok(courses);
        }


    }

}