using System.ComponentModel;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DNetPlus_InteractiveButtons
{
    public class EnsureFromUserCriterion : ICriterion<IMessage>
    {
        private readonly ulong _id;

        public EnsureFromUserCriterion(IUser user)
            => _id = user.Id;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public EnsureFromUserCriterion(ulong id)
            => _id = id;

        public Task<bool> JudgeAsync(SocketCommandContext sourceContext, Interaction interaction)
        {
            bool ok = _id == sourceContext.User.Id;
            return Task.FromResult(ok);
        }
    }
}
