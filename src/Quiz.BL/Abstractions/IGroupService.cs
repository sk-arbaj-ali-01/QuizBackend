using Quiz.Shared.DTO.Group.Request;
using Quiz.Shared.DTO.Group.Response;
using Quiz.Shared.Models;

namespace Quiz.BL.Abstractions;
public interface IGroupService
{
    Task CreateGroup(GroupCreateRequestDto reqDto);

    Task<PaginatedResponse<GroupResponseDto>> GetGroups(GroupRequestDto reqDto);

}
