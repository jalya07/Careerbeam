namespace Careerbeam.Core.Entities;

public class Vacancy
{
    public int Id { get; set; }
    public int EmployerProfileId { get; set; }
    public EmployerProfile EmployerProfile { get; set; } = null!;

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;

    public ICollection<VacancySkill> RequiredSkills { get; set; } = new List<VacancySkill>();
}