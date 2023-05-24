using KatifiApp.Models;

namespace KatifiWebServer.Models.SecurityModels
{
    public class LoggedInModel
    {
        public string JwtToken { get; set; }
        public User LoggedInUser { get; set; }
    }
}
