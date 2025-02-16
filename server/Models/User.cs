namespace Models;

public class User(int id, string username, string password)
{
    public User() : this(0, string.Empty, string.Empty) { }

    public int Id { get; set; } = id;
    public string Username { get; set; } = username;
    public string Password { get; set; } = password;
    // Add other properties as needed, for example:
    // public string Email { get; set; }
    // public DateTime CreatedAt { get; set; }
}