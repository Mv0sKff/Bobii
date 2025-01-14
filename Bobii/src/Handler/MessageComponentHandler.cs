﻿using Discord;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bobii.src.Handler
{
    class MessageComponentHandlingService
    {
        public static async Task MessageComponentHandler(SocketInteraction interaction, DiscordSocketClient client)
        {
            var parsedArg = (SocketMessageComponent)interaction;
            var parsedUser = (SocketGuildUser)interaction.User;
            //If SelectMenu
            if (parsedArg.Data.Values != null)
            {
                var commandName = parsedArg.Data.Values.First<string>();

                switch (commandName)
                {
                    case "temp-channel-help-selectmenuoption":
                        await parsedArg.Message.ModifyAsync(msg => msg.Embeds = new Embed[] { 
                            Bobii.Helper.CreateEmbed(interaction, TempChannel.Helper.HelpTempChannelInfoPart(client.Rest.GetGlobalApplicationCommands().Result).Result +
                            TempChannel.Helper.HelpEditTempChannelInfoPart(client.Rest.GetGlobalApplicationCommands().Result).Result, "Temporary Voice Channel Commands:").Result });
                        await Handler.HandlingService._bobiiHelper.WriteToConsol("MessageCom", false, "MessageComponentHandler, Help", new Entities.SlashCommandParameter() { Guild = parsedUser.Guild, GuildUser = parsedUser},
                            message: "Temp channel help was chosen", hilfeSection: "Temp Channel");
                        await parsedArg.DeferAsync();
                        break;
                    case "filter-word-help-selectmenuoption":
                        await parsedArg.Message.ModifyAsync(msg => msg.Embeds = new Embed[] { Bobii.Helper.CreateEmbed(interaction, FilterWord.Helper.HelpFilterWordInfoPart(client.Rest.GetGlobalApplicationCommands().Result).Result, "Filter Word Commands:").Result });
                        await Handler.HandlingService._bobiiHelper.WriteToConsol("MessageCom", false, "MessageComponentHandler, Help", new Entities.SlashCommandParameter() { Guild = parsedUser.Guild, GuildUser = parsedUser },
                            message: "Filter word help was chosen", hilfeSection: "Filter Word");
                        await parsedArg.DeferAsync();
                        break;
                    case "how-to-cereate-temp-channel-guide":
                        await parsedArg.Message.ModifyAsync(msg => msg.Embeds = new Embed[] { Bobii.Helper.CreateEmbed(interaction, "Click [here](https://www.youtube.com/watch?v=W15u-wk9j-g) to open the YouTube video.", "YouTube Tutorial on how to manage create-temp-channels").Result });
                        await Handler.HandlingService._bobiiHelper.WriteToConsol("MessageCom", false, "MessageComponentHandler, Guide", new Entities.SlashCommandParameter() { Guild = parsedUser.Guild, GuildUser = parsedUser },
                             message: "temp-channel guide was chosen", hilfeSection: "Temp Channel");
                        await parsedArg.DeferAsync();
                        break;
                    case "how-to-add-filter-link-guide":
                        await parsedArg.Message.ModifyAsync(msg => msg.Embeds = new Embed[] { Bobii.Helper.CreateEmbed(interaction, FilterLink.Guides.StepByStepFLLAdd().Result, "Step by step instruction on how to add a link to the whitelist").Result });
                        await Handler.HandlingService._bobiiHelper.WriteToConsol("MessageCom", false, "MessageComponentHandler, Guide", new Entities.SlashCommandParameter() { Guild = parsedUser.Guild, GuildUser = parsedUser },
                            message: "/flladd guide was chosen", hilfeSection: "Filter Link");
                        await parsedArg.DeferAsync();
                        break;
                    case "filter-link-help-selectmenuotion":
                        await parsedArg.Message.ModifyAsync(msg => msg.Embeds = new Embed[] { Bobii.Helper.CreateEmbed(interaction, FilterLink.Helper.HelpFilterLinkInfoPart(client.Rest.GetGlobalApplicationCommands().Result).Result, "Filter Link Commands:").Result });
                        await Handler.HandlingService._bobiiHelper.WriteToConsol("MessageCom", false, "MessageComponentHandler, Help", new Entities.SlashCommandParameter() { Guild = parsedUser.Guild, GuildUser = parsedUser },
                            message: "Filter link help was chosen", hilfeSection: "Filter Link");
                        await parsedArg.DeferAsync();
                        break;
                    case "support-help-selectmenuotion":
                        await parsedArg.Message.ModifyAsync(msg => msg.Embeds = new Embed[] { Bobii.Helper.CreateEmbed(interaction, Bobii.Helper.HelpSupportPart().Result, "Support:").Result });
                        await Handler.HandlingService._bobiiHelper.WriteToConsol("MessageCom", false, "MessageComponentHandler, Help", new Entities.SlashCommandParameter() { Guild = parsedUser.Guild, GuildUser = parsedUser },
                            message: "Support help was chosen", hilfeSection: "Support");
                        await parsedArg.DeferAsync();
                        break;
                    case "text-utility-help-selectmenuotion":
                        await parsedArg.Message.ModifyAsync(msg => msg.Embeds = new Embed[] { Bobii.Helper.CreateEmbed(interaction, TextUtility.Helper.HelpTextUtilityInfoPart(client.Rest.GetGlobalApplicationCommands().Result).Result +
                           "\n\n" + StealEmoji.Helper.HelpSteaEmojiInfoPart(client.Rest.GetGlobalApplicationCommands().Result).Result, "Text Utility Commands:", false).Result });
                        await Handler.HandlingService._bobiiHelper.WriteToConsol("MessageCom", false, "MessageComponentHandler, Help", new Entities.SlashCommandParameter() { Guild = parsedUser.Guild, GuildUser = parsedUser },
                            message: "Text Utility help was chosen", hilfeSection: "Text Utility");
                        await parsedArg.DeferAsync();
                        break;
                    case "w2g-help-selectmenuoption":
                        await parsedArg.Message.ModifyAsync(msg => msg.Embeds = new Embed[] { Bobii.Helper.CreateEmbed(interaction, Watch2Gether.Helper.HelpW2GInfoPart(client.Rest.GetGlobalApplicationCommands().Result).Result, "Watch 2 Gether Command:", false).Result });
                        await Handler.HandlingService._bobiiHelper.WriteToConsol("MessageCom", false, "MessageComponentHandler, Help", new Entities.SlashCommandParameter() { Guild = parsedUser.Guild, GuildUser = parsedUser },
                            message: "W2G help was chosen", hilfeSection: "Watch2Gether");
                        await parsedArg.DeferAsync();
                        break;
                }
            }
            //Button
            else
            {
                try
                {
                    switch (parsedArg.Data.CustomId)
                    {
                        case "gostupid-button":
                            await interaction.FollowupAsync("", new Embed[] { Bobii.Helper.CreateEmbed(interaction, $"{parsedArg.User.Username} went stupid", "").Result });
                            break;
                        case "wtog-button":
                            await interaction.DeferAsync();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }

        }
    }
}
