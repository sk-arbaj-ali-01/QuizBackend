namespace Quiz.Shared.Abstractions;
public interface IPageable
{
    public int Page { get; set; }

    public int PerPage { get; set; }
}
