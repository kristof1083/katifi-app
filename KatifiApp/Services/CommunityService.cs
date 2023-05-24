using KatifiApp.Models;
using System.Net.Http.Json;


namespace KatifiApp.Services;

public class CommunityService : ICommunityService
{
    readonly HttpClient _httpClient;
    List<Community> _communities = new();

    public CommunityService()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://zr4jwc3b-443.euw.devtunnels.ms");
    }

    public async Task<List<Community>> GetCommunities()
    {
        if (_communities.Count > 0)
            return _communities;

        var response = await _httpClient.GetAsync("/api/community");

        if (response.IsSuccessStatusCode)
        {
            _communities = await response.Content.ReadFromJsonAsync<List<Community>>();
        }

        return _communities;
    }
}
