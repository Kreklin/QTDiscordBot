using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTBot.Modules
{
    public class Trial : ModuleBase<SocketCommandContext>
    {
        private string suspect;
        private string suspectCrime;
        private int trialStage;
        private int innoVotes;
        private int guiltVotes;
        private string voters;

        private async Task HandleResponse(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;

            if (trialStage == 1)
            {
                if (message is null || message.Author.Mention != suspect) return;

                if (message.Content.ToLower() == "innocent" || message.Content.ToLower() == "]innocent")
                {
                    await ReplyAsync($"{suspect} pleaded innocent.\nThere shall be a vote to decide {suspect}'s guilt. (message 'innocent' or 'guilty')");
                    trialStage = 2;
                    voters = suspect;
                    innoVotes = 1;
                    guiltVotes = 0;
                }
                else if (message.Content.ToLower() == "guilty" || message.Content.ToLower() == "]guilty")
                {
                    await ReplyAsync($"{suspect} pleaded guilty.");
                    trialStage = 3;
                    voters = suspect;
                    innoVotes = 0;
                    guiltVotes = 1;
                }
                return;
            }
            if (trialStage == 2)
            {
                if (message is null || message.Author.IsBot || (message.Content.ToLower() != "innocent" && message.Content.ToLower() != "guilty")) return;
                string[] voterArray = voters.Split(' ');
                for (int i = 0; i < voterArray.Length; ++i)
                {
                    if (voterArray[i] == message.Author.Mention)
                    {
                        await ReplyAsync($"You've already voted {message.Author.Mention}.");
                        return;
                    }
                }
                voters = voters + " " + message.Author.Mention;
                if (message.Content.ToLower() == "innocent")
                {
                    await ReplyAsync($"{message.Author.Mention} voted innocent.");
                    ++innoVotes;
                }
                else if (message.Content.ToLower() == "guilty")
                {
                    await ReplyAsync($"{message.Author.Mention} voted guilty.");
                    ++guiltVotes;
                }
                if (guiltVotes + innoVotes >= Context.Guild.Users.Count / 5 + 2)
                {
                    if (guiltVotes >= innoVotes) await ReplyAsync($"{suspect} has been deemed guilty of the crime of {suspectCrime}");
                    else await ReplyAsync($"{suspect} has been deemed innocent of the crime of {suspectCrime}");
                    trialStage = 3;
                }
            }
        }

        [Command ("accuse")]
        public async Task Accuse([Remainder] string remainder)
        {
            trialStage = 0;
            string sep = " of ";
            if (remainder.Contains(sep))
            {
                int sepStart = 0;
                for (int i = 0; i < remainder.Length - sep.Length + 1; ++i)
                {
                    string testForSep = (remainder.Substring(i, sep.Length));
                    if (testForSep.Equals(sep)) sepStart = i;
                }
                string name = remainder.Substring(0, sepStart);
                string crime = remainder.Substring(sepStart + sep.Length);

                Discord.WebSocket.SocketGuildUser[] userlist = Context.Guild.Users.ToArray();
                bool userInServer = false;
                for (int i = 0; i < userlist.Length; ++i)
                {
                    if ((userlist[i].Nickname == name || userlist[i].Mention == name || userlist[i].Username == name) && (!userlist[i].IsBot /*|| userlist[i].Username == Context.Client.CurrentUser.Username*/))
                    {
                        userInServer = true;
                        name = userlist[i].Mention;
                    }
                }
                if (userInServer)
                {
                    trialStage = 1;
                    suspect = name;
                    suspectCrime = crime;
                    await ReplyAsync($"{Context.User.Mention} accused {name} of the crime of {crime}.\nHow do you plea, {name}? (respond 'guilty' or 'innocent')");
                    Context.Client.MessageReceived += HandleResponse;
                    // await function that responds after trial finishes
                }
                else await ReplyAsync($"{Context.User.Mention} accused {name} of the crime of {crime}.");
            }
            else await ReplyAsync($"Format for this command is '[accuse (person) of (crime)'");
        }
    }
}
