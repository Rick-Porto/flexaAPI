namespace flexa.Domain.Entities;

public class DataEntry
{
    public Guid Id { get; set; }
    public string ElementType { get; set; }
    public string Label { get; set; }
    public string Model { get; set; }
    public string Config { get; set; }
    public int Order { get; set; }
    public string Data { get; set; } = "{}"; // Store data as JSON string

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public Guid ScreenId { get; set; }
    public Screen screen { get; set; }
}
