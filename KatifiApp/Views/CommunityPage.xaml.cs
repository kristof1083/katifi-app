using KatifiApp.ViewModels;

namespace KatifiApp.Views;

public partial class CommunityPage : ContentPage
{
	public CommunityPage(CommunityViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}