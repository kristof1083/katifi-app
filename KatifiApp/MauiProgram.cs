using KatifiApp.Services;
using KatifiApp.ViewModels;
using KatifiApp.Views;
using Microsoft.Extensions.Logging;

namespace KatifiApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		#region Add Services
		//API-ok
		builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
		builder.Services.AddSingleton<IPreferences>(Preferences.Default);
		builder.Services.AddSingleton<ISecureStorage>(SecureStorage.Default);
		builder.Services.AddSingleton<IFilePicker>(FilePicker.Default);

        //Service-ek
        builder.Services.AddSingleton<BaseService>();
        builder.Services.AddSingleton<IEventService, EventService>();
        builder.Services.AddSingleton<IChurchService, ChurchService>();
        builder.Services.AddSingleton<ICommunityService, CommunityService>();
        builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();

		//ViewModelek
        builder.Services.AddSingleton<AppShellViewModel>();
        builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddTransient<EventViewModel>();
        builder.Services.AddSingleton<LoginViewModel>();
        builder.Services.AddSingleton<ChurchViewModel>();
        builder.Services.AddSingleton<CommunityViewModel>();
        builder.Services.AddSingleton<RegistrationViewModel>();

        builder.Services.AddTransient<EventDetailsViewModel>();
		builder.Services.AddTransient<ChurchDetailsViewModel>();
		builder.Services.AddTransient<CommunityDetailsViewModel>();
		builder.Services.AddTransient<WebViewViewModel>();

		//Page-ek
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<EventPage>();
        builder.Services.AddSingleton<MyEventsPage>();
        builder.Services.AddSingleton<ChurchPage>();
        builder.Services.AddSingleton<CommunityPage>();
        builder.Services.AddSingleton<RegistrationPage>();
        builder.Services.AddSingleton<WebviewPage>();

        //Details page-ek
        builder.Services.AddTransient<EventDetailsPage>();
        builder.Services.AddTransient<MyEventsDetailsPage>();
		builder.Services.AddTransient<ChurchDetailsPage>();
		builder.Services.AddTransient<CommunityDetailsPage>();
        #endregion



#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
