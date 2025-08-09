using BeautyMap.Application.Persistence;
using BeautyMap.Application.Tools.Managers.Languages;
using BeautyMap.Common.Enums;
using BeautyMap.Domain.Entities.Account;
using BeautyMap.Domain.Entities.Notifications;
using BeautyMap.NotificationManager.Enums;
using BeautyMap.NotificationManager.Interfaces;
using BeautyMap.NotificationManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BeautyMap.Application.Tools.Managers.Notifications
{
    public class NotificationBuilder : INotificationBuilder
    {
        private readonly IBeautyMapDbContext _db;
        private readonly ILanguageService _languageService;
        private readonly ISendNotification _sendNotification;
        private readonly IConfiguration _configuration;
        public NotificationBuilder(IBeautyMapDbContext db, ILanguageService languageService, ISendNotification sendNotification, IConfiguration configuration)
        {
            _db = db;
            _languageService = languageService;
            _sendNotification = sendNotification;
            _configuration = configuration;
        }
        public async Task BuildNotificationAsync(UserEntity user, SendNotificationTypes type, string text)
        {
            var notification = await GetNotification(type);

            text = type switch
            {
                SendNotificationTypes.ResetPassword => GenerateUrl("reset-password-confirmation", text),
                SendNotificationTypes.SetAdminPassword => GenerateUrl("reset-password-confirmation", text),
                SendNotificationTypes.NotifyAdminAboutPayment => GenerateUrl("admin/get-order-details", text),
                SendNotificationTypes.NotifyUserAboutPayment => GenerateUrl("order/get-order-details", text),
                _ => text
            };

            await _sendNotification.SendNotificationAsync(user,
                new NotificationModel
                {
                    Type = (SendNotificationTypes)notification.NotificationTypeId,
                    Title = notification.Title,
                    Body = notification.Body
                },
                text);
        }

        private async Task<NotificationLocale> GetNotification(SendNotificationTypes type)
        {
            var languageId = _languageService.GetPreferredLanguageId();

            var notification = await _db.NotificationLocales
                .FirstOrDefaultAsync(x => x.NotificationTypeId == (int)type && x.LanguageId == languageId);

            notification ??= await _db.NotificationLocales
                    .FirstOrDefaultAsync(x => x.NotificationTypeId == (int)type && x.LanguageId == (int)LanguageType.Georgian);

            if (notification == null)
                throw new Exception("Notification Type was not found");

            return notification;
        }

        private string GenerateUrl(string path, string text)
        {
            var baseUrl = _configuration["BaseUrl"];
            return $"{baseUrl}{path}/{text}";
        }
    }
}
