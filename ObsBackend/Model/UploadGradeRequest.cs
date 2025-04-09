
using Microsoft.AspNetCore.Http;

namespace ObsBackend.Model
{
    public class UploadGradeRequest
    {
        public IFormFile File { get; set; }
    }
}

