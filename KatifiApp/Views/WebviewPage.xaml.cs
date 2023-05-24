using KatifiApp.ViewModels;

namespace KatifiApp.Views;

public partial class WebviewPage : ContentPage
{
	public WebviewPage(WebViewViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}