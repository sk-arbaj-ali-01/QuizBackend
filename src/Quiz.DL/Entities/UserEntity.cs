using System;
using Quiz.Shared.Enums;

namespace Quiz.DL.Entities;

public class UserEntity : BaseEntity
{
    public Guid UserId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string EmailId { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;
}
