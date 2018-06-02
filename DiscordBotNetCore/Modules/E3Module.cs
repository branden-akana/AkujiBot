﻿using Discord;
using Discord.Commands;
using Easy.Common;
using Easy.Common.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public class E3Module : ModuleBase<SocketCommandContext>
	{
		[Command("bongo")]
		[Remarks("bongo [Facepunch username]")]
		[Summary("Generates your E3 bongo card using your Facepunch username. Make sure you allow DMs from users in this channel so the bot can send you the card.")]
		public async Task Bongo(string username = " ")
		{			
			if (String.IsNullOrWhiteSpace(username))            //empty name check
			{
				var message = await ReplyAsync($"`Error: No name entered.`");
				await Task.Delay(2000);
				await message.DeleteAsync();
				return;
			}

			if (DateTime.Now.Month != 6)            //E3 month check (June)
			{
				var message = await ReplyAsync($"`Error: This command only works during June, the month of E3. If it is June and you see this message, check the internal date of the system this bot is running on.`");
				await Task.Delay(2000);
				await message.DeleteAsync();
				return;
			}

			string url = "http://e3.novaember.com/";
			string imagePath = $"cards-{DateTime.Now.Year}/{username}.png";

			using (IRestClient client = new RestClient())
			{
				try
				{
					var generate = await client.GetAsync(url + username);
					generate.EnsureSuccessStatusCode();

					var embed = new EmbedBuilder();
					embed.WithAuthor($"Facepunch E3 Bongo Card", Context.User.GetAvatarUrl(), url + imagePath);
					embed.WithColor(Color.Green);
					embed.WithFooter($"Image URL: {url + imagePath}. Be sure to read the rules on Facepunch or the FP Gaming Events Discord!");
					embed.WithImageUrl(url + imagePath);
					await Context.User.SendMessageAsync("", false, embed.Build());
				}
				catch (HttpRequestException e)
				{
					var message = await ReplyAsync($"`Error: {e.Message}`");
					await Task.Delay(2000);
					await message.DeleteAsync();
					return;
				}
			}
		}
	}
}
