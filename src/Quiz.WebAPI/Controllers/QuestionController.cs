using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quiz.BL.Abstractions;
using Quiz.Shared.DTO.Question.Request;
using Quiz.Shared.DTO.Question.Response;
using Quiz.Shared.Helpers;

namespace Quiz.WebAPI.Controllers;

[ApiController]
[Route("api/v1/questions")]
[Authorize]
public class QuestionController(
    IQuestionService questionService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateQuestions([FromBody] QuestionCreateRequestDto questionCreateRequestDto)
    {
        await questionService.CreateQuestions(questionCreateRequestDto);

        return Created();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateQuestions([FromBody] QuestionUpdateRequestDto questionUpdateRequestDto)
    {
        await questionService.UpdateQuestions(questionUpdateRequestDto);

        return NoContent();
    }

    [HttpGet("{groupId:guid}")]
    [ProducesResponseType<QuestionResponseDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetQuestions([FromRoute] Guid groupId)
    {
        return Ok(await questionService.GetQuestions(groupId));
    }
}
