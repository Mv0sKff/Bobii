﻿using Bobii.src.EntityFramework;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bobii.src.Bobii.EntityFramework
{
    class BobiiHelper
    {
        #region Tasks
        public static async Task DeleteEverythingFromGuild(SocketGuild guild)
        {
            try
            {
                using (var context = new BobiiEntities())
                {
                    context.TempChannels.RemoveRange(context.TempChannels.AsEnumerable().Where(tc => tc.guildid == guild.Id));
                    context.CreateTempChannels.RemoveRange(context.CreateTempChannels.AsEnumerable().Where(ctc => ctc.guildid == guild.Id));
                    context.FilterLink.RemoveRange(context.FilterLink.AsEnumerable().Where(fl => fl.guildid == guild.Id));
                    context.FilterLinkLogs.RemoveRange(context.FilterLinkLogs.AsEnumerable().Where(fl => fl.guildid == guild.Id));
                    context.FilterLinksGuild.RemoveRange(context.FilterLinksGuild.AsEnumerable().Where(fl => fl.guildid == guild.Id));
                    context.FilterLinkUserGuild.RemoveRange(context.FilterLinkUserGuild.AsEnumerable().Where(fl => fl.guildid == guild.Id));
                    context.FilterWords.RemoveRange(context.FilterWords.AsEnumerable().Where(fw => fw.guildid == guild.Id));
                    context.SaveChanges();
                    await Handler.HandlingService._bobiiHelper.WriteToConsol("NukeDataGu", false, "DeleteEverythingFromGuild", new Entities.SlashCommandParameter() { Guild = guild}, message: "Successfully nuked guild information!");
                }
            }
            catch (Exception ex)
            {
                await Handler.HandlingService._bobiiHelper.WriteToConsol("NukeDataGu", true, "DeleteEverythingFromGuild", new Entities.SlashCommandParameter() { Guild = guild }, exceptionMessage: ex.Message);
            }
        }

        public static async Task<List<src.EntityFramework.Entities.caption>> GetCaptions()
        {
            try
            {
                using (var context = new BobiiEntities())
                {
                    return context.Captions.ToList();
                }
            }
            catch (Exception ex)
            {
                await Handler.HandlingService._bobiiHelper.WriteToConsol("BobiiHelpe", true, "GetMsg", exceptionMessage: ex.Message);
                return null;
            }
        }

        public static async Task<string> GetLanguage(ulong guildId)
        {
            try
            {
                return "en";
                // §TODO JG/22.12.2021 build this in
                //using (var context = new BobiiEntities())
                //{
                //    var language = context.Languages.AsQueryable().Where(l => l.guildid == guildId).FirstOrDefault();
                //    if (language == null)
                //    {
                //        return "en";
                //    }
                //    else
                //    {
                //        return language.langugeshort;
                //    }
                //}
            }
            catch (Exception ex)
            {
                await Handler.HandlingService._bobiiHelper.WriteToConsol("BobiiHelpe", true, "GetLanguage", exceptionMessage: ex.Message);
                return null;
            }
        }

        public static async Task<List<src.EntityFramework.Entities.content>> GetContents()
        {
            try
            {
                using (var context = new BobiiEntities())
                {
                    return context.Contents.ToList();
                }
            }
            catch (Exception ex)
            {
                await Handler.HandlingService._bobiiHelper.WriteToConsol("BobiiHelpe", true, "GetMsg", exceptionMessage: ex.Message);
                return null;
            }
        }
        #endregion
    }
}
