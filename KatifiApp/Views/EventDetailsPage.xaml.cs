using KatifiApp.ViewModels;

namespace KatifiApp.Views;

public partial class EventDetailsPage : ContentPage
{
	public EventDetailsPage(EventDetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}