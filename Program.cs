using Microsoft.Extensions.Hosting;
using NetCord.Hosting.Gateway;
using NetCord.Hosting.Services;
using NetCord.Hosting.Services.ApplicationCommands;
using Microsoft.Extensions.DependencyInjection;
using WildsApi;
using CommandModules;

var builder = Host.CreateApplicationBuilder(args);

var services = builder.Services
    .AddDiscordGateway()
    .AddApplicationCommands()
    .AddSingleton<WildsDocService>()
    .AddHttpClient("wildsapi", (client) => {
        client.BaseAddress = new Uri("https://wilds.mhdb.io");
    });

var host = builder.Build();

// Add commands using minimal APIs
// host.AddSlashCommand("test", "Testing!", () => "Yep I am alive!");

// Add commands from modules
host.AddModules(typeof(Program).Assembly);

// Add handlers to handle the commands
host.UseGatewayEventHandlers();

await host.RunAsync(); 
