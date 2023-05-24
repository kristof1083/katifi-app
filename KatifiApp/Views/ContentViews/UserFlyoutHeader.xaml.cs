namespace KatifiApp.Views.ContentViews;

public partial class UserFlyoutHeader : StackLayout
{

    public UserFlyoutHeader()
	{
		InitializeComponent();
        this.Refresh();
	}

	public void Refresh()
	{
		if(Preferences.Default.Get("loggedin", false))
		{
			var username = Preferences.Default.Get("username", string.Empty);
			lblUsername.Text = $"user: {username}";
        }
		else
		{
			lblUsername.Text = string.Empty;
		}
    }
}