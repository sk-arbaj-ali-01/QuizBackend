using Microsoft.Extensions.Logging;
using Quiz.BL.Abstractions;
using Quiz.DL.Abstractions;
using Quiz.Shared.DTO.Student.Response;

namespace Quiz.BL.Services;
public class StudentService(
    ILogger<StudentService> logger,
    IStudentRepository studentRepository)
    : BaseService, IStudentService
{
    public async Task<IEnumerable<GroupsForStudentsResponseDto>> GetGroupsFroStudents(Guid studentId)
    {
        return await studentRepository.GetGroupsFroStudents(studentId);
    }

    public async Task<IEnumerable<AttemptedQuizzesResponseDto>> GetAttemptedQuizzes(Guid studentId)
    {
        return await studentRepository.GetAttemptedQuizzes(studentId);
    }
}
