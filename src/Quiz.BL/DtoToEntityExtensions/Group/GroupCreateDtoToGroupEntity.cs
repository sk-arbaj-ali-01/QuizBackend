using Quiz.DL.Entities;
using Quiz.Shared.DTO.Group.Request;

namespace Quiz.BL.DtoToEntityExtensions.Group;
public static class GroupCreateDtoToGroupEntity
{
    public static GroupEntity ConvertToEntity(this GroupCreateRequestDto requestDto)
    {
        var entity = new GroupEntity
        {
            GroupId = Guid.NewGuid(),
            GroupName = requestDto.GroupName,
            Description = requestDto.Description,
            IsActive = requestDto.IsActive,
            ActiveForDays = requestDto.ActiveForDays
        };

        return entity;
    }
}
