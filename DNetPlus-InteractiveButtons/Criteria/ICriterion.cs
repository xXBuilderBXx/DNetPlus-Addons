using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNetPlus_InteractiveButtons
{
    public interface ICriterion<in T>
    {
        Task<bool> JudgeAsync(SocketCommandContext sourceContext, Interaction interaction);
    }
}
