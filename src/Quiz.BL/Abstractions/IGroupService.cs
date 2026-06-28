using Quiz.Shared.DTO.Group.Request;
using Quiz.Shared.DTO.Group.Response;
using Quiz.Shared.Models;

namespace Quiz.BL.Abstractions;
public interface IGroupService
{
    Task CreateGroup(GroupCreateRequestDto reqDto, string userId);

    Task<PaginatedResponse<GroupResponseDto>> GetGroups(GroupRequestDto reqDto, Guid userId);

    Task<GroupResponseDto> GetGroupById(Guid groupId);

    Task UpdateGroupById(Guid groupId, GroupUpdateRequestDto reqDto);

    Task DeleteGroupById(Guid groupId);
}
