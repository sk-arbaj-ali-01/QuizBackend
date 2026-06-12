using Microsoft.Extensions.Logging;
using Quiz.BL.Abstractions;
using Quiz.BL.DtoToEntityExtensions;
using Quiz.DL.Abstractions;
using Quiz.DL.Entities;
using Quiz.Shared.DTO.Group.Request;

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
}
