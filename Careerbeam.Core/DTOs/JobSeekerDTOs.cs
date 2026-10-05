using Careerbeam.Core.Entities;

namespace Careerbeam.Core.DTOs;

public class JobSeekerProfileResponse
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? CvFileUrl { get; set; }
    public List<SkillDto> Skills { get; set; } = new();
}

public class SkillDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public SkillLevel Level { get; set; }
}

public class UpdateJobSeekerProfileRequest
{
    public string FullName { get; set; } = string.Empty;
}

public class AddSkillRequest
{
    public string SkillName { get; set; } = string.Empty;
    public SkillLevel Level { get; set; } = SkillLevel.Beginner;
}