using System.Diagnostics;

namespace API.Services
{
    public class CloudMailService: IMailService
    {
        private string _mailTo = Startup.Configuration["mailSettings:mailToAddr"];
        private string _mailFrom = Startup.Configuration["mailSettings:mailFromAddr"];

        public void Send(string subject, string message) {
            Debug.WriteLine($"CLOUD Mail from {_mailFrom} to {_mailTo}, with LocalMailService");
            Debug.WriteLine($"CLOUD Subject: {subject}");
            Debug.WriteLine($"CLOUD Message: {message}");
        }
    }
}