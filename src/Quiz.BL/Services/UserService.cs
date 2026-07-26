using Isopoh.Cryptography.Argon2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Quiz.BL.Abstractions;
using Quiz.BL.DtoToEntityExtensions.User;
using Quiz.DL.Abstractions;
using Quiz.DL.Entities;
using Quiz.DL.Parameters;
using Quiz.Shared.DTO.User.Request;
using Quiz.Shared.DTO.User.Response;
using Quiz.Shared.Exceptions;
using Quiz.Shared.Exceptions.DatabaseExceptions;
using Quiz.Shared.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Quiz.BL.Services;
public class UserService(
    ILogger<UserService> logger,
    IOptions<JWTOptions> jwtOption,
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

    public async Task<LoginDetails> Login(UserLoginRequestDto reqDto)
    {
        UserLoginEntity entity = reqDto.ConvertToEntity();
        UserLoginResponseDto? response = 
            await userRepository.Login(entity);

        if(response is null)
        {
            logger.LogInformation("User not found with the provided email : {email}", reqDto.Email);
            throw new RecordNotFoundException($"User not found with the provided email : {reqDto.Email}");
        }

        bool isPasswordMatched = Argon2.Verify(response.Password, entity.Password);

        if(!isPasswordMatched)
        {
            logger.LogInformation("Password not matched");
            throw new UnAuthenticatedException();
        }

        JWTOptions options = jwtOption.Value;

        byte[] key = Encoding.UTF8.GetBytes(options.Key);
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim> 
        {
            new Claim(JwtRegisteredClaimNames.Sub, response.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, response.EmailId),
            new Claim(ClaimTypes.Role, response.Role),
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            //Expires = DateTime.UtcNow.AddSeconds(30),
            Expires = DateTime.UtcNow.AddMinutes(options.Expiry),
            Issuer = options.Issuer,
            Audience = options.Audience,
            SigningCredentials = credentials,
        };

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

        JwtSecurityToken securityToken = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

        string token = tokenHandler.WriteToken(securityToken);

        return new LoginDetails
        {
            AccessToken = token,
            Role = response.Role,
            Expires = tokenDescriptor.Expires.Value
        };
    }

    public async Task<PagedRecordModel<UserTeacherResponseDto>> GetTeachersData(
        RelatedTeachersRequestDto requestDto,
        Guid userId)
    {
        RelatedTeachersParameter parameter = requestDto.ConvertToParameter();
        return await userRepository.GetTeachersData(parameter, userId);
    }

    public async Task CreateRelationBetweenStudentAndTeacher(Guid studentId, Guid teacherId)
    {
        bool exists = await userRepository.CheckIfStudentAndTeacherDataAlreadyExists(studentId, teacherId);

        if (exists)
        {
            logger.LogInformation("Student:{studentId} is already associated with Teacher:{teacherId}", studentId, teacherId);
            throw new RecordAlreadyExistsException("Student is already associated with teacher");
        }

        await userRepository.CreateRelationBetweenStudentAndTeacher(studentId, teacherId);
    }
}
