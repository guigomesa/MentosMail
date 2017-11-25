namespace MentosMailCore
{
    public interface ISmtpServerConf
    {
        string Host { get; set; }
        int Port { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        bool UseSsl { get; set; }
        bool UseDefaultCredential { get; set; }
    }

    public class SmtpServerConf : ISmtpServerConf
    {
        public string Host { get; set; }
        public int Port { get; set; } = 587;
        public string Username { get; set; }
        public string Password { get; set; }
        public bool UseSsl { get; set; }
        public bool UseDefaultCredential { get; set; }
    }
}