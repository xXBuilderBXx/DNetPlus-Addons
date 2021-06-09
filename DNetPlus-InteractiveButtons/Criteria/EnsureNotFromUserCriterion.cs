using System.ComponentModel;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DNetPlus_InteractiveButtons
{
    public class EnsureNotFromUserCriterion : ICriterion<IMessage>
    {
        private readonly ulong _id;

        public EnsureNotFromUserCriterion(IUser user)
            => _id = user.Id;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public EnsureNotFromUserCriterion(ulong id)
            => _id = id;

        public Task<bool> JudgeAsync(SocketCommandContext sourceContext, Interaction interaction)
        {
            bool ok = _id != interaction.User.Id;
            return Task.FromResult(ok);
        }
    }
}
