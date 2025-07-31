using BeautyMap.Domain.Entities.Account;
using BeautyMap.NotificationManager.Enums;
using BeautyMap.NotificationManager.Interfaces;
using BeautyMap.NotificationManager.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;

namespace BeautyMap.NotificationManager.Services
{
    public class SendNotification : ISendNotification
    {
        private readonly IConfiguration configuration;

        private string title;
        private string body;
        public SendNotification(IConfiguration configuration)
            => this.configuration = configuration;

        public async Task SendNotificationAsync(UserEntity user, NotificationModel notification, string text)
        {
            title = notification.Title;
            body = notification.Body;

            switch (notification.Type)
            {
                case SendNotificationTypes.ResetPassword:
                    await SendEmailAsync(new SendModel(user,
                        title,
                        string.Format(body, user.FirstName, user.LastName, text)
                    ));
                    break;
                case SendNotificationTypes.SetAdminPassword:
                    await SendEmailAsync(new SendModel(user,
                        title,
                        string.Format(body, text)
                    ));
                    break;
                case SendNotificationTypes.ConfirmEmail:
                    await SendEmailAsync(new SendModel(user,
                        title,
                        string.Format(body, text)
                    ));
                    break;
                case SendNotificationTypes.AccountIsBlocked:
                    await SendEmailAsync(new SendModel(user,
                        title,
                        string.Format(body, text)));
                    break;
                default:
                    throw new Exception("Unsuported Email type");
            }
        }

        #region Private

        private async Task SendEmailAsync(SendModel sendModel)
        {
            await SendEmailAsyncInternal(sendModel.Email, sendModel.Subject, sendModel.Body);
        }
        private async Task SendEmailAsyncInternal(string to, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(configuration["NotificationSettings:SMTPUserName"]));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Plain) { Text = body };

            await SendEmailAsyncInternal(email);
        }
        private async Task SendEmailAsyncInternal(MimeMessage email)
        {
            using var smtp = new SmtpClient();

            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

            smtp.Authenticate(configuration["NotificationSettings:SMTPUserName"], configuration["NotificationSettings:SMTPPassword"]);

            await smtp.SendAsync(email);

            smtp.Disconnect(true);
        }

        #endregion
    }
}
