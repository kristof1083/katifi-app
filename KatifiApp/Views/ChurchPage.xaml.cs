using KatifiApp.ViewModels;

namespace KatifiApp.Views;

public partial class ChurchPage : ContentPage
{
	
	public ChurchPage(ChurchViewModel churchViewModel)
	{
		InitializeComponent();
		BindingContext = churchViewModel;
	}
}