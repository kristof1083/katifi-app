using Android.App;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Core.Content;

namespace KatifiApp;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    internal static readonly string ChannelID = "MainChannel";
    internal static readonly int NotificationID = 777;

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        if (ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.PostNotifications) == Permission.Denied)
        {
            ActivityCompat.RequestPermissions(this, new String[] { Android.Manifest.Permission.PostNotifications }, 1);
        }

        ShowToken();
        CreateNotificationChannel();
    }

    private static void ShowToken()
    {
        bool tokenexist = Preferences.Default.ContainsKey("firebasetoken");
        if (tokenexist)
        {
            string token = Preferences.Get("firebasetoken", string.Empty);
            System.Diagnostics.Debug.WriteLine($"[Token]-------------------------------------------Token: {token}");
        }
    }

    private void CreateNotificationChannel()
    {
        if(OperatingSystem.IsOSPlatformVersionAtLeast("android", 26))
        {
            var channel = new NotificationChannel(ChannelID, "Test Notification", NotificationImportance.Default);

            var notificationManager = (NotificationManager)GetSystemService(Android.Content.Context.NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }
    }
}
