using System.ComponentModel.DataAnnotations;

namespace ObsBackend.Model;

  public class UploadResitExam
{
    [Required]
    public IFormFile File { get; set; } = null!;
}
