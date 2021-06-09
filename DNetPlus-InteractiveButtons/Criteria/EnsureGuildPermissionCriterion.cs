using System.ComponentModel;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DNetPlus_InteractiveButtons
{
    public class EnsureGuildPermissionCriterion : ICriterion<IMessage>
    {
        private readonly GuildPermission _perm;

        public EnsureGuildPermissionCriterion(GuildPermission perm)
            => _perm = perm;


        public Task<bool> JudgeAsync(SocketCommandContext sourceContext, Interaction interaction)
        {
            bool ok = false;
            if (sourceContext.GuildUser != null && sourceContext.GuildUser.GuildPermissions.Has(_perm))
                ok = true;
            return Task.FromResult(ok);
        }
    }
}
