using KatifiApp.ViewModels;

namespace KatifiApp.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        this.SetElementsvalue();
    }

	private void SetElementsvalue()
	{
        LoginViewModel viewModel = BindingContext as LoginViewModel;
        viewModel.SetEntryTexts();
    }

}