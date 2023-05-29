namespace KatifiApp.Models;

public class Address
{
    public int Id { get; set; }
    public string CountryCode { get; set; }
    public string County { get; set; }
    public int? PostCode { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public int? HouseNumber { get; set; }

    public override string ToString()
    {
        if (PostCode is null && Street is null && HouseNumber is null)
        {
            return $"{CountryCode} {County}, {City}";
        }

        return $"{CountryCode} {PostCode} {City}, {Street} {HouseNumber}";
    }
}
