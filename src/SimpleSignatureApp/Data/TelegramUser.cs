namespace SimpleSignatureApp.Data;

public class TelegramUser
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string PhotoUrl { get; set; }
    public string AuthDate { get; set; }
    public string Hash { get; set; }

    public TelegramUser()
    {
    }

    public TelegramUser(IDictionary<string, string> data)
    {
    }
}