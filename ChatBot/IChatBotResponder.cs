namespace Hemogoblin.ChatBot
{
    public interface IChatBotResponder
    {
        string Trigger { get; }
        string GetResponse(string triggerMessage);
    }
}