using BeautyMap.FileManager.Interfaces;
using BeautyMap.FileManager.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BeautyMap.FileManager.Tools.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddFileManager(this IServiceCollection services)
        {
            services.AddScoped<IFileUploadService, FileUploadService>();
            services.AddScoped<IFileDeleteService, FileDeleteService>();
            services.AddScoped<IFileBulkDeleteService, FileDeleteService>();
            services.AddScoped<IFileCreateService, FileCreateService>();
            services.AddScoped<IFileRetrieveService, FileRetrieveService>();
            services.AddScoped<IFileEditService, FileEditService>();
        }
    }
}
