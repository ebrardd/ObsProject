using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ObsBackend.Data;
using ObsBackend.Dto;
using ObsBackend.Model;

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


        [HttpGet("resitExam-schedule/{courseCode}/download")]
        public IActionResult Download(string courseCode)
        {
            var exam = _context.ResitExams.FirstOrDefault(e => e.CourseCode == courseCode);

            if (exam == null || string.IsNullOrEmpty(exam.filePath))
                return NotFound(new { message = "File not found." });

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploaded Resit Schedules");
            var filePath = Path.Combine(uploadsFolder, exam.filePath);

            if (!System.IO.File.Exists(filePath))
                return NotFound(new { message = "File does not exist on server" });

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var contentType = "application/pdf";
            return File(fileBytes, contentType, Path.GetFileName(filePath));
        }

        
        [HttpGet("courses")]
        public IActionResult Get([FromQuery] int instructorId)
        {
            var instructor = _context.Instructors
                .FirstOrDefault(i => i.Id == instructorId);

            if (instructor == null)
                return NotFound(new { message = "Instructor not found." });

            var courses = _context.Courses
                .Where(c => c.InstructorId == instructorId)
                .Select(c => new CourseDto
                {
                    Code = c.Code,
                    LectureName = c.LectureName,
                    InstructorFullName = instructor.Name + " " + instructor.Surname
                })
                .ToList();

            return Ok(courses);
        }

        
        [HttpGet("/{studentId}/grade status")]
        public IActionResult GetGradeStatus(int studentId)
        {
            var letterGrades = _context.LetterGrades
                .Where(lg => lg.StudentId == studentId)
                .ToList();

            if (!letterGrades.Any())
                return NotFound(new { message = "Note information is not found" });

            var result = new List<GradeEvaluationDto>();

            foreach (var lg in letterGrades)
            {
                string grade = lg.Grade?.ToUpper();
                string status;
                bool canTakeResit;

                if (grade == "AA" || grade == "BA" || grade == "BB" || grade == "CB" || grade == "CC")
                {
                    status = "Passed";
                    canTakeResit = false;
                }
                else if (grade == "DC" || grade == "DD" || grade == "FD" || grade == "FF")
                {
                    status = "Failed";
                    canTakeResit = true;
                }
                else
                {
                    status = "Unknown";
                    canTakeResit = false;
                }

                result.Add(new GradeEvaluationDto
                {
                    Course = lg.Course,
                    Grade = grade,
                    Status = status,
                    CanTakeResit = canTakeResit
                });
            }

            return Ok(result);
        }

        [HttpGet("resitExam Announcement/{examId}")]
        public IActionResult GetResitAnnouncementByExamId(int examId)
        {
            var exam = _context.ResitExams
                .Include(e => e.Announcement)
                .FirstOrDefault(e => e.Id == examId); // <-- Artık Id'ye göre arıyoruz

            if (exam == null || exam.Announcement == null)
                return NotFound(new { message = "Resit exam or announcement not found." });

            return Ok(new ExamAnnouncementDto
            {
                ExamId = exam.Id,
                Message = exam.Announcement.Message
            });
        }
        
        
        [HttpGet("{studentId}/grades")]
        public IActionResult GetStudentGrades(int studentId)
        {
            var grades = _context.LetterGrades
                .Where(g => g.StudentId == studentId)
                .ToList();

            if (!grades.Any())
                return NotFound(new { message = "No grades found for this student." });

            var result = grades.Select(g =>
            {
                var course = _context.Courses.FirstOrDefault(c => c.Code == g.Course);
                string status;

                switch (g.Grade.ToUpper())
                {
                    case "AA":
                    case "BA":
                    case "BB":
                    case "CB":
                    case "CC":
                        status = "Passed";
                        break;

                    case "DC":
                    case "DD":
                        status = "Passed (Resit Available)";
                        break;

                    case "FD":
                    case "FF":
                    case "DZ":
                        status = "Failed";
                        break;

                    default:
                        status = "Unknown";
                        break;
                }

                return new
                {
                    courseCode = g.Course,
                    courseName = course?.LectureName ?? "-",
                    grade = g.Grade,
                    status
                };
            });

            return Ok(result);
        }
        [HttpGet("{studentId}/resit_exams")]
        public IActionResult GetStudentResitExams(int studentId)
        {
            var studentCourses = _context.LetterGrades
                .Where(lg => lg.StudentId == studentId)
                .Select(lg => lg.Course)
                .ToList();

            var resitExams = _context.ResitExams
                .Where(re => studentCourses.Contains(re.CourseCode) && re.ResitDate != null && re.ResitTime != null)
                .ToList();

            var result = resitExams.Select(re =>
            {
                var course = _context.Courses.FirstOrDefault(c => c.Code == re.CourseCode);
                return new
                {
                    courseCode = re.CourseCode,
                    courseName = course?.LectureName ?? "-",
                    resitDate = re.ResitDate?.ToString(),
                    resitTime = re.ResitTime,
                    location = re.Location
                };
            });

            return Ok(result);
        }
        [HttpGet("{studentId}/new-grades")]
        public IActionResult GetNewLetterGrades(int studentId)
        {
            var grades = _context.LetterGrades
                .Where(lg => lg.StudentId == studentId)
                .Join(_context.Courses,
                    lg => lg.Course,
                    c => c.Code,
                    (lg, c) => new
                    {
                        CourseCode = c.Code,
                        CourseName = c.LectureName,
                        Grade = lg.Grade.ToUpper(),
                        Status = GetStatus(lg.Grade)
                    })
                .ToList();

            return Ok(grades);
        }

        private string GetStatus(string grade)
        {
            var upper = grade.ToUpper();
            return upper switch
            {
                "AA" or "BA" or "BB" or "CB" or "CC" => "Passed",
                "DC" or "DD" => "Passed (Resit Available)",
                "FD" or "FF" or "DZ" => "Failed",
                _ => "Unknown"
            };
        }

        [HttpGet("grade-download/{courseCode}")]
        public IActionResult DownloadGradeFile(string courseCode)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedGrades");
            var filePattern = $"{courseCode}_*"; // örnek: SE101_XXX.docx gibi

            var file = Directory.GetFiles(uploadsFolder)
                .FirstOrDefault(f => Path.GetFileName(f).StartsWith(courseCode));

            if (file == null)
                return NotFound(new { message = "Grade file not found for this course." });

            var fileBytes = System.IO.File.ReadAllBytes(file);
            var contentType = "application/octet-stream";
            return File(fileBytes, contentType, Path.GetFileName(file));
        } 
    }
}

    


  