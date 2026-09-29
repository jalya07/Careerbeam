namespace Careerbeam.Core.Entities;

public class EmployerProfile
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public string CompanyName { get; set; } = string.Empty;
    public string? CompanyDescription { get; set; }

    public ICollection<Vacancy> Vacancies { get; set; } = new List<Vacancy>();
}