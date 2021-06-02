using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DNetPlus_InteractiveButtons
{
    public class EmptyCriterion<T> : ICriterion<T>
    {
        public Task<bool> JudgeAsync(SocketCommandContext sourceContext, Interaction interaction)
            => Task.FromResult(true);
    }
}
