namespace flexa.App.DTOs.Component;

public class ComponentRequest
{
    public required string ElementType { get; set; }
    public required string Label { get; set; }
    public string? Model { get; set; }
    public string? Config { get; set; }
    public int Order { get; set; }
    public bool Required { get; set; }
    public int ScreenId { get; set; }
}