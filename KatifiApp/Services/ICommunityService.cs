using KatifiApp.Models;


namespace KatifiApp.Services;

public interface ICommunityService
{
    public Task<List<Community>> GetCommunities();
}
