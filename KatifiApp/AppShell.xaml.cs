using KatifiApp.ViewModels;
using KatifiApp.Views;
using KatifiApp.Views.ContentViews;

namespace KatifiApp;

public partial class AppShell : Shell
{
	public AppShell(AppShellViewModel shellViewModel)
	{
		InitializeComponent();
        RegisterRoutes();
		BindingContext = shellViewModel;
        this.FlyoutHeader = new UserFlyoutHeader();
    }

	private void RegisterRoutes()
	{
        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(RegistrationPage), typeof(RegistrationPage));
        Routing.RegisterRoute(nameof(ChurchDetailsPage), typeof(ChurchDetailsPage));
        Routing.RegisterRoute(nameof(CommunityDetailsPage), typeof(CommunityDetailsPage));
        Routing.RegisterRoute(nameof(EventDetailsPage), typeof(EventDetailsPage));
        Routing.RegisterRoute(nameof(MyEventsDetailsPage), typeof(MyEventsDetailsPage));
        Routing.RegisterRoute(nameof(WebviewPage), typeof(WebviewPage));
    }

}
