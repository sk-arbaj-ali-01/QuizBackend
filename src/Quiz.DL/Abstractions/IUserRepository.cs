using Quiz.DL.Entities;
using Quiz.DL.Parameters;
using Quiz.Shared.DTO.User.Response;
using Quiz.Shared.Models;

namespace Quiz.DL.Abstractions;
public interface IUserRepository
{
    Task CreateUser(UserEntity entity);

    Task<UserResponseDto?> GetUserById(Guid userId);

    Task<UserLoginResponseDto?> Login(UserLoginEntity entity);

    Task<PagedRecordModel<UserTeacherResponseDto>> GetTeachersData(
        RelatedTeachersParameter parameter,
        Guid userId);

    Task CreateRelationBetweenStudentAndTeacher(Guid studentId, Guid teacherId);

    Task<bool> CheckIfStudentAndTeacherDataAlreadyExists(Guid studentId, Guid teacherId);
}
