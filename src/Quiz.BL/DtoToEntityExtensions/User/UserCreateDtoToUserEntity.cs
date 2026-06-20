using Quiz.DL.Entities;
using Quiz.Shared.Models;

namespace Quiz.BL.DtoToEntityExtensions.User;
public static class UserCreateDtoToUserEntity
{
    public static UserEntity ConvertToEntity(this UserCreateRequestDto requestDto)
    {
        UserEntity entity = new()
        {
            UserId = Guid.NewGuid(),
            FullName = requestDto.FullName,
            EmailId = requestDto.EmailId,
            Password = requestDto.Password,
            Role = requestDto.Role.ToString(),
            CreatedAt = DateTime.UtcNow
        };

        return entity;
    }   
}
