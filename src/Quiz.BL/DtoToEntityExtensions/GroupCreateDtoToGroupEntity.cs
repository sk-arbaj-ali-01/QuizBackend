using Quiz.DL.Entities;
using Quiz.Shared.DTO.Group.Request;

namespace Quiz.BL.DtoToEntityExtensions;
public static class GroupCreateDtoToGroupEntity
{
    public static GroupEntity ConvertToEntity(this GroupCreateRequestDto requestDto)
    {
        var entity = new GroupEntity
        {
            GroupName = requestDto.GroupName,
            Description = requestDto.Description,
            IsActive = requestDto.IsActive,
            ActiveForDays = requestDto.ActiveForDays,
            IsArchived = requestDto.IsArchived,
        };

        return entity;
    }
}
