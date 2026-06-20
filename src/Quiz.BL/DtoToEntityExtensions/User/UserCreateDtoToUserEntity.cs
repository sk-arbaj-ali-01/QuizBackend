using Isopoh.Cryptography.Argon2;
using Quiz.DL.Entities;
using Quiz.Shared.DTO.User.Request;
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
            Password = Argon2.Hash(requestDto.Password),
            Role = requestDto.Role.ToString(),
            CreatedAt = DateTime.UtcNow
        };

        return entity;
    }   

    public static UserLoginEntity ConvertToEntity(this UserLoginRequestDto requestDto)
    {
        UserLoginEntity entity = new()
        {
            Email = requestDto.Email,
            Password = requestDto.Password,
        };

        return entity;
    }
}
