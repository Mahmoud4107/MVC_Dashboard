using Microsoft.Extensions.Configuration;
using MVC_Dashboard_PL.Services.SendEmail;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MVC_Dashboard_PL.Services.EmailSender
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string From, string Recipients, string Subject, string Body)
        {
            // Generate Email

            var EmailMessage = new MailMessage();

            EmailMessage.From = new MailAddress(From);
            EmailMessage.To.Add(Recipients);
            EmailMessage.Subject = Subject;
            EmailMessage.Body = $@"
                                    <!DOCTYPE html>
                                    <html>
                                    <head>
                                        <meta charset='UTF-8'>
                                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                                    </head>
                                    <body style='margin:0;padding:0;background-color:#f4f6f9;font-family:Arial,Helvetica,sans-serif;'>

                                        <table width='100%' cellpadding='0' cellspacing='0' style='background:#f4f6f9;padding:40px 0;'>
                                            <tr>
                                                <td align='center'>

                                                    <table width='600' cellpadding='0' cellspacing='0'
                                                           style='background:#ffffff;border-radius:12px;overflow:hidden;
                                                                  box-shadow:0 5px 15px rgba(0,0,0,.08);'>

                                                        <!-- Header -->
                                                        <tr>
                                                            <td align='center'
                                                                style='background:#0d6efd;padding:30px;color:#ffffff;'>

                                                                <h1 style='margin:0;font-size:28px;'>
                                                                    MVC Dashboard
                                                                </h1>

                                                                <p style='margin-top:8px;font-size:16px;color:#dfe9ff;'>
                                                                    Secure Account Notification
                                                                </p>

                                                            </td>
                                                        </tr>

                                                        <!-- Body -->
                                                        <tr>
                                                            <td style='padding:40px;'>

                                                                {Body}

                                                            </td>
                                                        </tr>

                                                        <!-- Divider -->
                                                        <tr>
                                                            <td style='padding:0 40px;'>
                                                                <hr style='border:none;border-top:1px solid #eeeeee;'>
                                                            </td>
                                                        </tr>

                                                        <!-- Footer -->
                                                        <tr>
                                                            <td align='center'
                                                                style='padding:25px;font-size:13px;color:#888;'>

                                                                <p style='margin:0;'>
                                                                    This is an automated email. Please do not reply.
                                                                </p>

                                                                <p style='margin-top:10px;'>
                                                                    &copy; @DateTime.Now.Year MVC Dashboard. All rights reserved.
                                                                </p>

                                                            </td>
                                                        </tr>

                                                    </table>

                                                </td>
                                            </tr>
                                        </table>

                                    </body>
                                    </html>";
            EmailMessage.IsBodyHtml = true;

            // Connect

            var SenderEmail = _configuration["EmailSender:Email"];
            var SenderPassword = _configuration["EmailSender:Password"];

            var SmtpClient = new SmtpClient(_configuration["EmailSender:Host"], int.Parse(_configuration["EmailSender:Port"]))
            {
                Credentials = new NetworkCredential(SenderEmail, SenderPassword),
                EnableSsl = true
            };
            await SmtpClient.SendMailAsync(EmailMessage);
        }
    }
}
