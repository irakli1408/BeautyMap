using BeautyMap.Common.CurrentState;
using Microsoft.Extensions.DependencyInjection;

namespace BeautyMap.Common.Tools.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCommon(this IServiceCollection services)
        {
            services.AddScoped<ICurrentStateService, CurrentStateService>();
        }
    }
}
