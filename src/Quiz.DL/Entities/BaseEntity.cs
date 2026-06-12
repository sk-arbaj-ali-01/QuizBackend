namespace Quiz.DL.Entities;
public abstract class BaseEntity
{
    public Guid CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime ModifiedAt { get; set; }
}
