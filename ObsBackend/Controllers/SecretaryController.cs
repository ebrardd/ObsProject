
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        
        [HttpGet("instructor/list")]
        public async Task<IActionResult> GetInstructorList( string secretaryId)
        {
            var instructors = await _context.Instructors
                .Select(i => new
                {
                    FullName = $"{i.Name} {i.Surname}",
                    Department = i.Department
                })
                .ToListAsync();

            return Ok(instructors);
        }
        
        
        [HttpPost("resitExam_schedule/{courseCode}/upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadResitExamSchedule([FromRoute] string courseCode, [FromForm] UploadGradeRequest request)
        {
            var file = request.File;

            if (file == null || file.Length == 0)
                
                return BadRequest(new { message = "Please upload a valid file." });

            var allowedExtensions = new[] { ".pdf", ".doc", ".docx" };
            
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                
                return BadRequest(new { message = "Invalid file format. Only PDF, DOC, and DOCX are allowed." });

            if (file.Length > 25 * 1024 * 1024)
                
                return BadRequest(new { message = "The file is too large. Max allowed size is 25 MB." });

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedResitSchedules");
            
            if (!Directory.Exists(uploadsFolder))
                
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{courseCode}_{Guid.NewGuid()}{extension}";
            
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
                
            {
                await file.CopyToAsync(stream);
            }
            var exam = await _context.ResitExams.FirstOrDefaultAsync(e => e.CourseCode == courseCode);
            
            if (exam == null)
                return NotFound(new { message = "Course not found in ResitExam." });

            exam.filePath = uniqueFileName;
            
            await _context.SaveChangesAsync();

            return Ok(new { message = "Resit exam schedule uploaded successfully." });
        }
        



    }
}
