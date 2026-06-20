using Quiz.DL.Entities;
using Quiz.Shared.DTO.User.Response;
using Quiz.Shared.Models;

namespace Quiz.DL.Abstractions;
public interface IUserRepository
{
    Task CreateUser(UserEntity entity);

    Task<UserResponseDto?> GetUserById(Guid userId);

    Task<UserLoginResponseDto?> Login(UserLoginEntity entity);
}
