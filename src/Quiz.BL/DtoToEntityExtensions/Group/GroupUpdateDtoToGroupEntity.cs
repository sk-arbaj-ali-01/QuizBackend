using Quiz.DL.Entities;
using Quiz.Shared.DTO.Group.Request;

namespace Quiz.BL.DtoToEntityExtensions.Group;
public static class GroupUpdateDtoToGroupEntity
{
    public static GroupEntity ConvertToEntity(this GroupUpdateRequestDto reqDto)
    {
        return new GroupEntity
        {
            GroupName = reqDto.GroupName,
            Description = reqDto.Description,
            IsActive = reqDto.IsActive,
            IsArchived = reqDto.IsArchived,
            ActiveForDays = reqDto.ActiveForDays,
            ExamDuration = reqDto.ExamDuration,
        };
    }
}
