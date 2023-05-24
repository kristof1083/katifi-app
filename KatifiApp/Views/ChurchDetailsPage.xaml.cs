using KatifiApp.ViewModels;

namespace KatifiApp.Views;


public partial class ChurchDetailsPage : ContentPage
{
	public ChurchDetailsPage(ChurchDetailsViewModel detailsViewModel)
	{
		InitializeComponent();
		BindingContext = detailsViewModel;
	}
}