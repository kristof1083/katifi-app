using KatifiApp.Models;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging;
using KatifiApp.Views;
#if ANDROID
using KatifiApp.Platforms.Android.Services;
#endif

namespace KatifiApp.Services;

public class EventService : BaseService, IEventService
{
    List<Event> _events = new();
    List<Event> _myEvents = new();
    string jwttoken;
    string myid;

    readonly ISecureStorage _secureStorage;
    readonly IFilePicker _filePicker;
    readonly IBrowser _browser;

    public EventService(ISecureStorage secureStorage, IFilePicker filePicker, IBrowser browser)
    {
        _secureStorage = secureStorage;
        _filePicker = filePicker;
        _browser = browser;
        _httpClient.MaxResponseContentBufferSize = 134217728;
        this.GetSecureProperties();
    }

    private async void GetSecureProperties()
    {
        this.jwttoken = await _secureStorage.GetAsync("jwttoken");
        this.myid = await _secureStorage.GetAsync("myid");
    }

    public async Task<List<Event>> GetEvents()
    {
        var response = await _httpClient.GetAsync("/api/event");

        if (response.IsSuccessStatusCode)
        {
            _events = await response.Content.ReadFromJsonAsync<List<Event>>();
        }

        return _events;
    }

    public async Task<List<Event>> GetMyEvents()
    {
        this.GetSecureProperties();
        if(jwttoken is null)
            return _myEvents;

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwttoken);
        var response = await _httpClient.GetAsync($"/api/event/my-events?userId={MyId}");

        if (response.IsSuccessStatusCode)
        {
            _myEvents = await response.Content.ReadFromJsonAsync<List<Event>>();
        }

        return _myEvents;
    }

    public async Task<bool> RegistUserForEventAsync(int eventId)
    {
        this.GetSecureProperties();
        if (jwttoken is null || myid is null)
            return false;

        if(_httpClient.DefaultRequestHeaders.Authorization is null)
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwttoken);

        string json = JsonSerializer.Serialize(myid);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"/api/event/{eventId}/regist", content);

        if (response.IsSuccessStatusCode)
        {
            return true;
        }

        return false;
    }

    public async Task<bool> DeleteUserFromEvent(int eventId)
    {
        this.GetSecureProperties();
        if (jwttoken is null || myid is null)
            return false;

        if (_httpClient.DefaultRequestHeaders.Authorization is null)
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwttoken);

        var response = await _httpClient.DeleteAsync($"/api/event/{eventId}/cancel-registration?userId={myid}");

        if (response.IsSuccessStatusCode)
        {
            return true;
        }

        return false;
    }

    public async Task<bool> UploadImageFiles(int eventId)
    {
        this.GetSecureProperties();

        var result = await _filePicker.PickAsync(new PickOptions
        {
            FileTypes = FilePickerFileType.Images,
            PickerTitle = "Please select an image file"
        });

        if (result is null)
            return false;

        if (jwttoken is null || myid is null)
            return false;

        if (_httpClient.DefaultRequestHeaders.Authorization is null)
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwttoken);;

        var form = new MultipartFormDataContent();
        var streamcontent = new StreamContent(await result.OpenReadAsync());
        streamcontent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");

        form.Add(streamcontent, "file", result.FileName);
        form.Add(new StringContent(myid), "userId");

        var response = await _httpClient.PostAsync($"/api/event/{eventId}/upload-photo?userId={myid}", form);

        if (response.IsSuccessStatusCode)
            return true;

        return false;
    }

    public bool AddToLocalCalendar(Event varevent)
    {
#if ANDROID
        return CalendarService.SendCalendarRequest(varevent);
#else
        return false;
#endif
    }

    public async Task<bool> AddToGoogleCalendar(Event varevent)
    {
        this.GetSecureProperties();

        if (jwttoken is null || myid is null)
            return false;

        if (_httpClient.DefaultRequestHeaders.Authorization is null)
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwttoken);

        var response = await _httpClient.GetAsync("/api/google/oauth");

        if (response.IsSuccessStatusCode)
        {
            string url = await response.Content.ReadAsStringAsync();

            bool success = await _browser.OpenAsync(url, BrowserLaunchMode.SystemPreferred);
            Thread.Sleep(10000);

            string json = JsonSerializer.Serialize(varevent);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response2 = await _httpClient.PostAsync("api/google/create-event", content);
            if (response2.IsSuccessStatusCode && response.IsSuccessStatusCode)
                return true;
        }

        return false;
    }
}
