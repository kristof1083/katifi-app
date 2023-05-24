using KatifiApp.ViewModels;

namespace KatifiApp.Views;

public partial class MyEventsDetailsPage : ContentPage
{
	public MyEventsDetailsPage(EventDetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}