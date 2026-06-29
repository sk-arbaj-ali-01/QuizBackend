namespace Quiz.BL.Abstractions;
public interface IResultService
{
    Task<byte[]> GetExamReport(Guid userId, Guid groupId);
}
