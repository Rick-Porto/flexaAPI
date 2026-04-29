namespace flexa.Domain.Entities;

public class Component
{

    public int Id { get; set; }
    public required string ElementType { get; set; }
    public required string Label { get; set; }
    public string? Model { get; set; }
    public string? Config { get; set; }
    public int Order { get; set; }
    public bool Required { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Supabase user ID do claim "sub" do JWT.
    // Cada app pertence a exatamente um usuário.
    public string UserId { get; set; } = string.Empty;

    // Navigation properties
    public int ScreenId { get; set; }
    public required Screen Screen { get; set; }
    public ICollection<DataEntry> DataEntries { get; set; } = [];

}
