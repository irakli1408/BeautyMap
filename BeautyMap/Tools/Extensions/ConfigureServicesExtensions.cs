using Asp.Versioning;
using AspNetCoreRateLimit;
using BeautyMap.API.Settings;
using BeautyMap.Common.Tools.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json;

namespace BeautyMap.API.Tools.Extensions
{
    public static class ConfigureServicesExtensions
    {
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            var environment = builder.Environment;
            builder.Services.AddRouting(options => options.LowercaseUrls = true);
            builder.Services.AddHttpContextAccessor();

            //builder.Services.AddIdentity<UserEntity, Role>(opts =>
            //{
            //    opts.Password.RequireDigit = true;
            //    opts.Password.RequireLowercase = true;
            //    opts.Password.RequireUppercase = true;
            //    opts.Password.RequireNonAlphanumeric = false;
            //    opts.Password.RequiredLength = 8;
            //})
            //.AddRoles<Role>()
            //.AddEntityFrameworkStores<BlogLikeDbContext>()
            //.AddDefaultTokenProviders();

            //ToDo: Add RateLimiter after 
            builder.Services.AddRateLimiter();
            builder.Services.AddApiVersioningOptions();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwagger();

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.WriteIndented = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                }).ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

            builder.Services.AddCommon();
            //builder.Services.AddApplication();
            //builder.Services.AddPersistence(builder.Configuration, environment.IsDevelopment());

        }

        private static void GlobalParallelExceptionHooks()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                var ex = (Exception)args.ExceptionObject;
                Console.WriteLine($"[AppDomain] Fatal exception: {ex}");
            };

            TaskScheduler.UnobservedTaskException += (sender, args) =>
            {
                Console.WriteLine($"[TaskScheduler] Background task exception: {args.Exception}");
                args.SetObserved(); // Optional but recommended to prevent app domain crash
            };
        }

        private static void AddRateLimiter(this IServiceCollection services)
        {
            services.AddOptions();
            services.AddMemoryCache();

            services.Configure<IpRateLimitOptions>(options =>
            {
                options.EnableEndpointRateLimiting = true;
                options.StackBlockedRequests = false;
                options.RealIpHeader = "X-Real-IP";
                options.ClientIdHeader = "X-ClientId";
                options.HttpStatusCode = 429;

                options.GeneralRules =
                [
                    new RateLimitRule
                    {
                        Endpoint = "*",
                        Limit = 10,
                        Period = "1m"
                    }
                ];
            });

            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IClientPolicyStore, MemoryCacheClientPolicyStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        }

        private static void AddApiVersioningOptions(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("X-Api-Version")
                );
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            })
            .AddMvc();
        }

        private static void AddSwagger(this IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfiguration>();

            services.AddSwaggerGen(options =>
            {
                options.UseInlineDefinitionsForEnums();

                options.CustomSchemaIds(type => type.ToString());

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization Header"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference{
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }
    }
}
