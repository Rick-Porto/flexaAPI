namespace flexa.App.DTOs.DataEntry
{
    public class DataEntryResponse
    {
        public Guid Id { get; set; }
        public string? Config { get; set; }
        public string Data { get; set; } = "{}"; // JSON string
        public Guid ComponentId { get; set; }
        public Guid ScreenId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
