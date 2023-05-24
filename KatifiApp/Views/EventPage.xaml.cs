using KatifiApp.ViewModels;

namespace KatifiApp.Views;

public partial class EventPage : ContentPage
{
	public EventPage(EventViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		this.LoadData();
	}

    private async void LoadData()
    {
        await (this.BindingContext as EventViewModel).GetEventsAsync();
    }
}