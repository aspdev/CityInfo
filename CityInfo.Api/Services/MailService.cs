using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace CityInfo.Api.Services
{
    public class MailService : IMailService
    {
        private string _mailTo = Startup.Configuration["mailSettings: mailToAddress"];
            
        private string _mailFrom = Startup.Configuration["mailSettings: mailFromAddress"];

       

        public void Send(string subject, string message)
        {
            // send Email - output to debug window

            Debug.WriteLine($"Mail from {_mailFrom} to {_mailTo}, with Local Mail Service");
            Debug.WriteLine($"Subject: {subject}");
            Debug.WriteLine($"Message: {message}");
        }
    }
}
