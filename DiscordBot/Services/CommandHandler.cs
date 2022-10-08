using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Interactions;
using System.Collections.Generic;

namespace DiscordBot.Services
{
    internal class CommandHandler
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

       

        public static Task Main(string[] args) => new CommandHandler().MainAsync();

        private CommandHandler()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info,
            });

            _commands = new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Info,
                CaseSensitiveCommands = false,
            });

            _client.Log += Log;
            _commands.Log += Log;
            _client.MessageReceived += RulesChannelHandler;
            _client.MessageReceived += VerifyChannelHandler;
            _client.MessageReceived += GlobalChannelHandler;
            _client.MessageReceived += NavigationChannelHandler;
            _client.MessageReceived += NewsServerChannelHandler;
            _client.MessageReceived += HandleCommandAsync;


        }

        private async Task RulesChannelHandler(SocketMessage msg)
        {
            var message = msg as SocketUserMessage;
            int pos = 0;
            ulong idAll = 1024983123143970870;
            var chnl = _client.GetChannel(idAll) as IMessageChannel;
            if (!message.Author.IsBot && message.Channel == chnl) await chnl.SendMessageAsync("There are Rules!");                     
        }
        private async Task VerifyChannelHandler(SocketMessage msg)
        {
            ulong idAll = 1025469570028408993;
            var chnl = _client.GetChannel(idAll) as IMessageChannel;
            if (!msg.Author.IsBot && msg.Channel == chnl) await chnl.SendMessageAsync("There are Verify!");           
        }
        private async Task GlobalChannelHandler(SocketMessage msg)
        {
            ulong idAll = 1024983123143970869;
            var chnl = _client.GetChannel(idAll) as IMessageChannel;
            if (!msg.Author.IsBot && msg.Channel == chnl) await chnl.SendMessageAsync("There are Global!");           
        }
        private async Task NavigationChannelHandler(SocketMessage msg)
        {
            ulong idAll = 1025478403509469256;
            var chnl = _client.GetChannel(idAll) as IMessageChannel;
            if (!msg.Author.IsBot && msg.Channel == chnl) await chnl.SendMessageAsync("There are Navigation!");           
        }
        private async Task NewsServerChannelHandler(SocketMessage msg)
        {
            ulong idAll = 1025480134444195900;
            var chnl = _client.GetChannel(idAll) as IMessageChannel;
            if (!msg.Author.IsBot && msg.Channel == chnl) await chnl.SendMessageAsync("There are News Server!");           
        }


       

        public async Task MainAsync()
        {
            Environment.SetEnvironmentVariable("TOKEN", "MTAyNTMzNzA1NDg3MzI2ODI4NA.GexhoQ.49TWevHmTeB1imQsfSeYrmKUpa9qtM8XrR1WAA");                    
            await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("TOKEN"));
            await _client.StartAsync();
            await Task.Delay(Timeout.Infinite);
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            
            var msg = arg as SocketUserMessage;
            if (msg == null) return;
          
            if (msg.Author.Id == _client.CurrentUser.Id || msg.Author.IsBot) return;
          
            int pos = 0;
 
            if (msg.HasCharPrefix('!', ref pos) /* || msg.HasMentionPrefix(_client.CurrentUser, ref pos) */)
            {              
                var context = new SocketCommandContext(_client, msg);

                var result = await _commands.ExecuteAsync(context, pos, _services);

                // Uncomment the following lines if you want the bot
                // to send a message if it failed.
                // This does not catch errors from commands with 'RunMode.Async',
                // subscribe a handler for '_commands.CommandExecuted' to see those.
                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                    await msg.Channel.SendMessageAsync(result.ErrorReason);
            }
        }

        private Task Log(LogMessage message)
        {
            switch (message.Severity)
            {
                case LogSeverity.Critical:
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogSeverity.Verbose:
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
            }

            Console.WriteLine($"{DateTime.Now,-19} [{message.Severity,8}] {message.Source}: {message.Message} {message.Exception}");
            Console.ResetColor();

            return Task.CompletedTask;
        }
    }
}
