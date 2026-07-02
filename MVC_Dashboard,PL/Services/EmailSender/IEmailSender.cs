using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MVC_Dashboard_PL.Services.SendEmail
{
    public interface IEmailSender
    {
        public Task SendEmailAsync(string From, string Recipients, string Subject, string Body);

        
    }
}
