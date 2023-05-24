using KatifiApp.Models;

namespace KatifiApp.Services;

public interface IEventService
{
    public Task<List<Event>> GetEvents();
    public Task<List<Event>> GetMyEvents();
    public Task<bool> RegistUserForEventAsync(int eventId);
    public Task<bool> DeleteUserFromEvent(int eventId);
    public Task<bool> UploadImageFiles(int eventId);
    public bool AddToLocalCalendar(Event varevent);
    public Task<bool> AddToGoogleCalendar(Event varevent);
}
