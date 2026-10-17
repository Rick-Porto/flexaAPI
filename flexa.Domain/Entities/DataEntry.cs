namespace flexa.Domain.Entities;

public class DataEntry
{
    public int Id { get; set; }
    public string? Config { get; set; }
    public string Data { get; set; } = "{}"; // data as JSON string

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Supabase user ID do claim "sub" do JWT.
    // Cada app pertence a exatamente um usuário.
    public string UserId { get; set; } = string.Empty;

    // Navigation properties
    public int ScreenId { get; set; }
    public Screen? Screen { get; set; } = null!;

    public int ComponentId { get; set; }      // qual componente ela pertence
    public Component? Component { get; set; } = null!;
}
