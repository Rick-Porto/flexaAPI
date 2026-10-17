namespace flexa.App.DTOs.Screen
{
    public class ScreenRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Guid AppEntityId { get; set; }
    }
}
