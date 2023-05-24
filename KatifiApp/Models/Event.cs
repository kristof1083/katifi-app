namespace KatifiApp.Models;

public class Event
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public DateTime RegistrationDeadline { get; set; }
    public int Fee { get; set; }
    public string ImageUrl { get; set; }
    public string Organizer { get; set; }
    public int? MaxParticipant { get; set; }

    public Address Address { get; set; }
    public int ParticipantCount { get; set; }
}
