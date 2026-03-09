namespace flexa.Domain.Entities;

public class App
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public ICollection<Screen> Screens { get; set; }

    //public ICollection<Component> Components { get; set; }
    //public ICollection<DataEntry> DataEntries { get; set; }
    //public App()
    //{
    //    Screens = new List<Screen>();
    //    Components = new List<Component>();
    //    DataEntries = new List<DataEntry>();
    //}

}

