using KatifiApp.Models;
using KatifiWebServer.Models.SecurityModels;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace KatifiApp.Services;

public class AuthenticationService : IAuthenticationService
{
    readonly HttpClient _httpClient;

    public AuthenticationService()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://zr4jwc3b-443.euw.devtunnels.ms");
    }

    public async Task<LoggedInModel> LogInAsync(LoginModel loginModel)
    {
    
        string json = JsonSerializer.Serialize(value: loginModel);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/api/user/login", content);

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<LoggedInModel>();
    }

    public async Task<bool> RegistAsync(RegisterModel registerModel)
    {
        string json = JsonSerializer.Serialize(value: registerModel);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/api/user/regist", content);

        if (!response.IsSuccessStatusCode)
            return false;

        return true;
    }

    public List<string> GetRolesFromJwtToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(token);
        var roles = new List<string>();

        foreach (var claim in jwtSecurityToken.Claims)
        {
            if (claim.Type.Contains("role"))
            {
                roles.Add(claim.Value);
            }
        }

        return roles;
    }
}
