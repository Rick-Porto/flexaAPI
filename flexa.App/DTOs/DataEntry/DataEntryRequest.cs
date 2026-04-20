namespace flexa.App.DTOs.DataEntry;

public class DataEntryRequest
{
    public string? Config { get; set; }
    public string Data { get; set; } = "{}";
    public Guid ScreenId { get; set; }
    public Guid ComponentId { get; set; }
}