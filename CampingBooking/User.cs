namespace CampingBooking
{
    public enum UserRole { Guest, Admin }

    public class User
    {
        public string Username { get; set; }
        public UserRole Role { get; set; }

        public User(string username, UserRole role)
        {
            Username = username;
            Role = role;
        }
    }
}
