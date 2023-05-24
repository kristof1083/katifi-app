using CommunityToolkit.Mvvm.Input;
using KatifiApp.Views;
using KatifiApp.Views.ContentViews;
using System.Diagnostics;

namespace KatifiApp.ViewModels;

public partial class AppShellViewModel : BaseViewModel
{
    readonly ISecureStorage _secureStorage;
    readonly IPreferences _preferences;

    public AppShellViewModel(ISecureStorage secureStorage, IPreferences preferences)
    {
        _secureStorage = secureStorage;
        _preferences = preferences;
    }

    [RelayCommand]
    private async Task LogOutAsync()
    {
        if (IsBusy)
            return;

        try
        {
            if (!_preferences.Get("loggedin", false))
                return;

            if (string.IsNullOrWhiteSpace(await _secureStorage.GetAsync("jwttoken")))
                return;

            _secureStorage.Remove("jwttoken");
            _secureStorage.Remove("myid");

            _preferences.Set("loggedin", false);
            _preferences.Remove("username");
            ((UserFlyoutHeader)AppShell.Current.FlyoutHeader).Refresh();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Login error", $"Unable to log out.\nError message: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
        }
    }

    [RelayCommand]
    private async Task QuitAsync()
    {
        bool answer = await Shell.Current.DisplayAlert("Close application", "Are you sure, you want to quit?", "Yes", "No");
        if (!answer)
            return;

    #if IOS
            System.Environment.Exit(0);
    #else
            Application.Current.Quit();
    #endif

    }
}
