using Quiz.Shared.Abstractions;
using Quiz.Shared.Models;

namespace Quiz.BL.Services;
public abstract class BaseService
{
    public PaginatedResponse<T> ToPaginatedResponse<T, TrequestDto>(
        TrequestDto resDto,
        IEnumerable<T> records,
        long totalCount) where TrequestDto : IPageable
    {
        return new PaginatedResponse<T>(resDto.Page, resDto.PerPage, records, totalCount);
    }
}
