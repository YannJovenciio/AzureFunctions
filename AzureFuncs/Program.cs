using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AzureFunctions.UserInterface;
using AzureFunctions.UserService;
using Microsoft.EntityFrameworkCore;
using AzureFunctions.DbContexts;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

    builder.Services.AddScoped<IUserService, UserService>();

    builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseNpgsql("Host=localhost;Database=TreinoDatabase;Username=postgres;Password=123"));

builder.Build().Run();
