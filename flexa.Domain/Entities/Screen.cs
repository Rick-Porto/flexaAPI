namespace flexa.Domain.Entities;

public class Screen
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public Guid AppId { get; set; }
    public App App { get; set; }
}