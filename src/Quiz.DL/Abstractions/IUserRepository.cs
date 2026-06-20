using Quiz.Shared.Models;
using Quiz.DL.Entities;

namespace Quiz.DL.Abstractions;
public interface IUserRepository
{
    Task CreateUser(UserEntity entity);

    Task<UserResponseDto?> GetUserById(Guid userId);
}
