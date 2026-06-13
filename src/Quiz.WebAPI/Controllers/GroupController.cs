using Microsoft.AspNetCore.Mvc;
using Quiz.BL.Abstractions;
using Quiz.Shared.DTO.Group.Request;
using Quiz.Shared.DTO.Group.Response;
using Quiz.Shared.Models;

namespace Quiz.WebAPI.Controllers;

[ApiController]
[Route("v1/groups")]
public class GroupController(
    IGroupService groupService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateGroup([FromBody] GroupCreateRequestDto requestDto)
    {
        await groupService.CreateGroup(requestDto);

        return Ok();
    }

    [HttpGet]
    [ProducesResponseType<PaginatedResponse<GroupResponseDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGroups([FromQuery] GroupRequestDto requestDto)
    {
        PaginatedResponse<GroupResponseDto> response = 
            await groupService.GetGroups(requestDto);

        return Ok(response);
    }
}
