using Discord.Commands;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace QTBot.Modules
{
    public class Waifu : ModuleBase<SocketCommandContext>
    {
        private class WaifData
        {
            private string m_userid = "0";
            private List<string> m_waifuNames = new List<string>();

            public WaifData(string userid) // constructor for only user id
            {
                m_userid = userid;
            }
            public WaifData(List<string> list) // constructor for user id and waifu list
            {
                m_userid = list[0];
                for (int i = 1; i < list.Count; ++i)
                {
                    m_waifuNames.Add(list[i]);
                }
            }

            public WaifData AddWaifu(string waifuName) // adds specified string to list
            {
                this.m_waifuNames.Add(waifuName);
                return this;
            }
            public WaifData RemoveWaifu(string waifuName) // removes specified string from list
            {
                this.m_waifuNames.Remove(waifuName);
                return this;
            }
            public bool CheckID(string id) // returns true if the user id stored here is the same as the input
            {
                if (id == m_userid) return true;
                else return false;
            }
            public bool CheckWaifuList(string waifuName) // returns true if the waifu name inputted is contained within the list
            {
                if (m_waifuNames.Contains(waifuName)) return true;
                else return false;
            }
            public string OutputData() // returns the data from this object in csv format
            {
                string output = m_userid;
                for (int i = 0; i < m_waifuNames.Count; ++i)
                {
                    output = output + "," + m_waifuNames[i];
                }
                return output;
            }
        }
        
        private List<WaifData> GetFileInput() // gets waifu database from csv file
        {
            Console.WriteLine("Retrieving waifu data...");
            using (var reader = new StreamReader(@"Sauce Files\waifdata.csv"))
            {
                List<WaifData> waifdata = new List<WaifData>();
                int op = 0;
                Console.WriteLine("Preparations completed.");
                while (!reader.EndOfStream) 
                {
                    Console.WriteLine($"Retrieving user {op}'s data...");
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    Console.WriteLine("The data has been split.");
                    List<string> list = new List<string>();
                    for (int i = 0; i < values.Length; ++i)
                    {
                        list.Add(values[i]);
                    }
                    Console.WriteLine("The data has been merged as a list."); 
                    WaifData tempWD = new WaifData(list); // error this line: object reference not set to an instance of an object.
                    Console.WriteLine("Temporary object created.");
                    waifdata.Add(tempWD);
                    ++op;
                    Console.WriteLine($"User {op}'s data has been retrieved.");
                } 
                Console.WriteLine("Retrieval successful.");
                return waifdata;
            }
        }

        private void WriteFileOutput(List<WaifData> waifdata)
        {
            Console.WriteLine("Updating database...");
            using (var writer = new StreamWriter(@"Sauce Files\waifdata.csv"))
            {
                for (int i = 0; i < waifdata.Count; ++i)
                {
                    writer.WriteLine(waifdata[i].OutputData());
                }
            }
        }

        private int FindUserIndex(string id) // compares specified id until finds a match, then returns index of match
        {
            List<WaifData> waifdata = GetFileInput();
            int index = -1;
            for (int i = 0; i < waifdata.Count; ++i)
            {
                if (waifdata[i].CheckID(id))
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        private int AddUser(List<WaifData> waifList, string id) // adds new user to database then returns the index of the new user
        {
            waifList.Add(new WaifData(id));
            return waifList.Count - 1;
        }

        private void AddWaifu(string id, string waifuName) // adds a waifu to a users list then writes to file
        {
            List<WaifData> waifdata = GetFileInput();
            int index = FindUserIndex(id);
            if (index == -1)
            {
                Console.WriteLine("User ID not found, creating new record.");
                index = AddUser(waifdata, id); // creates a new record for the user
            }
            waifdata[index].AddWaifu(waifuName);
            WriteFileOutput(waifdata);
        }
        private void RemoveWaifu(string id, string waifuName) // removes a waifu from a users list then writes to file
        {
            List<WaifData> waifdata = GetFileInput();
            int index = FindUserIndex(id);
            if (index == -1) Console.WriteLine("Error: User ID not found.\n");
            waifdata[index].RemoveWaifu(waifuName);
            WriteFileOutput(waifdata);
        }

        private bool SpecificWaifuSearch(string id, string waifuName) // returns true if a specifc user has a specific waifu
        {
            Console.WriteLine($"\nChecking if user {Context.User} has claimed {waifuName}.\n");
            List<WaifData> waifdata = GetFileInput();
            int index = FindUserIndex(id);
            if (index == -1) return false;
            if (waifdata[index].CheckWaifuList(waifuName)) return true;
            else return false;
        }

        private bool GeneralWaifuSearch(string waifuName) // returns true if any user has a specific waifu
        {
            Console.WriteLine($"\nChecking if anyone has claimed {waifuName}.\n");
            List<WaifData> waifdata = GetFileInput();
            for (int i = 0; i < waifdata.Count; ++i)
            {
                if (waifdata[i].CheckWaifuList(waifuName)) return true;
            }
            return false;
        }

            [Command("claim")]
            public async Task ClaimWaifu([Remainder] string name)
            {
            if (SpecificWaifuSearch(Convert.ToString(Context.User.Id), name)) // if the user already has this waifu
                await ReplyAsync($"You've already claimed this waifu.");
            else
            if (GeneralWaifuSearch(name))
                await ReplyAsync($"Sorry {Context.User}, someone else already claimed them. Better luck next time.");
            else
            {
                AddWaifu(Convert.ToString(Context.User.Id), name); // write new waifu to csv file
                await ReplyAsync($"{Context.User} has claimed {name} as their wife!");
            }
            }

            [Command("divorce")]
            public async Task DivorceWaifu([Remainder] string name)
            {
            if (!SpecificWaifuSearch(Convert.ToString(Context.User.Id), name))
                await ReplyAsync($"You can't divorce someone you're not married to!");
            else
            {
                RemoveWaifu(Convert.ToString(Context.User.Id), name); // remove waifu from file
                await ReplyAsync($"{Context.User} has divorced {name}...");
            }
            }

            [Command("harem")]
            public async Task PrintHarem()
        {
            int index = FindUserIndex(Convert.ToString(Context.User.Id));
            List<WaifData> waifdata = GetFileInput();
            string data = waifdata[index].OutputData();
            var split = data.Split(',');
            data = "";
            for (int i = 1; i < split.Length; ++i)
            {
                data = data + "\n" + i + ". " + split[i];
            }
            await ReplyAsync($"{Context.User}'s harem:{data}");
        }
    }
}
