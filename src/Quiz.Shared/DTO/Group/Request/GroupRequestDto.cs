using Quiz.Shared.Abstractions;
using System.ComponentModel;

namespace Quiz.Shared.DTO.Group.Request;
public class GroupRequestDto : IPageable, ISearchable
{
    [DefaultValue(1)]
    public int Page { get; set; }

    [DefaultValue(10)]
    public int PerPage { get; set; }

    public string SearchQuery { get; set; } = string.Empty;

    public bool? IsActive { get; set; }

    public bool? IsArchieved { get; set; }
}
