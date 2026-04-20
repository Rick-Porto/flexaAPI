using flexa.App.DTOs.DataEntry;

namespace flexa.App.DTOs.Component;

public class ComponentResponse
{
    public Guid Id { get; set; }
    public string ElementType { get; set; } = null!;
    public string Label { get; set; } = null!;
    public string? Model { get; set; }
    public string? Config { get; set; }
    public int Order { get; set; }
    public bool Required { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid ScreenId { get; set; }
    public List<DataEntryResponse>? DataEntries { get; set; }
}
