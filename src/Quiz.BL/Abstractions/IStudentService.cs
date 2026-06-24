using Quiz.Shared.DTO.Student.Response;

namespace Quiz.BL.Abstractions;
public interface IStudentService
{
    Task<IEnumerable<GroupsForStudentsResponseDto>> GetGroupsFroStudents(Guid studentId);

}
