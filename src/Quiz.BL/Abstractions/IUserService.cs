using Quiz.Shared.Models;

namespace Quiz.BL.Abstractions;
public interface IUserService
{
    Task CreateUser(UserCreateRequestDto reqDto);

    Task<UserResponseDto?> GetUserById(Guid userId);
}
