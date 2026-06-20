using Quiz.Shared.Enums;

namespace Quiz.Shared.Models;

public class UserResponseDto
{
    public Guid UserId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string EmailId { get; set; } = string.Empty;

    public RoleEnum Role { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public Guid? ModifiedBy { get; set; }
}