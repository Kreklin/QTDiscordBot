using Discord.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QTBot.Modules.BlackJeck;

namespace QTBot.Modules
{
    public class Krekels : ModuleBase<SocketCommandContext>
    {
        private class KrekelsData
        {
            private string m_userid = "0";
            private int m_krekels = 0;

            public KrekelsData(string userid, int krekels = 0)
            {
                m_userid = userid;
                m_krekels = krekels;
            }

            public int ChangeKrekels(int change = 0) // changes money by the amount specified
            {
                m_krekels += change;
                return m_krekels;
            }
            public bool CheckID(string id) // returns true if the user id stored here is the same as the input
            {
                if (id == m_userid) return true;
                else return false;
            }
            public string OutputData()
            {
                string output = m_userid + "," + Convert.ToString(m_krekels);
                return output;
            }
        }

        private List<KrekelsData> GetFileInput() // retrieves krekels data
        {
            using (var reader = new StreamReader(@"Sauce Files\krekels.csv"))
            {
                List<KrekelsData> krekelsdata = new List<KrekelsData>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    KrekelsData temp = new KrekelsData(values[0], Convert.ToInt32(values[1]));
                    krekelsdata.Add(temp);
                }
                return krekelsdata;
            }
        }

        private void WriteFileOutput(List<KrekelsData> krekelsdata)
        {
            Console.WriteLine("Updating database...");
            using (var writer = new StreamWriter(@"Sauce Files\waifdata.csv"))
            {
                for (int i = 0; i < krekelsdata.Count; ++i)
                {
                    writer.WriteLine(krekelsdata[i].OutputData());
                }
            }
        }

        private int FindUserIndex(string id)
        {
            List<KrekelsData> krekelsdata = GetFileInput();
            int index = -1;
            for (int i = 0; i < krekelsdata.Count; ++i)
            {
                if (krekelsdata[i].CheckID(id))
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        private int AddUser(List<KrekelsData> krekelslist, string id) // adds new user to database then returns the index of the new user
        {
            krekelslist.Add(new KrekelsData(id));
            return krekelslist.Count - 1;
        }

        private bool ExchangeKrekels(string userA, string userB, int amount) // takes amount from userA and gives it to userB
        {
            List<KrekelsData> krekelsdata = GetFileInput();
            if (krekelsdata[FindUserIndex(userA)].ChangeKrekels() - amount < 0) return false;
            krekelsdata[FindUserIndex(userA)].ChangeKrekels(-amount);
            krekelsdata[FindUserIndex(userB)].ChangeKrekels(amount);
            return true;
        }

        /*private async Task<BlackJack.GameResult> ConvertGameResultToTask(BlackJack.GameResult result)
        {
            return result;
        }*/

        [Command("balance")]
        public async Task GetBalanceAsync()
        {
            int index = FindUserIndex(Convert.ToString(Context.User.Id));
            List<KrekelsData> krekelsdata = GetFileInput();
            if (index == -1) AddUser(krekelsdata, Convert.ToString(Context.User.Id));
            int krekels = krekelsdata[index].ChangeKrekels();
            await ReplyAsync($"{Context.User} has {krekels} krekels.");
        }

        /*[Command("blackjack")]
        public async Task ExecuteBlackJack([Remainder] int bet)
        {
            await ReplyAsync($"{Context.User} bets {bet} krekels on a game of nibbajeck, double or nothing.");
            BlackJack jeck = new BlackJack();

            if (jeck.PlayBlackJack() == ConvertGameResultToTask(BlackJack.GameResult.WIN))
                await ReplyAsync($"You won the big cash.");
        }*/
    }
}
