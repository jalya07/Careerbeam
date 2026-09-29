namespace Careerbeam.Core.Entities;

public enum SkillLevel
{
    Beginner,
    Intermediate,
    Advanced
}

public class JobSeekerSkill
{
    public int JobSeekerProfileId { get; set; }
    public JobSeekerProfile JobSeekerProfile { get; set; } = null!;

    public int SkillId { get; set; }
    public Skill Skill { get; set; } = null!;

    public SkillLevel Level { get; set; } = SkillLevel.Beginner;
}