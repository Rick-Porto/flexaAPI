namespace flexa.App.DTOs.DataEntry
{
    public class DataEntryResponse
    {
        public int Id { get; set; }
        public string? Config { get; set; }
        public string Data { get; set; } = "{}";
        public int ComponentId { get; set; }
        public int ScreenId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
