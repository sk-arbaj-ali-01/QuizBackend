using Quiz.DL.Entities;
using Quiz.Shared.Enums;
using Quiz.Shared.Models;

namespace Quiz.BL.DtoToEntityExtensions.User;
public static class UserEntityToUserResponse
{
    public static UserResponseDto ConvertToResponse(this UserEntity entity)
    {
        RoleEnum role = Enum.TryParse<RoleEnum>(entity.Role, true, out RoleEnum parsedRole)
            ? parsedRole
            : RoleEnum.STUDENT;

        UserResponseDto response = new()
        {
            UserId = entity.UserId,
            FullName = entity.FullName,
            EmailId = entity.EmailId,
            Role = role,
            CreatedAt = entity.CreatedAt,
            CreatedBy = entity.CreatedBy,
            ModifiedAt = entity.ModifiedAt,
            ModifiedBy = entity.ModifiedBy
        };

        return response;
    }
}
