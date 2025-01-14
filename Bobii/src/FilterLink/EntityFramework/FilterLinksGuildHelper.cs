﻿using Bobii.src.EntityFramework;
using Bobii.src.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bobii.src.FilterLink.EntityFramework
{
    class FilterLinksGuildHelper
    {
        #region Tasks
        public static async Task<List<filterlinksguild>> GetLinks(ulong guildid)
        {
            try
            {
                using (var context = new BobiiEntities())
                {
                    return context.FilterLinksGuild.AsQueryable().Where(fl => fl.guildid ==guildid).ToList();
                }
            }
            catch (Exception ex)
            {
                await Handler.HandlingService._bobiiHelper.WriteToConsol("FilterLink", true, "GetLinks", exceptionMessage: ex.Message);
                return null;
            }
        }

        public static async Task<bool> IsFilterlinkAllowedInGuild(ulong guildid, string bezeichnung)
        {
            try
            {
                using (var context = new BobiiEntities())
                {
                    var filterlink = context.FilterLinksGuild.AsQueryable().Where(fl => fl.bezeichnung == bezeichnung && fl.guildid == guildid).FirstOrDefault();
                    if (filterlink != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                await Handler.HandlingService._bobiiHelper.WriteToConsol("FilterLink", true, "IsFilterlinkAllowedInGuild", exceptionMessage: ex.Message);
                return false;
            }
        }

        public static async Task AddToGuild(ulong guildid, string bezeichnung)
        {
            try
            {
                using (var context = new BobiiEntities())
                {
                    var filterLink = new filterlinksguild();
                    filterLink.bezeichnung = bezeichnung;
                    filterLink.guildid = guildid;
                    context.FilterLinksGuild.Add(filterLink);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                await Handler.HandlingService._bobiiHelper.WriteToConsol("FilterLink", true, "AddToGuild", exceptionMessage: ex.Message);
            }
        }

        public static async Task RemoveFromGuild(ulong guildid, string bezeichnung)
        {
            try
            {
                using (var context = new BobiiEntities())
                {
                    var filterLink = context.FilterLinksGuild.AsQueryable().Where(fl => fl.guildid == guildid && fl.bezeichnung == bezeichnung).FirstOrDefault();
                    context.FilterLinksGuild.Remove(filterLink);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                await Handler.HandlingService._bobiiHelper.WriteToConsol("FilterLink", true, "RemoveFromGuild", exceptionMessage: ex.Message);
            }
        }
        #endregion
    }
}
