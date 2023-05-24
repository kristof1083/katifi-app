using KatifiApp.Models;
using System.Net.Http.Json;

namespace KatifiApp.Services;

public class ChurchService : IChurchService
{
    readonly HttpClient _httpClient = new(); 
    List<Church> churches = new ();

    public ChurchService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<List<Church>> GetChurches()
    {
        if(churches.Count > 0)
            return churches;

        var url = "https://zr4jwc3b-443.euw.devtunnels.ms/api/church";

        var response = await _httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            churches = await response.Content.ReadFromJsonAsync<List<Church>>();
        }

        return churches;
    }
}
