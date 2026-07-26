using Quiz.Shared.Abstractions;
using System.ComponentModel;

namespace Quiz.Shared.DTO.User.Request;
public class RelatedTeachersRequestDto : IPageable, ISearchable
{
    [DefaultValue(1)]
    public int Page { get; set; }

    [DefaultValue(10)]
    public int PerPage { get; set; }

    public string SearchQuery { get; set; } = string.Empty;
}
