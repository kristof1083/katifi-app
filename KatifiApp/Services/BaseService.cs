namespace KatifiApp.Services;

public class BaseService
{
    protected readonly HttpClient _httpClient;

    protected int? MyId { get => this.GetMyId();}

    public BaseService()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://zr4jwc3b-443.euw.devtunnels.ms");
    }

    private int? GetMyId()
    {
        var stringid = SecureStorage.Default.GetAsync("myid").GetAwaiter().GetResult();
        if (stringid is null)
            return null;

        return int.Parse(stringid);
    }
}
