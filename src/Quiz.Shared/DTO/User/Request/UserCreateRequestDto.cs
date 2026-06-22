using Quiz.Shared.Enums;

namespace Quiz.Shared.DTO.User.Request;

public class UserCreateRequestDto
{
    public string FullName { get; set; } = string.Empty;

    public string EmailId { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public RoleEnum Role { get; set; }
}