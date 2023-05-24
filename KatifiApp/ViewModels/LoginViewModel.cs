using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KatifiApp.Models;
using KatifiApp.Services;
using KatifiApp.Views;
using KatifiApp.Views.ContentViews;
using System.Diagnostics;
namespace KatifiApp.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
    readonly IConnectivity _connectivity;
    readonly IPreferences _preferences;
    readonly ISecureStorage _secureStorage;
    readonly IAuthenticationService _authservice;

    public LoginViewModel(IConnectivity connectivity, ISecureStorage secureStorage, IPreferences preferences, IAuthenticationService authservice)
    {
        Title = "Log in";
        _connectivity = connectivity;
        _secureStorage = secureStorage;
        _authservice = authservice;
        _preferences = preferences;
    }

    [ObservableProperty]
    string username;

    [ObservableProperty]
    string password;

    [ObservableProperty]
    bool rememberMe;

    public async void SetEntryTexts()
    {
        bool rememberme = _preferences.Get("rememberme", false);
        if (rememberme)
        {
            Username = await _secureStorage.GetAsync("username");
            Password = await _secureStorage.GetAsync("password");
        }
        else
        {
            Username = string.Empty;
            Password = string.Empty;
        }
        RememberMe = rememberme;
    }


    [RelayCommand]
    private async Task LoginAsync()
    {
        if (IsBusy)
            return;

        if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            return;

        LoginModel loginModel = new()
        {
            Username = Username,
            Password = Password
        };

        try
        {
            if (_connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert("Internet issue", "Check your internet and try again!", "OK");
                return;
            }

            var loggedInModel = await _authservice.LogInAsync(loginModel);

            if (loggedInModel is null || string.IsNullOrWhiteSpace(loggedInModel.JwtToken))
            {
                await Shell.Current.DisplayAlert("Login error", "Wrong username or password", "OK");
                return;
            }

            if (RememberMe)
            {
                await _secureStorage.SetAsync("username", loginModel.Username);
                await _secureStorage.SetAsync("password", loginModel.Password);
            }

            await _secureStorage.SetAsync("myid", loggedInModel.LoggedInUser.Id.ToString());
            await _secureStorage.SetAsync("jwttoken", loggedInModel.JwtToken);

            _preferences.Set("username", loggedInModel.LoggedInUser.Username);
            _preferences.Set("rememberme", RememberMe);
            _preferences.Set("loggedin", true);

            ((UserFlyoutHeader)AppShell.Current.FlyoutHeader).Refresh();

            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Login error", $"Something went wrong. Error message: {ex.Message}", "OK");
        }
        finally
        {
            this.SetEntryTexts();
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task GoToRegistrationPage()
    {
        await Shell.Current.GoToAsync(nameof(RegistrationPage));
    }
}
