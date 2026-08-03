namespace Infrastructure.Email;

public sealed class EmailOptions
{
    public string Host { get; set; }
    public int Port { get; set; } = 587;
    public string Username { get; set; }
    public string Password { get; set; }
    public string ConfirmEmailUrl { get; set; }
    public string ResetPasswordUrl { get; set; }
}
