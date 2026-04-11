public interface IAppRepository
{
    // Define the required methods here
    void Add(App app);
    App Get(Guid id);
    IEnumerable<App> GetAll();
    void Update(App app);
    void Delete(Guid id);
}