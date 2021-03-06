﻿using Discord;
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

        public Task<InteractiveButtonData> NextButtonAsync(SocketCommandContext context, IUserMessage currentMessage,
           bool fromSourceUser = true,
           TimeSpan? timeout = null,
           CancellationToken token = default(CancellationToken))
        {
            var criterion = new Criteria<SocketMessage>();
            if (fromSourceUser)
                criterion.AddCriterion(new EnsureFromUserCriterion(context.User.Id));
            return NextButtonAsync(context, currentMessage, criterion, timeout, token);
        }

        public Task<InteractiveButtonData> NextButtonAsync(SocketCommandContext context, IUserMessage currentMessage,
           IUser user,
           TimeSpan? timeout = null,
           CancellationToken token = default(CancellationToken))
        {
            var criterion = new Criteria<SocketMessage>();
                criterion.AddCriterion(new EnsureFromUserCriterion(user));
            return NextButtonAsync(context, currentMessage, criterion, timeout, token);
        }

        public Task<InteractiveButtonData> NextButtonAsync(SocketCommandContext context, IUserMessage currentMessage,
          GuildPermission perm,
          TimeSpan? timeout = null,
          CancellationToken token = default(CancellationToken))
        {
            var criterion = new Criteria<SocketMessage>();
            criterion.AddCriterion(new EnsureGuildPermissionCriterion(perm));
            return NextButtonAsync(context, currentMessage, criterion, timeout, token);
        }

        public async Task<InteractiveButtonData> NextButtonAsync(SocketCommandContext context, IUserMessage currentMessage,
            ICriterion<SocketMessage> criterion,
            TimeSpan? timeout = null,
            CancellationToken token = default(CancellationToken))
        {
            timeout = timeout ?? _defaultTimeout;

            var eventTrigger = new TaskCompletionSource<InteractiveButtonData>();
            var cancelTrigger = new TaskCompletionSource<bool>();

            token.Register(() => cancelTrigger.SetResult(true));

            async Task Handler(Interaction interaction)
            {
                    bool result = await criterion.JudgeAsync(context, interaction).ConfigureAwait(false);
                if (result)
                {
                    InteractiveButtonData data = new InteractiveButtonData();
                    data.Update(interaction.Data, interaction.User);
                    eventTrigger.SetResult(data);
                }
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
