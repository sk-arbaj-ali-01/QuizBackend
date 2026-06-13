using Quiz.DL.Parameters;
using Quiz.Shared.DTO.Group.Request;

namespace Quiz.BL.DtoToEntityExtensions.Group;
public static class GroupRequestDtoToGroupParameter
{
    public static GroupParameter ConvertToParameter(this GroupRequestDto requestDto)
    {
        return new GroupParameter
        {
            Page = requestDto.Page,
            PerPage = requestDto.PerPage,
            SearchQuery = requestDto.SearchQuery,
            IsActive = requestDto.IsActive,
            IsArchieved = requestDto.IsArchieved,
        };
    }
}
