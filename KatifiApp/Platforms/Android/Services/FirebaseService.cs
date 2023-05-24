using Android.App;
using Android.Content;
using AndroidX.Core.App;
using Firebase.Messaging;
using KatifiApp.Models;
using Newtonsoft.Json;
using System.Text;

namespace KatifiApp.Platforms.Android.Services
{
    [Service(Exported = true)]
    [IntentFilter(new[] {"com.google.firebase.MESSAGING_EVENT"} )]
    public class FirebaseService : FirebaseMessagingService
    {

        public FirebaseService() { }

        //Getting Message
        public override void OnNewToken(string token)
        {
            base.OnNewToken(token);
            SaveFirebaseToken(token);
        }

        public override void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);

            var notification = message.GetNotification();
            this.ShowNotification(notification.Body, notification.Title, message.Data);
        }

        private void ShowNotification(string messageBody, string title, IDictionary<string, string> data)
        {
            var notificationBuilder = new NotificationCompat.Builder(this, MainActivity.ChannelID)
                .SetContentTitle(title)
                .SetSmallIcon(Resource.Mipmap.appicon)
                .SetContentText(messageBody)
                .SetChannelId(MainActivity.ChannelID)
                .SetPriority(2);

            var notificationManager = NotificationManagerCompat.From(this);
            notificationManager.Notify(MainActivity.NotificationID, notificationBuilder.Build());
        }

        //Sending Message
        public static async Task<bool> SendPushNotification(string title, string messageBody, IDictionary<string, string> data = null)
        {
            var notificationObject = data;
            string firebasetoken = Preferences.Default.Get("firebasetoken", string.Empty);
            System.Diagnostics.Debug.WriteLine($"[Token]---------------Token: {firebasetoken}");

            var pushNotificationRequest = new PushNotificationRequest
            {
                registration_ids = new List<string> { firebasetoken }
,
                notification = new NotificationMessageBody
                {
                    title = title,
                    body = messageBody
                }
            };

            if (data is not null)
                pushNotificationRequest.data = notificationObject;


            string url = "https://fcm.googleapis.com/fcm/send";
            string serverKey = "AAAAamUmpac:APA91bGDDFYbBO6SQ1C0eRkv7BzaBvxJiGIIilWp_XGJMDlplENeJpCZMMl4mIfHAlJNQf_rqUCssVNovS0YCCBgsKPlpKp8aeQJLuYwstUXregyxsSzgMKVCQK1Yan-HoMP_kKMzotR";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("key", "=" + serverKey);

                string serializeRequest = JsonConvert.SerializeObject(pushNotificationRequest);
                var response = await client.PostAsync(url, new StringContent(serializeRequest, Encoding.UTF8, "application/json"));
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return true;
                }
            }
            return false;
        }

        //private helper methods
        private static async void SaveFirebaseToken(string token)
        {
            Preferences.Default.Set("firebasetoken", token);
            await SecureStorage.Default.SetAsync("firebasetoken", token);
        }
    }
}
