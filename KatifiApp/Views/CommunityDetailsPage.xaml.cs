using KatifiApp.ViewModels;

namespace KatifiApp.Views;

public partial class CommunityDetailsPage : ContentPage
{
	public CommunityDetailsPage(CommunityDetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}