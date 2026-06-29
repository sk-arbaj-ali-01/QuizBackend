using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quiz.BL.Abstractions;
using Quiz.Shared.DTO.Group.Request;
using Quiz.Shared.DTO.Group.Response;
using Quiz.Shared.Helpers;
using Quiz.Shared.Models;
using System.Security.Claims;

namespace Quiz.WebAPI.Controllers;

[ApiController]
[Route("api/v1/groups")]
[Authorize]
[Authorize(Policy = "Teacher")]
public class GroupController(
    IGroupService groupService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateGroup([FromBody] GroupCreateRequestDto requestDto)
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if(userId == null)
        {
            return BadRequest();
        }

        await groupService.CreateGroup(requestDto, userId);

        return Ok();
    }

    [HttpGet]
    [ProducesResponseType<PaginatedResponse<GroupResponseDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGroups([FromQuery] GroupRequestDto requestDto)
    {
        Guid userId = User.GetUserIdFromClaims();

        PaginatedResponse<GroupResponseDto> response = 
            await groupService.GetGroups(requestDto, userId);

        return Ok(response);
    }

    [HttpGet("{groupId:Guid}")]
    [ProducesResponseType<GroupResponseDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGroupById([FromRoute] Guid groupId)
    {
        GroupResponseDto response =
            await groupService.GetGroupById(groupId);

        return Ok(response);
    }

    [HttpPut("{groupId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateGroupById([FromRoute] Guid groupId, [FromBody] GroupUpdateRequestDto requestDto) 
    { 
        await groupService.UpdateGroupById(groupId, requestDto); 

        return Ok();
    }

    [HttpDelete("{groupId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteGroupById([FromRoute] Guid groupId)
    {
        await groupService.DeleteGroupById(groupId);

        return Ok();
    }
}
