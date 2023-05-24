using KatifiApp.ViewModels;

namespace KatifiApp.Views;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
