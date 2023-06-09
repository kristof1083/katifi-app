using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KatifiApp.Models;
using KatifiApp.Services;
using KatifiApp.Views;
using System.Diagnostics;

namespace KatifiApp.ViewModels;

[QueryProperty("Varevent", "Varevent")]
public partial class EventDetailsViewModel : BaseViewModel
{
    readonly IConnectivity _connectivity;
    readonly IEventService _service;
    readonly IPreferences _preferences;

    public EventDetailsViewModel(IConnectivity connectivity, IEventService eventService, IPreferences preferences)
    {
        _connectivity = connectivity;
        _service = eventService;
        _preferences = preferences;
    }

    [ObservableProperty]
    Event varevent;

    [RelayCommand]
    private async Task RegistAsync()
    {
        if (IsBusy)
            return;

        if (Varevent is null)
            return;

        try
        {
            if (_connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert("Internet issue", "Check your internet and try again!", "OK");
                return;
            }

            if (!_preferences.Get("loggedin", false))
            {
                await Shell.Current.DisplayAlert("Authorization issue", "Please log in, if you want to regist to an event!", "OK");
                return;
            }

            IsBusy = true;
            bool success = await _service.RegistUserForEventAsync(Varevent.Id);

            if (!success)
            {
                await Shell.Current.DisplayAlert("Registration failed", "We are unable to regist you for this event. The event is possibly in the past", "OK");
                return;
            }
                

            await Shell.Current.DisplayAlert("Success", $"Your registration was successful to {Varevent.Name}", "OK");
            await Shell.Current.GoToAsync($"//{nameof(MyEventsPage)}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error", $"Unable to regist you to the event. Error message: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task DeleteRegistrationAsync()
    {
        if (IsBusy)
            return;

        if (Varevent is null)
            return;

        try
        {
            if (_connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert("Internet issue", "Check your internet and try again!", "OK");
                return;
            }

            if (!_preferences.Get("loggedin", false))
            {
                await Shell.Current.DisplayAlert("Authorization issue", "Please log in, if you want to delete a registration!", "OK");
                return;
            }

            IsBusy = true;
            bool success = await _service.DeleteUserFromEvent(Varevent.Id);

            if (!success)
                return;

            await Shell.Current.DisplayAlert("Success", $"Your request [delete registration from {Varevent.Name}] was fulfilled.", "OK");
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error", $"Unable to delete Registration. Error message: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task UploadImageFilesAsync()
    {
        if (IsBusy)
            return;

        if (Varevent is null)
            return;

        try
        {
            if (_connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert("Internet issue", "Check your internet and try again!", "OK");
                return;
            }

            if (!_preferences.Get("loggedin", false))
            {
                await Shell.Current.DisplayAlert("Authorization issue", "Please log in, if you want to send picture!", "OK");
                return;
            }

            if(Varevent.End > DateTime.Now)
            {
                await Shell.Current.DisplayAlert("Event not finished yet", "Picture upload only available after the event.", "OK");
                return;
            }

            IsBusy = true;
            bool result = await _service.UploadImageFiles(Varevent.Id);

            if(result)
                await Shell.Current.DisplayAlert("Success", $"Your request [upload images to {Varevent.Name}] was successful.", "OK");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error", $"Unable to upload file. Error message: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task AddToLocalCalendar()
    {
        if (IsBusy)
            return;

        if (Varevent is null)
            return;

        try
        {
            if (!_preferences.Get("loggedin", false))
            {
                await Shell.Current.DisplayAlert("Authorization issue", "Please log in, if you want to see the actual data on page!", "OK");
                return;
            }

            IsBusy = true;

            bool l_result = _service.AddToLocalCalendar(Varevent);
            
            if (!l_result){
                await Shell.Current.DisplayAlert("Oppsz", "There is no app that can support this call", "Sad");
                return;
            }

            await Shell.Current.DisplayAlert("Success", $"Your request [add to calendar {Varevent.Name}] was successful.", "OK");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error", $"Unable to upload file. Error message: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task AddToGoogleCalendar()
    {
        if (IsBusy)
            return;

        if (Varevent is null)
            return;

        try
        {
            if (_connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert("Internet issue", "Check your internet and try again!", "OK");
                return;
            }

            if (!_preferences.Get("loggedin", false))
            {
                await Shell.Current.DisplayAlert("Authorization issue", "Please log in, if you want to see the actual data on page!", "OK");
                return;
            }

            IsBusy = true;
            bool g_result = await _service.AddToGoogleCalendar(Varevent);

            if (!g_result)
            {
                await Shell.Current.DisplayAlert("Oppsz", "We are unable to handle the request. Please try again later.", "OK");
                return;
            }


            await Shell.Current.DisplayAlert("Success", $"Your request has been fulfilled.{Environment.NewLine}{Varevent.Name}] is now added to your calendar.", "OK");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error", $"Some error happend. Error message: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
