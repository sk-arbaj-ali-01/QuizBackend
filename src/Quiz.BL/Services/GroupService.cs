using Microsoft.Extensions.Logging;
using Quiz.BL.Abstractions;
using Quiz.BL.DtoToEntityExtensions.Group;
using Quiz.DL.Abstractions;
using Quiz.DL.Entities;
using Quiz.DL.Parameters;
using Quiz.Shared.DTO.Group.Request;
using Quiz.Shared.DTO.Group.Response;
using Quiz.Shared.Exceptions.DatabaseExceptions;
using Quiz.Shared.Models;

namespace Quiz.BL.Services;
public class GroupService(
    ILogger<GroupService> logger,
    IGroupRepository groupRepository) : BaseService, IGroupService
{
    public async Task CreateGroup(GroupCreateRequestDto reqDto, string userId)
    {
        GroupEntity entity = reqDto.ConvertToEntity();
        entity.CreatedBy = Guid.Parse(userId);

        await groupRepository.CreateGroup(entity);
    }

    public async Task<PaginatedResponse<GroupResponseDto>> GetGroups(GroupRequestDto reqDto, Guid userId)
    {
        GroupParameter parameter = reqDto.ConvertToParameter();

        PagedRecordModel<GroupResponseDto> records = await groupRepository.GetGroups(parameter, userId);

        return ToPaginatedResponse(reqDto, records.Records, records.TotalCount);
    }

    public async Task<GroupResponseDto> GetGroupById(Guid groupId)
    {
        GroupResponseDto response = await groupRepository.GetGroupById(groupId);

        if(response == null)
        {
            logger.LogInformation("Group not found for this {Id}", groupId);
            throw new RecordNotFoundException($"Group not found for this {groupId}");
        }

        return response;
    }

    public async Task UpdateGroupById(Guid groupId, GroupUpdateRequestDto reqDto)
    {
        GroupEntity groupEntity = reqDto.ConvertToEntity();
        groupEntity.GroupId = groupId;

        await groupRepository.UpdateGroupById(groupEntity);
    }

    public async Task DeleteGroupById(Guid groupId)
    {
        await groupRepository.DeleteGroupById(groupId);
    }
}
