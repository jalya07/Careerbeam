namespace Careerbeam.Core.Entities;

public class JobSeekerProfile
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public string FullName { get; set; } = string.Empty;
    public string? CvFileUrl { get; set; } // путь к загруженному CV

    // Навыки, которыми владеет (через промежуточную сущность)
    public ICollection<JobSeekerSkill> Skills { get; set; } = new List<JobSeekerSkill>();
}