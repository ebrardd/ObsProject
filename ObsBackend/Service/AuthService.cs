
using ObsBackend.Model;
using ObsBackend.Data;

namespace ObsBackend.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public User? Authenticate(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user == null)
                return null;

            if (user.Role == "student")
            {
                var student = _context.Students.FirstOrDefault(s => s.Id == user.Id);
                if (student == null)
                    return null;
            }
            else if (user.Role == "instructor")
            {
                var instructor = _context.Instructors.FirstOrDefault(i => i.Id == user.Id);
                if (instructor == null)
                    return null;
            }
            else if (user.Role == "secretary")
            {
                var secretary = _context.Secretaries.FirstOrDefault(s => s.Id == user.Id);
                if (secretary == null)
                    return null;
            }

            return user;
        }

    }
}



