using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace QTBot.Modules
{
    public class JoJo : ModuleBase<SocketCommandContext>
    {
        [Command("delet")]
        public async Task TheHand()
        {
            List<IReadOnlyCollection<Discord.IMessage>> messageCollection = await Context.Channel.GetMessagesAsync(5).ToList();

            for (int i = 0; i < messageCollection.Count; i++)
            {
                await Context.Channel.DeleteMessagesAsync(messageCollection[i]);
            }

            await ReplyAsync($"ZA HANDO!\nb̸̃̉m̸͛͒m̷̊̊m̵̚͝m̵̒͋m̷̉͂m̶̏̑");
        }
        [Command("stop", RunMode = RunMode.Async)]
        public async Task TheWorld()
        {
            await ReplyAsync($"ZA WARUDO!\nToki wo tomare!\np̷̭̓ͅş̶̰̰͕͋̑̋̔̈̇̍͘̕ś̶̢͈̊̽̈́́s̸̱̱͕͙͔̾̂̇s̶̢̳̜̩̰̘͔̳͇͂̉̆̌̉̈́͗ṣ̷̩̍̔̓͂̇͠s̸̜̜͙͉̠̫̈͋̐͜t̶̠̫͙̞̝̤͎̳͊̎̀͒͛̅͜͝ţ̴̭̲̯͇͗ḥ̶̦̘̫̻̙̀͛̌́c̷̟͕̟̀̎̔̊ḫ̵̻͉̜̟͔̟̠́c̴̗̞͗̎͒͐̀͝h̵̡̡͖̥͓̠̩̎͂͠");

            List<Discord.WebSocket.SocketGuildUser> userCollection = Context.Guild.Users.ToList();
            List<Discord.WebSocket.SocketRole>[] UserRoleCollection = new List<SocketRole>[userCollection.Count];
            
            for (int i = 0; i < userCollection.Count; i++)
            {
                if (userCollection[i].Hierarchy < Context.Guild.CurrentUser.Hierarchy && !userCollection[i].IsBot)
                {
                    List<Discord.WebSocket.SocketRole> UserRoles = userCollection[i].Roles.ToList();
                    UserRoleCollection[i] = userCollection[i].Roles.ToList();
                    for (int j = 0; j < userCollection[i].Roles.Count; j++)
                    {
                        if (!UserRoles[j].IsEveryone)
                        {
                            Console.WriteLine("Deleting user " + userCollection[i].Username + "'s " + UserRoles[j].Name + " role.");
                            await userCollection[i].RemoveRoleAsync(UserRoles[j]);
                        }
                    }
                }
            }


            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while (stopwatch.ElapsedMilliseconds < 1000) {}
            await ReplyAsync($"One second has passed...");
            while (stopwatch.ElapsedMilliseconds < 2000) {}
            await ReplyAsync($"Two seconds have passed.");
            while (stopwatch.ElapsedMilliseconds < 3000) {}
            await ReplyAsync($"Three seconds have passed!");
            while (stopwatch.ElapsedMilliseconds < 4000) {}
            await ReplyAsync($"FOUR SECONDS HAVE PASSED!");
            while (stopwatch.ElapsedMilliseconds < 5000) {}
            await ReplyAsync($"*FIVE SECONDS HAVE PASSED!*");
            while (stopwatch.ElapsedMilliseconds < 6000) {}
            await ReplyAsync($"Soshite toki wa ugaki dasu...\nB̷͔͔̣͊͐͐͒́̄̕̕̕̕̕̕͝W̶̡̠̫͎̦̱͉̭̓͆̋̅̊͊̋̾̀Õ̶̢̢̼̗͚̞̘͔̫̋̑̍̑̚ͅW̸̢̛̜̣͈͍̾̆̽̔̌͂͂͋̚͝Ǒ̶̞͒͒̋̒̏̔͛̓W̷̧̨̗̻̼̖͔̳͚͈̙͑͒͑͆͐̋̓̾͋͝ͅƠ̵̳̐̒̑̄̏̀̍̚͝͠W̷̛͚̖̝̻̿̀̐̓̈͆̌̑͋̇͗͘͝Ơ̸̼͇͙͚̦̦̯̮͈͈̗̽͛͂̀̾̔̊̀̑̕͝W̶̺̖̫̫͎̋̾̐̃͆̋̔̽͌̕M̸̝̯̦̻̒̄͆̌̈́̇̊͗͒̊̒̚͘͝͝M̶̛̛̫̤͚̤͈̀̏͛́̎̅̐̒̈́͘M̶͓̝̑͐M̸̛̹̪̳̼̯̖̫̤̯͔̑̃̎́̈́̐͋͛͜ͅM̶̧̤̣͇͈̤̏̀̔̇͒͘ͅM̴̨̨̙̟̗̘̲̖̜̤̻͐̓̇́M̶͉͈̺̻̹̱͕̐̈́̊͂̓̐ͅM̸̨̩̎͒̏̾̔͒̏̕Ç̶̡̣̫̰̺͉̾̂̀͋͘͝ͅK̷̡͓̜̻͇͙̝̙̭̳͙̗̩͚̃");
            stopwatch.Stop();

            for (int i = 0; i < userCollection.Count; i++)
            {
                if (userCollection[i].Hierarchy < Context.Guild.CurrentUser.Hierarchy && !userCollection[i].IsBot)
                {
                    for (int j = 0; j < UserRoleCollection[i].Count; j++)
                    {
                        if (!UserRoleCollection[i][j].IsEveryone)
                        {
                            Console.WriteLine("Returning user " + userCollection[i].Username + "'s " + UserRoleCollection[i][j] + " role.");
                            await userCollection[i].AddRoleAsync(UserRoleCollection[i][j]);
                        }
                    }
                }
            }
        }
    }
}
