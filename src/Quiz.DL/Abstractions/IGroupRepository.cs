using Quiz.DL.Entities;
using Quiz.DL.Parameters;
using Quiz.Shared.DTO.Group.Response;
using Quiz.Shared.DTO.Student.Response;
using Quiz.Shared.Models;

namespace Quiz.DL.Abstractions;
public interface IGroupRepository
{
    Task CreateGroup(GroupEntity entity);

    Task<PagedRecordModel<GroupResponseDto>> GetGroups(GroupParameter parameter, Guid userId);

    Task<GroupResponseDto> GetGroupById(Guid groupId);

    Task UpdateGroupById(GroupEntity groupEntity);

    Task DeleteGroupById(Guid groupId);
}
