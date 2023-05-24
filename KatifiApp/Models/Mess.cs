namespace KatifiApp.Models
{
    public class Mess
    {
        public int Id { get; set; }
        public string Day { get; set; }

        public TimeOnly StartTime { get; set; }

        public string Ocasion { get; set; }

        public string Priest { get; set; }

        public int ChurchId { get; set; }
    }
}
