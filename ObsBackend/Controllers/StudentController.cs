using Microsoft.AspNetCore.Mvc;
using ObsBackend.Data;

namespace ObsBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("resit-exam-schedule/{courseCode}/download")]
        public IActionResult DownloadSchedule(string courseCode)
        {
            var exam = _context.ResitExams.FirstOrDefault(e => e.CourseCode == courseCode);

            if (exam == null || string.IsNullOrEmpty(exam.filePath))
                return NotFound(new { message = "Dosya bulunamadı." });

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedResitSchedules");
            var filePath = Path.Combine(uploadsFolder, exam.filePath);

            if (!System.IO.File.Exists(filePath))
                return NotFound(new { message = "Dosya sunucuda yok." });

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var contentType = "application/pdf";
            return File(fileBytes, contentType, Path.GetFileName(filePath));
        }

    }
}