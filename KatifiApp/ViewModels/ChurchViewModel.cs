using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KatifiApp.Models;
using KatifiApp.Services;
using KatifiApp.Views;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace KatifiApp.ViewModels;

public partial class ChurchViewModel : BaseViewModel
{
    readonly IChurchService _service;
    readonly IConnectivity _connectivity;
    public ObservableCollection<Church> Churches { get; } = new();

    public ChurchViewModel(IChurchService churchService, IConnectivity connectivity)
    {
        Title = "Churches page";
        _service = churchService;
        _connectivity = connectivity;
        Task.Run(async () => await GetChurchesAsync());
    }

    [ObservableProperty]
    bool isRefreshing;

    [RelayCommand]
    private async Task GetChurchesAsync()
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
            var churhes = await _service.GetChurches();

            if (Churches.Count != 0)
                Churches.Clear();

            foreach (var church in churhes)
                Churches.Add(church);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error", $"Unable to load Churches. Error message: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    private async Task GoToDetailsAsync(Church church)
    {
        if(church == null)
            return;

        if (string.IsNullOrWhiteSpace(church.Vicar))
            church.Vicar = "n/a";

        await Shell.Current.GoToAsync($"{nameof(ChurchDetailsPage)}", true, new Dictionary<string, object>()
        {
            {"Church", church}
        });
    }

}
