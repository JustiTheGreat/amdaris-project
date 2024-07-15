namespace AmdarisProject.Application.Options
{
    public class SmtpSettings
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required int Port { get; set; }
    }
}
