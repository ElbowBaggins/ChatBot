namespace Hemogoblin.ChatBot
{
    public interface IChatBotConfig
    {
        // Server to connect to
        string[] Addresses { get; }
        int Port { get; }

        // Server settings
        bool AutoHandleNickCollision { get; }
        bool AutoJoinOnInvite { get; }
        bool AutoReconnect { get; }
        bool AutoRejoin { get; }
        bool AutoRetryConnection { get; }
        int AutoRetryDelay { get; }
        int AutoRetryLimit { get; }
        bool ReceiveWallops { get; }
        bool SupportNonRFC { get; }
        bool SyncChannelsOnJoin { get; }
        bool UseSSL { get; }
        string VersionResponse { get; }

        // Identity info
        string[] Nicks { get; }
        string Username { get; }
        string RealName { get; }
        
        // Post-connection commands
        string[] PostConnectionCommands { get; }

        // Channels to join
        string[] Channels { get; }
    }
}