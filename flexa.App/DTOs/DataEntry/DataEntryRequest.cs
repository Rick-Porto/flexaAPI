namespace flexa.App.DTOs.DataEntry;

public class DataEntryRequest
{
    public string? Config { get; set; }
    public string Data { get; set; } = "{}";
    public int ScreenId { get; set; }
    public int ComponentId { get; set; }
}
