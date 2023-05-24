namespace KatifiApp.Models;

public class Community
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsOpen { get; set; }
    public string ImageUrl { get; set; }

    public Address Address { get; set; }

    public int MemberCount { get; set; }

}
