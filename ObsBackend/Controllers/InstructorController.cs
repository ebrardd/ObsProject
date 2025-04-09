using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ObsBackend.Data;
using Microsoft.AspNetCore.Http;
using ObsBackend.Model;

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
        [HttpPost("courses/{code}/upload-grade")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadGradeFile([FromRoute] string code, [FromForm] UploadGradeRequest request)
        {
            var file = request.File;

            if (file == null || file.Length == 0)
                return BadRequest("The uploaded file is empty.");

            var allowedExtensions = new[] { ".pdf", ".doc", ".docx" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                return BadRequest("Invalid file type. Only PDF, DOC, and DOCX files are allowed.");

            if (file.Length > 25 * 1024 * 1024)
                return BadRequest("File size cannot exceed 25MB.");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedGrades");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, $"{code}_{file.FileName}");

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { message = "File uploaded successfully." });
        }




    }

}