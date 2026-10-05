using Careerbeam.Core.DTOs;
using Careerbeam.Core.Entities;
using Careerbeam.Core.Interfaces;
using Careerbeam.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Careerbeam.Infrastructure.Services;

public class JobSeekerService : IJobSeekerService
{
    private readonly CareerbeamDbContext _context;
    private readonly IFileStorageService _fileStorage;

    public JobSeekerService(CareerbeamDbContext context, IFileStorageService fileStorage)
    {
        _context = context;
        _fileStorage = fileStorage;
    }

    public async Task<JobSeekerProfileResponse> GetProfileAsync(int userId)
    {
        var profile = await GetProfileEntityAsync(userId);
        return MapToResponse(profile);
    }

    public async Task<JobSeekerProfileResponse> UpdateProfileAsync(int userId, UpdateJobSeekerProfileRequest request)
    {
        var profile = await GetProfileEntityAsync(userId);
        profile.FullName = request.FullName;
        await _context.SaveChangesAsync();
        return MapToResponse(profile);
    }

    public async Task<string> UploadCvAsync(int userId, IFormFile file)
    {
        var profile = await GetProfileEntityAsync(userId);

        // Если уже был загружен CV — удаляем старый файл, чтобы не копить мусор
        if (!string.IsNullOrEmpty(profile.CvFileUrl))
            _fileStorage.DeleteFile(profile.CvFileUrl);

        var fileUrl = await _fileStorage.SaveFileAsync(file, "cv");
        profile.CvFileUrl = fileUrl;
        await _context.SaveChangesAsync();

        return fileUrl;
    }

    public async Task<JobSeekerProfileResponse> AddSkillAsync(int userId, AddSkillRequest request)
    {
        var profile = await GetProfileEntityAsync(userId);

        // Ищем навык в справочнике без учёта регистра (чтобы "C#" и "c#" не создавались как разные)
        var skill = await _context.Skills
            .FirstOrDefaultAsync(s => s.Name.ToLower() == request.SkillName.ToLower());

        if (skill == null)
        {
            skill = new Skill { Name = request.SkillName };
            _context.Skills.Add(skill);
            await _context.SaveChangesAsync(); // сохраняем, чтобы получить skill.Id
        }

        var alreadyHasSkill = await _context.JobSeekerSkills
            .AnyAsync(js => js.JobSeekerProfileId == profile.Id && js.SkillId == skill.Id);

        if (!alreadyHasSkill)
        {
            _context.JobSeekerSkills.Add(new JobSeekerSkill
            {
                JobSeekerProfileId = profile.Id,
                SkillId = skill.Id,
                Level = request.Level
            });
            await _context.SaveChangesAsync();
        }

        var updatedProfile = await GetProfileEntityAsync(userId);
        return MapToResponse(updatedProfile);
    }

    public async Task RemoveSkillAsync(int userId, int skillId)
    {
        var profile = await GetProfileEntityAsync(userId);

        var link = await _context.JobSeekerSkills
            .FirstOrDefaultAsync(js => js.JobSeekerProfileId == profile.Id && js.SkillId == skillId);

        if (link != null)
        {
            _context.JobSeekerSkills.Remove(link);
            await _context.SaveChangesAsync();
        }
    }

    // Вспомогательный метод — получить профиль со всеми связанными навыками
    private async Task<JobSeekerProfile> GetProfileEntityAsync(int userId)
    {
        var profile = await _context.JobSeekerProfiles
            .Include(p => p.Skills)
                .ThenInclude(js => js.Skill)
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (profile == null)
            throw new KeyNotFoundException("Профиль соискателя не найден.");

        return profile;
    }

    private static JobSeekerProfileResponse MapToResponse(JobSeekerProfile profile)
    {
        return new JobSeekerProfileResponse
        {
            Id = profile.Id,
            FullName = profile.FullName,
            CvFileUrl = profile.CvFileUrl,
            Skills = profile.Skills.Select(js => new SkillDto
            {
                Id = js.SkillId,
                Name = js.Skill.Name,
                Level = js.Level
            }).ToList()
        };
    }
}