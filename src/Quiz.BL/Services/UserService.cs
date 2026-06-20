using Microsoft.Extensions.Logging;
using Quiz.BL.Abstractions;
using Quiz.BL.DtoToEntityExtensions.User;
using Quiz.DL.Abstractions;
using Quiz.DL.Entities;
using Quiz.Shared.Exceptions.DatabaseExceptions;
using Quiz.Shared.Models;

namespace Quiz.BL.Services;
public class UserService(
    ILogger<UserService> logger,
    IUserRepository userRepository)
    : BaseService, IUserService
{
    public async Task CreateUser(UserCreateRequestDto reqDto)
    {
        UserEntity entity = reqDto.ConvertToEntity();

        await userRepository.CreateUser(entity);

        logger.LogInformation("User create request mapped successfully for {EmailId}", reqDto.EmailId);
    }

    public async Task<UserResponseDto?> GetUserById(Guid userId)
    {
        UserResponseDto? response = await userRepository.GetUserById(userId);

        if(response is null)
        {
            logger.LogInformation("User not found with the provided id : {userId}", userId);
            throw new RecordNotFoundException($"User not found with the provided id : {userId}");
        }

        return response;
    }
}
