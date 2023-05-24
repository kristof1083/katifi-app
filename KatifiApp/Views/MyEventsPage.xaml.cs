using KatifiApp.ViewModels;

namespace KatifiApp.Views;

public partial class MyEventsPage : ContentPage
{
	public MyEventsPage(EventViewModel eventViewModel)
	{
		InitializeComponent();
		BindingContext = eventViewModel;
		this.LoadData();
	}

	private async void LoadData()
	{
        await (this.BindingContext as EventViewModel).GetMyEventsAsync();
    }
}