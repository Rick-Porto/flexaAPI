namespace flexa.Domain.Entities;

public class DataEntry
{
    public Guid Id { get; set; }
    public string? Config { get; set; }
    public string Data { get; set; } = "{}"; // data as JSON string

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public Guid ScreenId { get; set; }
    public Screen? Screen { get; set; } = null!;

    public Guid ComponentId { get; set; }      // qual componente ela pertence
    public Component? Component { get; set; } = null!;
}
