using Quiz.Shared.DTO.User.Request;
using Quiz.Shared.DTO.User.Response;
using Quiz.Shared.Models;

namespace Quiz.BL.Abstractions;
public interface IUserService
{
    Task CreateUser(UserCreateRequestDto reqDto);

    Task<UserResponseDto?> GetUserById(Guid userId);

    Task<LoginDetails> Login(UserLoginRequestDto reqDto);

    Task<PagedRecordModel<UserTeacherResponseDto>> GetTeachersData();

    Task CreateRelationBetweenStudentAndTeacher(Guid studentId, Guid teacherId);
}
