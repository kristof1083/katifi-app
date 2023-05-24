using KatifiApp.ViewModels;

namespace KatifiApp.Views;

public partial class RegistrationPage : ContentPage
{
	public RegistrationPage(RegistrationViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}