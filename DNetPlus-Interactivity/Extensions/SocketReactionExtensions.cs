﻿using System.Threading.Tasks;
using Discord.WebSocket;

namespace Interactivity.Extensions
{
    internal static partial class Extensions
    {
        public static async Task DeleteAsync(this SocketReaction reaction, BaseSocketClient client)
        {
            var channel = reaction.Channel;
            var message = reaction.Message.IsSpecified
                ? reaction.Message.Value
                :await channel.GetMessageAsync(reaction.MessageId).ConfigureAwait(false) as SocketUserMessage;
            var user = reaction.User.IsSpecified 
                ? reaction.User.Value
                : client.GetUser(reaction.UserId);

            await message.RemoveReactionAsync(reaction.Emote, user).ConfigureAwait(false);
        }
    }
}
