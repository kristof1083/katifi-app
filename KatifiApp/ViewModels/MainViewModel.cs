using CommunityToolkit.Mvvm.Input;
using KatifiApp.Services;
using System.Diagnostics;
#if ANDROID
using KatifiApp.Platforms.Android.Services;
#endif

namespace KatifiApp.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    readonly IAuthenticationService _authservice;
    readonly IConnectivity _connectivity;
    public MainViewModel(IAuthenticationService service, IConnectivity connectivity)
    {
        _authservice = service;
        _connectivity = connectivity;
    }

    [RelayCommand]
    private async Task SendNotificationAsync()
    {
        if (IsBusy)
            return;

        try
        {
            var token = await SecureStorage.GetAsync("jwttoken");
            if (token is null)
            {
                await Shell.Current.DisplayAlert("Authorization issue", "Please log in, if you want to send push notification!", "OK");
                return;
            }

            if (_connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert("Internet issue", "Check your internet and try again!", "OK");
                return;
            }

            var roles = _authservice.GetRolesFromJwtToken(token);

#if ANDROID
            if (roles.Contains("Admin"))
            {
                bool success = await FirebaseService.SendPushNotification("HU-Katifi-Title", "HU-Katifi-Message");
                if (!success)
                    await App.Current.MainPage.DisplayAlert("Error", "Unexpected error. Probably on Firebase side.", "OK");
            }
            else
            {
                await Shell.Current.DisplayAlert("Authorization issue", "You do not have the required role for this action!", "OK");
            }
#endif
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error", $"Unable to load Events. Error message: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
