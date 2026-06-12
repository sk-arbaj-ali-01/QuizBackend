using Microsoft.AspNetCore.Mvc;
using Quiz.BL.Abstractions;
using Quiz.Shared.DTO.Group.Request;

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
}
