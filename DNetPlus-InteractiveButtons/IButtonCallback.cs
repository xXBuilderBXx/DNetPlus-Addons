using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNetPlus_InteractiveButtons
{
    public interface IButtonCallback
    {
        RunMode RunMode { get; }
        ICriterion<InteractionData> Criterion { get; }
        TimeSpan? Timeout { get; }
        SocketCommandContext Context { get; }

        Task<bool> HandleCallbackAsync(InteractionData interaction);
    }
}
