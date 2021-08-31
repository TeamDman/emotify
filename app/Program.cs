using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Emotify.Models;
using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Emotify
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var discord = host.Services.GetService<DiscordSocketClient>();
            var discordLogger = host.Services.GetService<ILogger<IDiscordClient>>();
            discord.Log += msg =>
            {
                discordLogger.LogInformation(msg.ToString());
                return Task.CompletedTask;
            };

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    // todo: make async, add content
                    SeedData.Initialize(services);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel();
                    // webBuilder.UseUrls("http://localhost:80","http://192.168.3.51:80", "https://localhost:443","https://192.168.3.51:443");
                    webBuilder.UseStartup<Startup>();
                });
    }
}