using CommunityToolkit.Mvvm.ComponentModel;
using KatifiApp.Models;
using Microsoft.Maui.Controls;

namespace KatifiApp.ViewModels;

[QueryProperty("Church", "Church")]
public partial class ChurchDetailsViewModel : BaseViewModel
{
    public ChurchDetailsViewModel()
    {
    }

    [ObservableProperty]
    Church church;

}
