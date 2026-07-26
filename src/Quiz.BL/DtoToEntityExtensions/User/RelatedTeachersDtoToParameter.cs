using Quiz.DL.Parameters;
using Quiz.Shared.DTO.User.Request;

namespace Quiz.BL.DtoToEntityExtensions.User;
public static class RelatedTeachersDtoToParameter
{
    public static RelatedTeachersParameter ConvertToParameter(
        this RelatedTeachersRequestDto requestDto)
    {
        return new RelatedTeachersParameter
        {
            Page = requestDto.Page,
            PerPage = requestDto.PerPage,
            SearchQuery = requestDto.SearchQuery
        };
    }
}
