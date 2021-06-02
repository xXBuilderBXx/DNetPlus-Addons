using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DNetPlus_InteractiveButtons
{
    public class InteractiveButtonsService
    {
        public BaseSocketClient Discord { get; }

        private TimeSpan _defaultTimeout;

        // helpers to allow DI containers to resolve without a custom factory
        public InteractiveButtonsService(DiscordSocketClient discord)
            : this((BaseSocketClient)discord) { }

        public InteractiveButtonsService(DiscordShardedClient discord)
            : this((BaseSocketClient)discord) { }

        public InteractiveButtonsService(BaseSocketClient discord)
        {
            Discord = discord;

            //config = config ?? new InteractiveServiceConfig();
           // _defaultTimeout = config.DefaultTimeout;
        }

        public Task<InteractionData> NextButtonAsync(SocketCommandContext context, IUserMessage currentMessage,
           bool fromSourceUser = true,
           TimeSpan? timeout = null,
           CancellationToken token = default(CancellationToken))
        {
            var criterion = new Criteria<SocketMessage>();
            if (fromSourceUser)
                criterion.AddCriterion(new EnsureFromUserCriterion(context.User.Id));
            //if (inSourceChannel)
            //    criterion.AddCriterion(new EnsureSourceChannelCriterion());
            return NextButtonAsync(context, currentMessage, criterion, timeout, token);
        }

        public async Task<InteractionData> NextButtonAsync(SocketCommandContext context, IUserMessage currentMessage,
            ICriterion<SocketMessage> criterion,
            TimeSpan? timeout = null,
            CancellationToken token = default(CancellationToken))
        {
            timeout = timeout ?? _defaultTimeout;

            var eventTrigger = new TaskCompletionSource<InteractionData>();
            var cancelTrigger = new TaskCompletionSource<bool>();

            token.Register(() => cancelTrigger.SetResult(true));

            async Task Handler(Interaction interaction)
            {
                    bool result = await criterion.JudgeAsync(context, interaction).ConfigureAwait(false);
                    if (result)
                        eventTrigger.SetResult(interaction.Data);
            }

            context.Client.InteractionReceived += Handler;

            var trigger = eventTrigger.Task;
            var cancel = cancelTrigger.Task;
            var delay = Task.Delay(timeout.Value);
            var task = await Task.WhenAny(trigger, delay, cancel).ConfigureAwait(false);

            context.Client.InteractionReceived -= Handler;

            if (task == trigger)
                return await trigger.ConfigureAwait(false);
            else
                return null;
        }
    }
}
