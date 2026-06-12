using Quiz.Shared.Abstractions;

namespace Quiz.Shared.Models;
public class PaginatedResponse<T>
{
    public IEnumerable<T> Records { get; set; }

    public PaginationMeta Meta { get; set; }
    public PaginatedResponse(int page,
        int perPage,
        IEnumerable<T> records, 
        long totalCount)
    {
        Records = records;
        Meta = CreatePaginationMeta(page,
        perPage, totalCount);
    }

    private PaginationMeta CreatePaginationMeta(
        int page,
        int perPage, 
        long totalCount)
    {
        if(Records.Any())
        {
            return new PaginationMeta
            {
                CurrentPage = page,
                PerPage = perPage,
                TotalCount = totalCount,
                TotalPages = (long)Math.Ceiling((double)totalCount / perPage)
            };
        }

        return new PaginationMeta();
    }
}
