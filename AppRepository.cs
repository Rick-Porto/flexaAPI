using System.Collections.Generic;
using System.Linq;

public class AppRepository : IAppRepository
{
    private readonly List<App> _apps = new List<App>();

    public void Add(App app)
    {
        _apps.Add(app);
    }

    public App Get(Guid id)
    {
        return _apps.FirstOrDefault(a => a.Id == id);
    }

    public IEnumerable<App> GetAll()
    {
        return _apps;
    }

    public void Update(App app)
    {
        var existingApp = Get(app.Id);
        if (existingApp != null)
        {
            existingApp.Name = app.Name;  // Update other properties as necessary
        }
    }

    public void Delete(Guid id)
    {
        var app = Get(id);
        if (app != null)
        {
            _apps.Remove(app);
        }
    }
}