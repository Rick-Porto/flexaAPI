using flexa.App.DTOs.Component;
using flexa.App.DTOs.DataEntry;

namespace flexa.App.DTOs.Screen
{
    public class ScreenResponse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Guid AppEntityId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Read‑only navigation collections
        public IReadOnlyCollection<ComponentResponse>? Components { get; set; }
        public IReadOnlyCollection<DataEntryResponse>? DataEntries { get; set; }
    }
}
