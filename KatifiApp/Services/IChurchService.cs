using KatifiApp.Models;

namespace KatifiApp.Services;

public interface IChurchService
{
    public Task<List<Church>> GetChurches();
}
