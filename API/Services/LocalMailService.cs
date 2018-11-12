using System.Diagnostics;

namespace API.Services
{
    public class LocalMailService : IMailService
    {
        private string _mailTo = Startup.Configuration["mailSettings:mailToAddr"];
        private string _mailFrom = Startup.Configuration["mailSettings:mailFromAddr"];

        public void Send(string subject, string message) {
            Debug.WriteLine("********");
            Debug.WriteLine($"Mail from {_mailFrom} to {_mailTo}, with LocalMailService");
            Debug.WriteLine($"Subject: {subject}");
            Debug.WriteLine($"Message: {message}");
            Debug.WriteLine("********");
        }
    }
}