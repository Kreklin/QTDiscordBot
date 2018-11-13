using Discord;
using Discord.Commands;
using Discord.WebSocket;

using WebSocket4Net;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace QTBot
{
    class Program
    {
        private string user = "211014567596261386";
        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        public DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

        public async Task RunBotAsync()
        {

            _client = new DiscordSocketClient();
            _commands = new CommandService();

            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();

            string botToken = "NDAzNzgyNzM4MTY0ODQyNDk2.DUNX3A.taJTfGzn5-Bl2eXIQfYIzt_soy4";

            // event subsricptions
            _client.Log += Log;
            _client.UserJoined += AnnounceUserJoined;

            await RegisterCommandsAsync();

            await _client.LoginAsync(TokenType.Bot, botToken);

            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private async Task AnnounceUserJoined(SocketGuildUser user)
        {
            var guild = user.Guild;
            var channel = guild.DefaultChannel;
            await channel.SendMessageAsync($"Halloa, {user.Mention}.\nYou have my sincere condolences for joining {guild.Name}");
        }

        public async Task SendMessage(ISocketMessageChannel channel, string message)
        {
            await channel.SendMessageAsync(message);
        }

        private Task Log(LogMessage arg)
        {
            Console.WriteLine(arg);

            return Task.CompletedTask;
        }

        public async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;

            if (message is null || message.Author.IsBot) return;

            int argPos = 0;

            if (message.HasStringPrefix("]", ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var context = new SocketCommandContext(_client, message);

                var result = await _commands.ExecuteAsync(context, argPos, _services);

                if (!result.IsSuccess)
                    Console.WriteLine(result.ErrorReason);
            }
        }
    }
}
