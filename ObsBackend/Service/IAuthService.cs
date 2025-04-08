using ObsBackend.Model;

namespace ObsBackend.Service;

public interface IAuthService
{
    User? Authenticate(string email, string password);
}