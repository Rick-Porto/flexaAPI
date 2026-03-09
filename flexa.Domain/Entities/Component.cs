namespace flexa.Domain.Entities;

public class Component
{
    public Guid Id { get; set; }
    public string ElementType { get; set; }
    public string Label { get; set; }
    public string Model { get; set; }
    public string Config { get; set; }
    public int Order { get; set; }
    public bool Required { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public Guid ScreenId { get; set; }
    public Screen screen { get; set; }
}
