﻿using Discord;
using Discord.Rest;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bobii.src.DMSupport
{
    class Helper
    {
        #region Functions
        private static SocketThreadChannel CheckIfThreadExists(SocketMessage message, SocketTextChannel dmChannel)
        {
            foreach (SocketThreadChannel thread in dmChannel.Threads)
            {
                if (thread.Name == message.Author.Id.ToString())
                {
                    return thread;
                }
            }
            return null;
        }

        private static SocketThreadChannel CreateThread(SocketMessage message, SocketTextChannel dmChannel)
        {
            return dmChannel.CreateThreadAsync(message.Author.Id.ToString()).Result;
        }

        private static Embed CreateDMEmbed(SocketMessage message)
        {
            var pictureUrl = "";
            //foreach (var attachment in message.Attachments)
            //{
            //    //As far as I know its only possible to attach one file so this foreach should be fine
            //    if (attachment.Filename.EndsWith(".jpg") || attachment.Filename.EndsWith(".gif") || attachment.Filename.EndsWith(".png"))
            //    {
            //        pictureUrl = attachment.ProxyUrl;
            //    }
            //    else
            //    {
            //        message.Channel.SendMessageAsync($"I can't deliver the `{attachment.Filename}` attachment. I can currently only transmit image attachments. (.jpg, .gif, .png)");
            //    }
            //}
            EmbedBuilder embed = new EmbedBuilder()
                .WithAuthor(message.Author)
                .WithColor(74, 171, 189)
                .WithDescription(message.Content);
            return embed.Build();
        }
        #endregion

        #region Methods
        private static void SendMessageToThread(SocketThreadChannel thread, SocketMessage message)
        {
            try
            {
                if (message.Attachments.Count > 0)
                {
                    var attachments = new List<FileAttachment>();
                    var exepath = AppDomain.CurrentDomain.BaseDirectory;
                    foreach (var file in message.Attachments)
                    {
                        var attachmentUrl = ((IAttachment)file).Url;
                        using (WebClient client = new WebClient())
                        {
                            client.DownloadFile(new Uri(attachmentUrl), @$"{exepath}\{file.Filename}");
                        }
                        attachments.Add(new FileAttachment(@$"{exepath}\{file.Filename}"));
                    }

                    var msg = thread.SendFilesAsync(attachments, "", embed: CreateDMEmbed(message)).Result;

                    foreach (var file in attachments)
                    {
                        file.Dispose();
                        File.Delete($@"{exepath}\{file.FileName}");
                    }
                }
                else
                {
                    thread.SendMessageAsync(embed: CreateDMEmbed(message));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        #endregion

        #region Tasks
        public static async Task<bool> IsPrivateMessage(SocketMessage msg)
        {
            await Task.CompletedTask;
            return (msg.Channel.GetType() == typeof(SocketDMChannel));
        }

        public static async Task HandleSendDMs(SocketMessage message, string userID, DiscordSocketClient client)
        {
            try
            {
                var user = client.GetUserAsync(ulong.Parse(userID)).Result;
                var privateChannel = Discord.UserExtensions.SendMessageAsync(user, embed: CreateDMEmbed(message));
                await message.AddReactionAsync(Emote.Parse("<:delivered:917731122299940904>"));
            }
            catch (Exception ex)
            {
                await Handler.HandlingService._bobiiHelper.WriteToConsol("MsgRecievd", true, "HandleSendDMs", message: "The dm could not be delivered!", exceptionMessage: ex.Message);
                await message.AddReactionAsync(Emote.Parse("<:deliverfail:917731174162526208>"));
            }

        }

        public static async Task HandleDMs(SocketMessage message, SocketTextChannel dmChannel, DiscordSocketClient client)
        {
            try
            {
                var thread = CheckIfThreadExists(message, dmChannel);
                if (thread == null)
                {
                    thread = CreateThread(message, dmChannel);
                    var myGuild = client.GetGuild(712373862179930144);
                    var myGuildRest = client.Rest.GetGuildAsync(712373862179930144).Result;
                    await thread.AddUserAsync((IGuildUser)myGuildRest.GetUserAsync(410312323409117185).Result);
                }
                SendMessageToThread(thread, message);
                await message.AddReactionAsync(Emote.Parse("<:delivered:917731122299940904>"));
            }
            catch (Exception ex)
            {
                await Handler.HandlingService._bobiiHelper.WriteToConsol("MsgRecievd", true, "HandleDMs", message: "The dm could not be delivered!", exceptionMessage: ex.Message);
                await message.AddReactionAsync(Emote.Parse("<:deliverfail:917731174162526208>"));
            }
        }
        #endregion
    }
}
