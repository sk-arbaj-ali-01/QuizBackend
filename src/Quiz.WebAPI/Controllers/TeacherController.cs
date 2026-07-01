using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quiz.BL.Abstractions;
using Quiz.Shared.DTO.Teacher.Request;
using Quiz.Shared.DTO.Teacher.Response;
using Quiz.Shared.Helpers;

namespace Quiz.WebAPI.Controllers;

[ApiController]
[Route("api/v1/teachers")]
[Authorize(Policy = "Teacher")]
public class TeacherController(
    ITeacherService teacherService) : ControllerBase
{
    [HttpGet("review")]
    [ProducesResponseType<ExamReviewResponseDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetExamsToBeReviewed()
    {
        Guid userId = User.GetUserIdFromClaims();

        var response = await teacherService.GetExamsToBeReviewed(userId);

        return Ok(response);
    }

    [HttpGet("group/{groupId:guid}/student/{studentId:guid}")]
    [ProducesResponseType<IEnumerable<ShortAnswerQuestionsResponseDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetShortAnswerQuestionsForReview(
        Guid groupId,
        Guid studentId)
    {
        Guid userId = User.GetUserIdFromClaims();

        var response = await teacherService.GetShortAnswerQuestionsForReview(groupId, studentId);

        return Ok(response);
    }

    [HttpPut("submit-result")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SubmitReviewResult([FromBody] ReviewResultRequestDto reqDto)
    {
        Guid userId = User.GetUserIdFromClaims();

        await teacherService.SubmitReviewResult(reqDto, userId);

        return Ok();
    }
}
