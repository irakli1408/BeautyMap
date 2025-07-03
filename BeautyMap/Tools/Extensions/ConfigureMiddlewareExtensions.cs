using Asp.Versioning.ApiExplorer;
using AspNetCoreRateLimit;
using BeautyMap.Common.ErrorHandler.Middlware;
using System.Reflection;

namespace BeautyMap.API.Tools.Extensions
{
    public static class ConfigureMiddlewareExtensions
    {
        private static readonly string PathBase = Environment.GetEnvironmentVariable("ASPNETCORE_APPL_PATH");
        public static void ConfigureMiddleware(this WebApplication app, Assembly assembly)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseIpRateLimiting();
            app.UsePathBase();
            app.UseCors(options =>
                options.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
            );

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger(assembly);
            }
            app.UseEndpoints(app.Environment.IsProduction());
        }

        private static void UseSwagger(this WebApplication app, Assembly assembly)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint(
                        $"/swagger/{description.GroupName}/swagger.json",
                        $"BeautyMap API {description.GroupName}");
                }
            });
        }
        private static void UsePathBase(this IApplicationBuilder app)
        {
            if (!string.IsNullOrWhiteSpace(PathBase))
            {
                app.UsePathBase($"/{PathBase}");
            }
        }

        private static void UseEndpoints(this IApplicationBuilder app, bool isProduction)
        {
            //ToDo: isProduction 
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //ToDo : If we need to have health check
                //endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
                //{
                //    Predicate = registration => registration.Name.Contains("self")
                //});

                //endpoints.MapHealthChecks("/hc", new HealthCheckOptions
                //{
                //    Predicate = _ => true,
                //});

                endpoints.MapGet("/", context =>
                {
                    if (!isProduction)
                    {
                        context.Response.Redirect($"{(!string.IsNullOrWhiteSpace(PathBase) ? $"/{PathBase}" : string.Empty)}/swagger");

                        return Task.FromResult(0);
                    }

                    return context.Response.WriteAsync("OK");
                });
            });
        }
    }
}
