using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KatifiApp.Models;
using KatifiApp.Services;
using KatifiApp.Views;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace KatifiApp.ViewModels;


public partial class CommunityViewModel : BaseViewModel
{
    readonly ICommunityService _service;
    readonly IConnectivity _connectivity;
    public ObservableCollection<Community> Commmunities { get; } = new();

    public CommunityViewModel(ICommunityService service, IConnectivity connectivity)
    {
        Title = "Communities page";
        _service = service;
        _connectivity = connectivity;
        Task.Run(async () => await GetCommunitiesAsync());
    }

    [ObservableProperty]
    bool isRefreshing;

    [RelayCommand]
    private async Task GetCommunitiesAsync()
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
            var communities = await _service.GetCommunities();

            if (Commmunities.Count != 0)
                Commmunities.Clear();

            foreach (var community in communities)
                Commmunities.Add(community);                
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error", $"Unable to load Communities. Error message: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    private async Task GoToDetailsAsync(Community community)
    {
        if (community == null)
            return;

        await Shell.Current.GoToAsync($"{nameof(CommunityDetailsPage)}", true, new Dictionary<string, object>()
        {
            {"Community", community}
        });
    }

}
