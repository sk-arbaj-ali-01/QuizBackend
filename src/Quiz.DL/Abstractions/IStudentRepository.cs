using Quiz.Shared.DTO.Student.Response;

namespace Quiz.DL.Abstractions;
public interface IStudentRepository
{
    Task<IEnumerable<GroupsForStudentsResponseDto>> GetGroupsFroStudents(Guid studentId);
}
