namespace flexa.App.DTOs.AppEntity
{
    public class AppEntityResponse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? PublicLink { get; set; }

        public string UserId { get; set; } = string.Empty; // ✅ vem do JWT, nunca do body do request
    }
}