using BeautyMap.API.Tools.Extensions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
Assembly assembly = Assembly.GetExecutingAssembly();
builder.ConfigureServices();
var app = builder.Build();

app.ConfigureMiddleware(assembly);

app.Run();