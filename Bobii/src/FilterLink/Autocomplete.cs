﻿using Discord;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Bobii.src.FilterLink
{
    class AutoComplete
    {
        public static async Task AddAutoComplete(SocketAutocompleteInteraction interaction)
        {
            var guildUser = (IGuildUser)interaction.User;
            var possibleChoices = FilterLink.Helper.GetFilterLinksOfGuild(guildUser.GuildId).Result;

            if (possibleChoices.Count() == 0)
            {
                possibleChoices = new string[] { "Already all links added" };
            }

            // lets get the current value they have typed. Note that were converting it to a string for this example, the autocomplete works with int and doubles as well.
            var current = interaction.Data.Current.Value.ToString();

            // We will get the first 20 options inside our string array that start with whatever the user has typed.
            var opt = possibleChoices.Where(x => x.StartsWith(current)).Take(20);

            // Then we can send them to the client
            await interaction.RespondAsync(opt.Select(x => new AutocompleteResult(x, x.ToLower())));
        }

        public static async Task RemoveAutoComplete(SocketAutocompleteInteraction interaction)
        {
            var guildUser = (IGuildUser)interaction.User;
            var filterLinksOfGuild = EntityFramework.FilterLinksGuildHelper.GetLinks(guildUser.GuildId).Result;
            var choicesList = new List<string>();
            foreach(var filterlink in filterLinksOfGuild)
            {
                choicesList.Add(filterlink.bezeichnung.Trim());
            }

            var possibleChoices = new string[] { };
            if (choicesList.Count == 0)
            {
                possibleChoices = new string[] { "No links to remove yet" };
            }
            else
            {
                possibleChoices = choicesList.ToArray() ;
            }

            // lets get the current value they have typed. Note that were converting it to a string for this example, the autocomplete works with int and doubles as well.
            var current = interaction.Data.Current.Value.ToString();

            // We will get the first 20 options inside our string array that start with whatever the user has typed.
            var opt = possibleChoices.Where(x => x.StartsWith(current)).Take(20);

            // Then we can send them to the client
            await interaction.RespondAsync(opt.Select(x => new AutocompleteResult(x, x.ToLower())));
        }
    }
}
