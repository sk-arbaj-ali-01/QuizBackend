using Microsoft.Extensions.Logging;
using Quiz.BL.Abstractions;
using Quiz.BL.DtoToEntityExtensions.Group;
using Quiz.DL.Abstractions;
using Quiz.DL.Entities;
using Quiz.DL.Parameters;
using Quiz.Shared.DTO.Group.Request;
using Quiz.Shared.DTO.Group.Response;
using Quiz.Shared.Models;

namespace Quiz.BL.Services;
public class GroupService(
    ILogger<GroupService> logger,
    IGroupRepository groupRepository) : BaseService, IGroupService
{
    public async Task CreateGroup(GroupCreateRequestDto reqDto)
    {
        GroupEntity entity = reqDto.ConvertToEntity();

        await groupRepository.CreateGroup(entity);
    }

    public async Task<PaginatedResponse<GroupResponseDto>> GetGroups(GroupRequestDto reqDto)
    {
        GroupParameter parameter = reqDto.ConvertToParameter();

        PagedRecordModel<GroupResponseDto> records = await groupRepository.GetGroups(parameter);

        return ToPaginatedResponse(reqDto, records.Records, records.TotalCount);
    }
}
