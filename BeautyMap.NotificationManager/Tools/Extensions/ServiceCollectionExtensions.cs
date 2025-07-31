using BeautyMap.NotificationManager.Interfaces;
using BeautyMap.NotificationManager.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BeautyMap.NotificationManager.Tools.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddNotificationManager(this IServiceCollection services)
        {
            services.AddScoped<ISendNotification, SendNotification>();
        }
    }
}
