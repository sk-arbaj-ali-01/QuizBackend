using Microsoft.AspNetCore.Mvc;
using Quiz.BL.Abstractions;
using Quiz.Shared.Models;

namespace Quiz.WebAPI.Controllers;
[Route("api/users")]
[ApiController]
public class UserController(
    IUserService userService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateUser([FromBody] UserCreateRequestDto requestDto)
    {
        await userService.CreateUser(requestDto);

        return Created();
    }

    [HttpGet("{userId:guid}")]
    [ProducesResponseType<UserResponseDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById([FromRoute] Guid userId)
    {
        UserResponseDto? response = await userService.GetUserById(userId);

        return Ok(response);
    }
}
