using KatifiApp.ViewModels;

namespace KatifiApp;

public partial class App : Application
{

	public App(AppShellViewModel shellViewModel)
	{
		InitializeComponent();

		MainPage = new AppShell(shellViewModel);
	}
}
