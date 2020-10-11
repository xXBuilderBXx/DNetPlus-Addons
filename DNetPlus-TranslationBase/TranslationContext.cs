using Discord.Commands;
using Discord.WebSocket;

namespace DNetPlus_TranslationBase
{
    public class TranslationContext : SocketCommandContext
    {
        public string Language { get; set; } = "english";
        public TranslationContext(DiscordSocketClient client, SocketUserMessage message, string lang = "") : base(client, message)
        {
            Language = lang.ToLower();
        }
    }
}
