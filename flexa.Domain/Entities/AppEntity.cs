namespace flexa.Domain.Entities;

public class AppEntity
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Supabase user ID do claim "sub" do JWT.
    // Cada app pertence a exatamente um usuário.
    public string UserId { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<Screen>? Screens { get; set; } = [];

}

