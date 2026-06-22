using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quiz.BL.Abstractions;
using Quiz.Shared.DTO.User.Request;
using Quiz.Shared.DTO.User.Response;
using Quiz.Shared.Models;
using System.Security.Claims;

namespace Quiz.WebAPI.Controllers;
[Route("api/v1/users")]
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

    [HttpPost("login")]
    [ProducesResponseType<LoginDetails>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Login([FromBody] UserLoginRequestDto reqDto)
    {
        LoginDetails? response = await userService.Login(reqDto);

        return Ok(response);
    }

    [Authorize]
    [HttpGet("teachers")]
    [ProducesResponseType<PagedRecordModel<UserTeacherResponseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTeachersData()
    {
        PagedRecordModel<UserTeacherResponseDto> pagedRecords =
            await userService.GetTeachersData();

        return Ok(pagedRecords);
    }

    [Authorize]
    [HttpPost("teachers")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateRelationBetweenStudentAndTeacher([FromBody] StudentTeacherCreateRequestDto reqDto)
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
        {
            return BadRequest();
        }

        await userService.CreateRelationBetweenStudentAndTeacher(
            Guid.Parse(userId),
            reqDto.TeacherId);

        return Created();
    }
}
