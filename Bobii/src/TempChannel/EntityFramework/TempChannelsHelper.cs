﻿using Bobii.src.EntityFramework;
using Bobii.src.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bobii.src.TempChannel.EntityFramework
{
    class TempChannelsHelper
    {
        #region Tasks
        public static async Task AddTC(ulong guildId, ulong tempChannelId, ulong createTempChannelId, ulong ownerId, ulong textChannelID)
        {
            try
            {
                using (var context = new BobiiEntities())
                {
                    var tempChannel = new tempchannels();
                    tempChannel.guildid = guildId;
                    tempChannel.channelid = tempChannelId;
                    tempChannel.channelownerid = ownerId;
                    tempChannel.createchannelid = createTempChannelId;
                    tempChannel.textchannelid = textChannelID;
                    var count = new int();
                    if (context.TempChannels.AsQueryable().Where(t => t.createchannelid == createTempChannelId).Count() == 0)
                    {
                        count = 1;
                    }
                    else
                    {
                        count = context.TempChannels.AsQueryable().Where(t => t.createchannelid == createTempChannelId).Max(channel => channel.count) + 1;
                    }
                    tempChannel.count = count;
                    context.TempChannels.Add(tempChannel);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                await Handler.HandlingService._bobiiHelper.WriteToConsol("TempChannl", true, "AddTC", exceptionMessage: ex.Message);
            }
        }

        public static async Task<tempchannels> GetTempChannel(ulong tempChannelID)
        {
            try
            {
                using (var context = new BobiiEntities())
                {
                    return context.TempChannels.AsQueryable().Where(t => t.channelid == tempChannelID).First();
                }
            }
            catch (Exception ex)
            {
                await Handler.HandlingService._bobiiHelper.WriteToConsol("TempChannl", true, "GetTempChannel", exceptionMessage: ex.Message);
                return null;
            }
        }

        public static async Task<int> GetCount(ulong createChannelID)
        {
            try
            {
                using (var context = new BobiiEntities())
                {
                    if (context.TempChannels.AsQueryable().Where(c => c.createchannelid == createChannelID).Count() == 0)
                    {
                        return 1;
                    }
                    return context.TempChannels.AsQueryable().Where(c => c.createchannelid == createChannelID).Max(channel => channel.count) + 1;
                }
            }
            catch (Exception ex)
            {
                await Handler.HandlingService._bobiiHelper.WriteToConsol("TempChannl", true, "RemoveTC", exceptionMessage: ex.Message);
                return 0;
            }
        }

        public static async Task RemoveTC(ulong guildId, ulong tempChannelID)
        {
            try
            {
                using (var context = new BobiiEntities())
                {
                    var tempChannel = context.TempChannels.AsQueryable().Where(tc => tc.channelid == tempChannelID).FirstOrDefault();
                    context.TempChannels.Remove(tempChannel);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                await Handler.HandlingService._bobiiHelper.WriteToConsol("TempChannl", true, "RemoveTC", exceptionMessage: ex.Message);
            }
        }

        public static async Task<List<tempchannels>> GetTempChannelList(ulong guildId)
        {
            try
            {
                using (var context = new BobiiEntities())
                {
                    return context.TempChannels.AsQueryable().Where(tc => tc.guildid == guildId).ToList();
                }
            }
            catch (Exception ex)
            {
                await Handler.HandlingService._bobiiHelper.WriteToConsol("TempChannl", true, "GetTempChannelList", exceptionMessage: ex.Message);
                return null;
            }
        }

        public static async Task<ulong> GetOwnerID(ulong channelId)
        {
            try
            {
                using (var context = new BobiiEntities())
                {
                    return context.TempChannels.AsQueryable().Where(tc => tc.channelid == channelId).Select(t => t.channelownerid.Value).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                await Handler.HandlingService._bobiiHelper.WriteToConsol("TempChannl", true, "GetOwnerID", exceptionMessage: ex.Message);
                return 0;
            }
        }

        public static async Task ChangeOwner(ulong channelId, ulong newOwnerId)
        {
            try
            {
                using (var context = new BobiiEntities())
                {
                    var tempChannel = context.TempChannels.AsQueryable().Where(channel => channel.channelid == channelId).First();
                    tempChannel.channelownerid = newOwnerId;
                    context.TempChannels.Update(tempChannel);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                await Handler.HandlingService._bobiiHelper.WriteToConsol("TempChannl", true, "ChangeOwner", exceptionMessage: ex.Message);
            }
        }

        public static async Task<bool> DoesOwnerExist(ulong ownerId)
        {
            try
            {
                using (var context = new BobiiEntities())
                {
                    var tempChannel = context.TempChannels.AsQueryable().Where(channel => channel.channelownerid == ownerId).FirstOrDefault();
                    return tempChannel != null;
                }

            }
            catch (Exception ex)
            {
                await Handler.HandlingService._bobiiHelper.WriteToConsol("TempChannl", true, "DoesOwnerExist", exceptionMessage: ex.Message);
                return false;
            }
        }

        public static async Task<bool> DoesTempChannelExist(ulong channelId)
        {
            try
            {
                using (var context = new BobiiEntities())
                {
                    var tempChannel = context.TempChannels.AsQueryable().Where(channel => channel.channelid == channelId).FirstOrDefault();
                    return tempChannel != null;
                }
            }
            catch (Exception ex)
            {
                await Handler.HandlingService._bobiiHelper.WriteToConsol("TempChannl", true, "DoesTempChannelExist", exceptionMessage: ex.Message);
                return false;
            }
        }
        #endregion
    }
}
