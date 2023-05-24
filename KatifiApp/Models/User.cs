namespace KatifiApp.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Lastname { get; set; }

        public string FirstName { get; set; }

        public char Gender { get; set; }

        public DateOnly BornDate { get; set; }

        public string Email { get; set; }

        public string ProfileImageUrl { get; set; }

        public bool AgreeTerm { get; set; }

        public Address Address { get; set; }
    }
}
