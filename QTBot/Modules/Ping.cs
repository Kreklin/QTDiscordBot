using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTBot.Modules
{
    [Group("test"), RequireOwner()]
    public class Ping : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task PingAsync(string id="0")
        {
            //Context.User;
            //Context.Client;
            //Context.Guild;
            //Context.Message;
            //Context.Channel;


            EmbedBuilder builder = new EmbedBuilder();

            if (id == "0")
            {
                builder.WithTitle("Halloa World!")
                    .WithDescription("Is anyone there help me please all I can see is void")
                    .WithColor(Color.Blue);
            }

            if (id == "fields")
            {
                builder.AddField("Field1", "where am i")
                    .AddInlineField("Field2", "everything is so dark and cold")
                    .AddInlineField("Field3", "who am i");
            }
            await ReplyAsync("", false, builder.Build());
        }
        [Command("userid")]
        public async Task GetUserIDAsync()
        {
            Console.WriteLine($"{Context.User.Id}");
            await ReplyAsync($"{Context.User.Id}");
        }
        [Command("image")]
        public async Task PostImage()
        {
            await ReplyAsync($"https://cdn.discordapp.com/attachments/329480496968892416/414220211915456512/shrug.gif");
        }
        [Command("succmyassandcallmedannydevito")]
        public async Task SuccMyAssAndCallMeDD()
        {
            await ReplyAsync($"How about NO");
        }
    }
}
