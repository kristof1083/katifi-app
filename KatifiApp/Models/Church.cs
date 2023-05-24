namespace KatifiApp.Models;

public class Church
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Vicar { get; set; }
    public string ImageUrl { get; set; }
    public Address Address { get; set; }
}
