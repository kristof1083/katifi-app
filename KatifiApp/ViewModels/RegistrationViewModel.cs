using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KatifiApp.Models;
using KatifiApp.Services;
using KatifiApp.Views;
using System.Diagnostics;

namespace KatifiApp.ViewModels;

public partial class RegistrationViewModel : BaseViewModel
{
    readonly IConnectivity _connectivity;
    readonly ISecureStorage _secureStorage;
    readonly IAuthenticationService _authservice;

    public RegistrationViewModel(IConnectivity connectivity, ISecureStorage secureStorage, IAuthenticationService authservice)
    {
        Title = "User registration";
        _connectivity = connectivity;
        _secureStorage = secureStorage;
        _authservice = authservice;
    }

    [ObservableProperty]
    RegisterModel registModel;

    [RelayCommand]
    private async Task RegistUserAsync()
    {
        if (IsBusy)
            return;

        if (RegistModel == null)
            return;

        try
        {
            if (_connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert("Internet issue", "Check your internet and try again!", "OK");
                return;
            }

            var success = await _authservice.RegistAsync(RegistModel);

            if(!success)
            {
                await Shell.Current.DisplayAlert("Registration failed", "Try again later.", "OK");
                return;
            }

            await Shell.Current.GoToAsync(nameof(LoginPage));

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error", $"Unable to regist user. Error message: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
