using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ObsBackend.Data;
using ObsBackend.Dto;
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
                .Where(c => c.InstructorId == instructorId)
                .Select(c => new
                {
                    c.Code,
                    c.LectureName,
                    c.InstructorId,
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
        [HttpGet("resitexams/{instructorId}")]
        public IActionResult Get(int instructorId)
        {
            var resitExams = _context.ResitExams
                .Include(r => r.Course)
                .Include(r => r.Instructor)
                .Where(r => r.LecturerId == instructorId)
                .ToList();

            return Ok(resitExams);
        }
        [HttpPost("examAnnouncement/{examId}")]
        public async Task<IActionResult> CreateAnnouncement(int examId, [FromBody] ExamAnnouncementDto dto)
        {
            var announcement = new ExamAnnouncement
            {
                ExamId = examId,
                Message = dto.Message
            };

            _context.ExamAnnouncements.Add(announcement);
            await _context.SaveChangesAsync();

            return Ok(announcement);
        }
        [HttpPut("examAnnouncement/{id}")]
        public async Task<IActionResult> UpdateAnnouncement(int id, [FromBody] ExamAnnouncementDto dto)
        {
            var existing = await _context.ExamAnnouncements.FindAsync(id);
            if (existing == null)
                return NotFound();

            existing.Message = dto.Message;
            existing.ExamId = dto.ExamId;

            await _context.SaveChangesAsync();

            return Ok(existing);
        }
        [HttpDelete("examAnnouncement/{id}")]
        public async Task<IActionResult> DeleteAnnouncement(int id)
        {
            var existing = await _context.ExamAnnouncements.FindAsync(id);
            if (existing == null)
                return NotFound();

            _context.ExamAnnouncements.Remove(existing);
            await _context.SaveChangesAsync();

            return NoContent(); // 204
        }
        [HttpGet("classlist/{courseCode}")]
        public async Task<IActionResult> GetResitClassList(string courseCode)
        {
            var trimmedCode = courseCode.Trim();

            var allGrades = await _context.LetterGrades
                .Where(g => g.Course == trimmedCode)
                .ToListAsync();

            var studentIds = allGrades.Select(g => g.StudentId).ToList();

            var students = await _context.Students
                .Where(s => studentIds.Contains(s.Id))
                .ToListAsync();

            var resitStudents = new List<ExamClassListItem>();

            foreach (var grade in allGrades)
            {
                var student = students.FirstOrDefault(s => s.Id == grade.StudentId);
                if (student == null) continue;

                var letter = grade.Grade.ToUpper();

                if (letter == "FD" || letter == "FF" || letter == "NA")
                {
                    resitStudents.Add(new ExamClassListItem
                    {
                        StudentNumber = student.Id.ToString(),
                        FullName = student.Name + " " + student.Surname,
                        Department = student.Department
                    });
                }
            }

            return Ok(resitStudents);
        }





        }


  

    }
        
    

