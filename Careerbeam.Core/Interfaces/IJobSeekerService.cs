using Careerbeam.Core.DTOs;
using Microsoft.AspNetCore.Http;

namespace Careerbeam.Core.Interfaces;

public interface IJobSeekerService
{
    Task<JobSeekerProfileResponse> GetProfileAsync(int userId);
    Task<JobSeekerProfileResponse> UpdateProfileAsync(int userId, UpdateJobSeekerProfileRequest request);
    Task<string> UploadCvAsync(int userId, IFormFile file);
    Task<JobSeekerProfileResponse> AddSkillAsync(int userId, AddSkillRequest request);
    Task RemoveSkillAsync(int userId, int skillId);
}