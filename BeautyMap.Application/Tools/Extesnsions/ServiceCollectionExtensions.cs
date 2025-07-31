using BeautyMap.Application.Common.Behaviour;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BeautyMap.Application.Tools.Extesnsions
{
    public static class ServiceCollectionExtensions
    {
        //public static void AddApplication(this IServiceCollection services)
        //{
        //    var assembly = Assembly.GetExecutingAssembly();
        //    services.AddMediatR(cfg =>
        //    {
        //        cfg.RegisterServicesFromAssembly(assembly);
        //    });

        //    services.AddFileManager();
        //    services.AddNotificationManager();
        //    services.AddValidatorsFromAssembly(assembly);
        //    services.AddPaymentManager();
        //    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        //    services.AddScoped<ITokenService, TokenService>();
        //    services.AddScoped<INotificationBuilder, NotificationBuilder>();
        //    services.AddScoped<ILanguageService, LanguageService>();
        //    services.AddScoped<IFilesManager, FilesManager>();
        //}
    }
}
