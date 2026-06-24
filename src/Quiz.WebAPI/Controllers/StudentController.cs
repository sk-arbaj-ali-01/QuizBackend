using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quiz.BL.Abstractions;
using Quiz.Shared.DTO.Student.Response;
using Quiz.Shared.Helpers;

namespace Quiz.WebAPI.Controllers;

[ApiController]
[Route("api/v1/students")]
[Authorize(Policy = "Student")]
public class StudentController(
    IStudentService studentService)
    : ControllerBase
{
    
    [HttpGet("dashboard/groups")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGroupsForStudents()
    {
        Guid userId = User.GetUserIdFromClaims();

        IEnumerable<GroupsForStudentsResponseDto> responses =
            await studentService.GetGroupsFroStudents(userId);

        return Ok(responses);
    }
}
