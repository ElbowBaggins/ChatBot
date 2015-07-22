using System;
using System.Collections.Generic;
using System.Linq;
using Meebey.SmartIrc4net;

namespace Hemogoblin.ChatBot
{
    public class ChatBot
    {
        private IrcClient Client { get; set; }
        private readonly List<IChatBotResponder> responderList;
        private readonly string[] initialChannels;

        public string[] Addresses { get; private set; }
        public int Port { get; private set; }

        public string[] Nicks { get; private set; }
        public string Username { get; private set; }
        public string RealName { get; private set; }
        public bool ReceiveWallops { get; private set; }

        public ChatBot(IChatBotConfig chatBotConfig)
        {
            responderList = new List<IChatBotResponder>();
            Addresses = chatBotConfig.Addresses;
            Port = chatBotConfig.Port;

            Nicks = chatBotConfig.Nicks;
            Username = chatBotConfig.Username;
            RealName = chatBotConfig.RealName;
            ReceiveWallops = chatBotConfig.ReceiveWallops;

            initialChannels = chatBotConfig.Channels;

            Client = new IrcClient
            {
                ActiveChannelSyncing = chatBotConfig.SyncChannelsOnJoin,
                AutoNickHandling = chatBotConfig.AutoHandleNickCollision,
                AutoReconnect = chatBotConfig.AutoReconnect,
                AutoRelogin = chatBotConfig.AutoReconnect,
                AutoRejoin = chatBotConfig.AutoReconnect,
                AutoRejoinOnKick = chatBotConfig.AutoRejoin,
                AutoJoinOnInvite = chatBotConfig.AutoJoinOnInvite,
                AutoRetry = chatBotConfig.AutoRetryConnection,
                AutoRetryDelay = chatBotConfig.AutoRetryDelay,
                AutoRetryLimit = chatBotConfig.AutoRetryLimit,
                CtcpVersion = chatBotConfig.VersionResponse,
                SupportNonRfc = chatBotConfig.SupportNonRFC,
                UseSsl = chatBotConfig.UseSSL
            };

            // Logs client in once connected
            Client.OnConnected += LoginOnceConnected;
            Client.OnPing += DoPong;
            Client.OnRegistered += DoJoins;
            Client.OnChannelMessage += MessageHandler;
            Client.OnRawMessage += ShowAllRawIn;
            Client.OnWriteLine += ShowAllRawOut;
        }

        public void Go()
        {
            Client.Connect(Addresses, Port);
            Client.Listen();
        }

        public void RegisterListener(IChatBotResponder chatBotResponder)
        {
            responderList.Add(chatBotResponder);
        }

        private void LoginOnceConnected(object sender, EventArgs eventArgs)
        {
            Client.Login(Nicks, RealName, (ReceiveWallops ? 0 : 4), Username);
        }

        public void DoPong(object sender, PingEventArgs eventArgs)
        {
            Client.RfcPong(eventArgs.PingData);
        }

        public void DoJoins(object sender, EventArgs eventArgs)
        {
            foreach (var channel in initialChannels)
            {
                Client.RfcJoin(channel);
            }
        }

        private void MessageHandler(object sender, IrcEventArgs eventArgs)
        {
            var incoming = eventArgs.Data.Message;
            var from = eventArgs.Data.Channel;
            foreach (var responder in responderList.Where(responder => incoming.StartsWith(responder.Trigger)))
            {
                Client.SendMessage(SendType.Message, @from, responder.GetResponse(incoming));
            }
        }

        private static void ShowAllRawIn(object sender, IrcEventArgs eventArgs)
        {
            Console.WriteLine(eventArgs.Data.RawMessage);
        }

        private static void ShowAllRawOut(object sender, WriteLineEventArgs eventArgs)
        {
            Console.WriteLine(eventArgs.Line);
        }
    }
}
