using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DNetPlus_InteractiveButtons
{
    public class Criteria<T> : ICriterion<T>
    {
        private List<ICriterion<T>> _critiera = new List<ICriterion<T>>();

        public Criteria<T> AddCriterion(ICriterion<T> criterion)
        {
            _critiera.Add(criterion);
            return this;
        }

        public async Task<bool> JudgeAsync(SocketCommandContext sourceContext, Interaction interaction)
        {
            if (interaction.Type != InteractionType.MessageComponent || interaction.MessageId.Value == sourceContext.Message.Id)
                return false;
            foreach (var criterion in _critiera)
            {
                var result = await criterion.JudgeAsync(sourceContext, interaction).ConfigureAwait(false);
                if (!result) return false;
            }
            return true;
        }
    }
}
