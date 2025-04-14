namespace ObsBackend.Dto;

public class GradeEvaluationDto
{
    public string Course { get; set; }
    public string Grade { get; set; }
    public string Status { get; set; }
    public bool CanTakeResit { get; set; }
}
