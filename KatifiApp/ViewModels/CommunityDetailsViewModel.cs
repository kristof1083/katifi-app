using CommunityToolkit.Mvvm.ComponentModel;
using KatifiApp.Models;
using Microsoft.Maui.Controls;

namespace KatifiApp.ViewModels;

[QueryProperty("Community","Community")]
public partial class CommunityDetailsViewModel : BaseViewModel
{
    public CommunityDetailsViewModel()
    {
    }

    [ObservableProperty]
    Community community;

}
