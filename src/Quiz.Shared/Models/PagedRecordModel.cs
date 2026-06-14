namespace Quiz.Shared.Models;
public class PagedRecordModel<Tresponse>
{
    public IEnumerable<Tresponse> Records { get; set; } = new List<Tresponse>();

    public int TotalCount { get; set; }
}
