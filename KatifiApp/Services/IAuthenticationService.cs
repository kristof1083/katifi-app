using KatifiApp.Models;
using KatifiWebServer.Models.SecurityModels;

namespace KatifiApp.Services;

public interface IAuthenticationService
{
    public Task<LoggedInModel> LogInAsync(LoginModel loginModel);
    public Task<bool> RegistAsync(RegisterModel registerModel);
    public List<string> GetRolesFromJwtToken(string token);
}
