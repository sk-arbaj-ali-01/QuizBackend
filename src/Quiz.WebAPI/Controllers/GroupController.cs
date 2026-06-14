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
    [ProducesResponseType(StatusCodes.Status201Created)]
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
