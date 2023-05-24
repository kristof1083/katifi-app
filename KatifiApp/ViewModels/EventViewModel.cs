using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KatifiApp.Models;
using KatifiApp.Services;
using KatifiApp.Views;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace KatifiApp.ViewModels;


public partial class EventViewModel : BaseViewModel
{
    readonly IEventService _service;
    readonly IConnectivity _connectivity;
    readonly IPreferences _preferences;

    public ObservableCollection<Event> Events { get; } = new();
    public ObservableCollection<Event> MyEvents { get; } = new();

    public EventViewModel(IEventService service, IPreferences preferences, IConnectivity connectivity)
    {
        Title = "Events";
        _service = service;
        _preferences = preferences;
        _connectivity = connectivity;
    }

    [ObservableProperty]
    bool isRefresing;

    [RelayCommand]
    public async Task GetEventsAsync()
    {
        if (IsBusy)
            return;

        try
        {
            if (_connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert("Internet issue", "Check your internet and try again!", "OK");
                return;
            }

            IsBusy = true;
            var events = await _service.GetEvents();

            if (Events.Count != 0)
                Events.Clear();

            foreach (var varevent in events)
                Events.Add(varevent);                
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error", $"Unable to load Events. Error message: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
            IsRefresing = false;
        }
    }

    [RelayCommand]
    public async Task GetMyEventsAsync()
    {
        if (IsBusy)
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
                await Shell.Current.DisplayAlert("Authorization issue", "Please log in, if you want to see tha actual data on page!", "OK");
                return;
            }

            IsBusy = true;
            var myevents = await _service.GetMyEvents();

            if (MyEvents.Count != 0)
                MyEvents.Clear();

            foreach (var varevent in myevents)
                MyEvents.Add(varevent);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error", $"Unable to load Events. Error message: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
            IsRefresing = false;
        }
    }

    [RelayCommand]
    private async Task GoToDetailsAsync(Event varevent)
    {
        if (varevent == null)
            return;

        await Shell.Current.GoToAsync($"{nameof(EventDetailsPage)}", true, new Dictionary<string, object>()
        {
            {"Varevent", varevent}
        });
    }

    [RelayCommand]
    private async Task GoToMyDetailsAsync(Event varevent)
    {
        if (varevent == null)
            return;

        await Shell.Current.GoToAsync($"{nameof(MyEventsDetailsPage)}", true, new Dictionary<string, object>()
        {
            {"Varevent", varevent}
        });
    }

}
