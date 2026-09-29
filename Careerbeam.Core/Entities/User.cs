namespace Careerbeam.Core.Entities;

public enum UserRole
{
    JobSeeker,
    Employer,
    Admin
}

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Навигационные свойства (связь один-к-одному с профилями)
    public JobSeekerProfile? JobSeekerProfile { get; set; }
    public EmployerProfile? EmployerProfile { get; set; }
}