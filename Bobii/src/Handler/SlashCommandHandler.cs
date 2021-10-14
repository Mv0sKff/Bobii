﻿using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bobii.src.Entities;

namespace Bobii.src.Handler
{
    class SlashCommandHandlingService
    {
        #region Tasks
        public static async Task<List<SocketSlashCommandDataOption>> GetOptions(IReadOnlyCollection<SocketSlashCommandDataOption> options)
        {
            var optionList = new List<SocketSlashCommandDataOption>();
            foreach (var option in options)
            {
                optionList.Add(option);
            }
            await Task.CompletedTask;
            return optionList;
        }

        public static async Task WriteToConsol(string message, ConsoleColor color = ConsoleColor.White)
        {
            Console.WriteLine($"{DateTime.Now.TimeOfDay:hh\\:mm\\:ss} SCommands   {message}", color);
            await Task.CompletedTask;
        }
        #endregion

        #region Handler  
        public static async Task SlashCommandHandler(SocketInteraction interaction, DiscordSocketClient client)
        {
            var parameter = new SlashCommandParameter();
            parameter.SlashCommand = (SocketSlashCommand)interaction;
            parameter.GuildUser = (SocketGuildUser)parameter.SlashCommand.User;
            parameter.Guild = TextChannel.TextChannel.GetGuildWithInteraction(interaction);
            parameter.GuildID = TextChannel.TextChannel.GetGuildWithInteraction(interaction).Id;
            parameter.Interaction = interaction;
            parameter.Client = client;
            parameter.SlashCommandData = parameter.SlashCommand.Data;

            switch (parameter.SlashCommandData.Name)
            {
                case "tcinfo":
                    await TempChannel.SlashCommands.TCInfo(parameter);
                    break;
                case "bobiiguides":
                    await Bobii.SlashCommands.BobiiGuides(parameter);
                    break;
                case "helpbobii":
                    await Bobii.SlashCommands.HelpBobii(parameter);
                    break;
                case "tcadd":
                    await TempChannel.SlashCommands.TCAdd(parameter);
                    break;
                case "tcremove":
                    await TempChannel.SlashCommands.TCRemove(parameter);
                    break;
                case "tcupdate":
                    await TempChannel.SlashCommands.TCUpdate(parameter);
                    break;
                case "comdelete":
                    await ComEdit.SlashCommands.ComDelete(parameter);
                    break;
                case "comdeleteguild":
                    await ComEdit.SlashCommands.ComDeleteGuild(parameter);
                    break;
                case "comregister":
                    await ComEdit.SlashCommands.ComRegister(parameter);
                    break;
                case "fwadd":
                    await FilterWord.SlashCommands.FWAdd(parameter);
                    break;
                case "fwremove":
                    await FilterWord.SlashCommands.FWRemove(parameter);
                    break;
                case "fwupdate":
                    await FilterWord.SlashCommands.FWUpdate(parameter);
                    break;
                case "fwinfo":
                    await FilterWord.SlashCommands.FWInfo(parameter);
                    break;
                case "flinfo":
                    await FilterLink.SlashCommands.FLInfo(parameter);
                    break;
                case "flset":
                    await FilterLink.SlashCommands.FLSet(parameter);
                    break;
                case "flladd":
                    await FilterLink.SlashCommands.FLLAdd(parameter);
                    break;
                case "fllremove":
                    await FilterLink.SlashCommands.FLLRemove(parameter);
                    break;
                case "fluadd":
                    await FilterLink.SlashCommands.FLUAdd(parameter);
                    break;
                case "fluremove":
                    await FilterLink.SlashCommands.FLURemove(parameter);
                    break;
                case "logset":
                    await FilterLink.SlashCommands.LogSet(parameter);
                    break;
                case "logupdate":
                    await FilterLink.SlashCommands.LogUpdate(parameter);
                    break;
                case "logremove":
                    await FilterLink.SlashCommands.LogRemove(parameter);
                    break;
            }
        }
        #endregion
    }
}
