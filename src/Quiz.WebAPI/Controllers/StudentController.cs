using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quiz.BL.Abstractions;
using Quiz.BL.Services;
using Quiz.Shared.DTO.Question.Request;
using Quiz.Shared.DTO.Student.Response;
using Quiz.Shared.Helpers;

namespace Quiz.WebAPI.Controllers;

[ApiController]
[Route("api/v1/students")]
[Authorize(Policy = "Student")]
public class StudentController(
    IStudentService studentService,
    IQuestionService questionService)
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

    [HttpPost("submit-answers")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> SubmitAnswers(
        [FromBody] QuestionSubmitAnswersRequestDto requestDto)
    {
        Guid userId = User.GetUserIdFromClaims();

        await questionService.SubmitAnswers(requestDto, userId);

        return NoContent();
    }

    [HttpGet("attempted-quizzes")]
    [ProducesResponseType<IEnumerable<AttemptedQuizzesResponseDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAttemptedQuizzes()
    {
        Guid userId = User.GetUserIdFromClaims();

        IEnumerable<AttemptedQuizzesResponseDto> responses =
            await studentService.GetAttemptedQuizzes(userId);

        return Ok(responses);
    }
}
