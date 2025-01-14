﻿using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using Newtonsoft.Json;
using System.IO;
using Bobii.src.Handler;
using Npgsql;
using Bobii.src.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Bobii.src.EntityFramework.Entities;

namespace Bobii
{
    public class Program
    {
        #region Methods
        public static void Main(string[] args)
        => new Program().MainAsync().GetAwaiter().GetResult();

        public static async Task SetBotStatusAsync(DiscordSocketClient client)
        {
            await client.SetActivityAsync(new Game("/helpbobii", ActivityType.Listening));
            await client.SetStatusAsync(UserStatus.Online);
            Console.WriteLine($"{DateTime.Now.TimeOfDay:hh\\:mm\\:ss} Bobii       Status was set sucessfully");
        }
        #endregion

        #region Functions 
        public async Task MainAsync()
        {
            // Doing migrations if there are some to do
            using (var context = new BobiiEntities())
            {
                context.Database.Migrate();
            }

            JObject config = GetConfig();
            string token = config["BobiiConfig"][0]["token"].Value<string>();

            using var services = ConfigureServices();

            var client = services.GetRequiredService<DiscordSocketClient>();
            client.Log += Log;
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            // §TODO JG/31.07.2021 Schauen wie ich das hier besser hinbekomme...
            var handlingService = new HandlingService(services);

            await Task.Delay(-1);
        }

        public static ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
                {
                    MessageCacheSize = 500,
                    LogLevel = LogSeverity.Critical,
                    GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.GuildPresences | GatewayIntents.GuildMembers,
                    AlwaysDownloadUsers = true
                }))
                .AddSingleton(new CommandService(new CommandServiceConfig
                {
                    LogLevel = LogSeverity.Info,
                    DefaultRunMode = RunMode.Async,
                    CaseSensitiveCommands = false
                }))
                .AddSingleton<HandlingService>()
                .BuildServiceProvider();
        }

        public static JObject GetConfig()
        {
            using StreamReader configJson = new StreamReader(Directory.GetCurrentDirectory() + @"/Config.json");
            return (JObject)JsonConvert.DeserializeObject(configJson.ReadToEnd());
        }

        public static Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());

            return Task.CompletedTask;
        }
        #endregion
    }
}
