namespace flexa.Domain.Entities;

public class Screen
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Supabase user ID do claim "sub" do JWT.
    // Cada app pertence a exatamente um usuário.
    public string UserId { get; set; } = string.Empty;

    // Navigation properties
    public Guid AppEntityId { get; set; }
    public required AppEntity AppEntity { get; set; } = null!;
    public ICollection<Component>? Components { get; set; } = [];
    public ICollection<DataEntry> DataEntries { get; set; } = [];

}