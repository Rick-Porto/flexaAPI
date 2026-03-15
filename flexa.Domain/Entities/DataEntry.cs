namespace flexa.Domain.Entities;

public class DataEntry
{
    public Guid Id { get; set; }
    public required string Config { get; set; }
    public string Data { get; set; } = "{}"; // data as JSON string

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public Guid ScreenId { get; set; }
    public Screen? Screens { get; set; }

    public Guid ComponentId { get; set; }      // qual componente ela pertence
    public Component? Components { get; set; } = null!;
}
