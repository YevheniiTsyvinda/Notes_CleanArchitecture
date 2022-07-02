namespace Notes.Domain;

public class Note
{
    public Guid UserId { get; set; }
    public Guid Id { get; set; }
    public string Title { get; set; } = String.Empty;
    public string Details { get; set; } =String.Empty;
    public DateTime CreationDate { get; set; }
    public DateTime? EditDate { get; set; }

}
