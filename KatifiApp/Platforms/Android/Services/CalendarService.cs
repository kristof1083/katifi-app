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

            double startvalue = varevent.Start.ToUniversalTime().Subtract(
                        new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
            double endvalue = varevent.End.ToUniversalTime().Subtract(
                        new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;


            intent.PutExtra(CalendarContract.Events.InterfaceConsts.Title, varevent.Name);
            intent.PutExtra(CalendarContract.Events.InterfaceConsts.Dtstart, startvalue);
            intent.PutExtra(CalendarContract.Events.InterfaceConsts.Dtend, endvalue);
            intent.PutExtra(CalendarContract.Events.InterfaceConsts.CalendarTimeZone, "Europe/Budapest");
            intent.PutExtra(CalendarContract.Events.InterfaceConsts.Organizer, varevent.Organizer);
            intent.PutExtra(CalendarContract.Events.InterfaceConsts.EventLocation, varevent.Address.ToString());
            intent.PutExtra(CalendarContract.Events.InterfaceConsts.Description, description);
            intent.PutExtra(CalendarContract.Events.InterfaceConsts.AllDay, false);
            
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
