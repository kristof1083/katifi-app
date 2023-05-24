using Android.App;
using Android.Content;
using Android.Provider;

namespace KatifiApp.Platforms.Android.Services
{
    [Service(Exported = true)]
    public class CalendarService
    {
        public CalendarService() { }

        public static bool SendCalendarRequest(KatifiApp.Models.Event varevent)
        {
            Intent intent = new Intent(Intent.ActionInsert);
            intent.SetData(CalendarContract.ContentUri);

            string description = $"Organizer: {varevent.Organizer}.{Environment.NewLine}Fee: {varevent.Fee}.{Environment.NewLine}Max participants: {varevent.MaxParticipant}.";

            intent.PutExtra(CalendarContract.Events.InterfaceConsts.Title, varevent.Name);
            intent.PutExtra(CalendarContract.Events.InterfaceConsts.Dtstart, varevent.Start.ToString());
            intent.PutExtra(CalendarContract.Events.InterfaceConsts.Organizer, varevent.Organizer);
            intent.PutExtra(CalendarContract.Events.InterfaceConsts.EventLocation, varevent.Address.ToString());
            intent.PutExtra(CalendarContract.Events.InterfaceConsts.Description, description);
            intent.PutExtra(CalendarContract.Events.InterfaceConsts.AllDay, true);
            
            if(intent.ResolveActivity(Platform.CurrentActivity.PackageManager) != null)
            {
                Platform.CurrentActivity.StartActivity(intent);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
