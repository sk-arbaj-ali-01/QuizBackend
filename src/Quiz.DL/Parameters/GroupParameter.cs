namespace Quiz.DL.Parameters;
public class GroupParameter : BaseParameter
{
    public string SearchQuery { get; set; } = string.Empty;

    public bool? IsActive { get; set; }

    public bool? IsArchieved { get; set; }
}
