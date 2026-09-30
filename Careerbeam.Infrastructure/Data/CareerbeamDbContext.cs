using Careerbeam.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Careerbeam.Infrastructure.Data;

public class CareerbeamDbContext : DbContext
{
    public CareerbeamDbContext(DbContextOptions<CareerbeamDbContext> options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<JobSeekerProfile> JobSeekerProfiles => Set<JobSeekerProfile>();
    public DbSet<EmployerProfile> EmployerProfiles => Set<EmployerProfile>();
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<JobSeekerSkill> JobSeekerSkills => Set<JobSeekerSkill>();
    public DbSet<Vacancy> Vacancies => Set<Vacancy>();
    public DbSet<VacancySkill> VacancySkills => Set<VacancySkill>();
    public DbSet<News> News => Set<News>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<JobSeekerSkill>()
            .HasKey(js => new { js.JobSeekerProfileId, js.SkillId });

        modelBuilder.Entity<VacancySkill>()
            .HasKey(vs => new { vs.VacancyId, vs.SkillId });

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Skill>()
            .HasIndex(s => s.Name)
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }
}