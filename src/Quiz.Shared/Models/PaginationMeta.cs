namespace Quiz.Shared.Models;
public class PaginationMeta
{
    public int CurrentPage { get; set; }

    public int PerPage { get; set; }

    public long TotalCount { get; set; }

    public long TotalPages { get; set; }
}
