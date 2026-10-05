using System.Security.Claims;
using Careerbeam.Core.DTOs;
using Careerbeam.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Careerbeam.API.Controllers;

[ApiController]
[Route("api/jobseeker")]
[Authorize(Roles = "JobSeeker")]
public class JobSeekerController : ControllerBase
{
    private readonly IJobSeekerService _jobSeekerService;

    public JobSeekerController(IJobSeekerService jobSeekerService)
    {
        _jobSeekerService = jobSeekerService;
    }

    private int CurrentUserId =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var result = await _jobSeekerService.GetProfileAsync(CurrentUserId);
        return Ok(result);
    }

    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile(UpdateJobSeekerProfileRequest request)
    {
        var result = await _jobSeekerService.UpdateProfileAsync(CurrentUserId, request);
        return Ok(result);
    }

    [HttpPost("cv")]
    public async Task<IActionResult> UploadCv(IFormFile file)
    {
        if (file.Length == 0)
            return BadRequest(new { message = "Файл пустой или не прикреплён." });

        var fileUrl = await _jobSeekerService.UploadCvAsync(CurrentUserId, file);
        return Ok(new { cvFileUrl = fileUrl });
    }

    [HttpPost("skills")]
    public async Task<IActionResult> AddSkill(AddSkillRequest request)
    {
        var result = await _jobSeekerService.AddSkillAsync(CurrentUserId, request);
        return Ok(result);
    }

    [HttpDelete("skills/{skillId}")]
    public async Task<IActionResult> RemoveSkill(int skillId)
    {
        await _jobSeekerService.RemoveSkillAsync(CurrentUserId, skillId);
        return NoContent();
    }
}