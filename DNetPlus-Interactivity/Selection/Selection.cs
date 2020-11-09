using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Interactivity.Extensions;

namespace Interactivity.Selection
{
    /// <summary>
    /// Represents a <see langword="abstract"/> class which allows for user selection in discord.
    /// </summary>
    /// <typeparam name="T">The type of values to select from.</typeparam>
    /// <typeparam name="T1">The way of selecting in discord. Either <see cref="SocketMessage"/> or <see cref="SocketReaction"/>.</typeparam>
    /// <typeparam name="TAppearance">The custom appearance of the selection.</typeparam>
    public abstract class Selection<T, T1> where T1 : class
    {
        #region Fields
        /// <summary>
        /// Gets the values to select from in the <see cref="Selection{T, T1}"/>.
        /// </summary>
        public IReadOnlyCollection<T> Values { get; }

        /// <summary>
        /// Gets which users can interact with the <see cref="Selection{T, T1}"/>.
        /// </summary>
        public IReadOnlyCollection<SocketUser> Users { get; }

        /// <summary>
        /// Gets the <see cref="Embed"/> which is sent into the channel.
        /// </summary>
        public Embed SelectionEmbed { get; }

        /// <summary>
        /// Gets the <see cref="EmbedBuilder"/> which the <see cref="Selection{T}"/> gets modified to after cancellation.
        /// </summary>
        public Embed CancelledEmbed { get; }

        /// <summary>
        /// Gets the <see cref="EmbedBuilder"/> which the <see cref="Selection{T}"/> gets modified to after a timeout.
        /// </summary>
        public Embed TimeoutedEmbed { get; }

        /// <summary>
        /// Gets an ORing determiting what the <see cref="Selection{T1, T2}"/> will delete.
        /// </summary>
        public DeletionOptions Deletion { get; }

        /// <summary>
        /// Determites whether everyone can interact with the <see cref="Selection{T, T1}"/>.
        /// </summary>
        public bool IsUserRestricted => Users.Count > 0;
        #endregion

        #region Constructor
        protected Selection(IReadOnlyCollection<T> values, IReadOnlyCollection<SocketUser> users,
            Embed selectionEmbed, Embed cancelledEmbed, Embed timeoutedEmbed,
            DeletionOptions deletion)
        {
            if (typeof(T1) != typeof(SocketReaction) && typeof(T1) != typeof(SocketMessage))
            {
                throw new InvalidOperationException("T2 can ONLY be SocketMessage or SocketReaction!");
            }

            Values = values;
            Users = users;
            SelectionEmbed = selectionEmbed;
            CancelledEmbed = cancelledEmbed;
            TimeoutedEmbed = timeoutedEmbed;
            Deletion = deletion;
        }
        #endregion

        #region Methods
        internal async Task<bool> HandleResponseAsync(BaseSocketClient client, T1 response)
        {
            bool valid = false;

            if (response is SocketMessage s)
            {
                valid = await RunChecksAsync(client, response).ConfigureAwait(false) && (IsUserRestricted || Users.Contains(s.Author));
                if (Deletion.HasFlag(DeletionOptions.Invalids) == true && !valid)
                {
                    await s.DeleteAsync().ConfigureAwait(false);
                }
                if (Deletion.HasFlag(DeletionOptions.Valid) == true && valid == true)
                {
                    await s.DeleteAsync().ConfigureAwait(false);
                }
            }
            if (response is SocketReaction r)
            {
                var user = r.User.Value as SocketUser ?? client.GetUser(r.UserId);
                valid = await RunChecksAsync(client, response).ConfigureAwait(false) && (IsUserRestricted || Users.Contains(user));
                if (Deletion.HasFlag(DeletionOptions.Invalids) == true && !valid)
                {
                    await r.DeleteAsync(client).ConfigureAwait(false);
                }
                if (Deletion.HasFlag(DeletionOptions.Valid) == true && valid == true)
                {
                    await r.DeleteAsync(client).ConfigureAwait(false);
                }
            }
            return valid;
        }

        /// <summary>
        /// Do something to the message before the actual selection starts. E.g. adding reactions...
        /// </summary>
        /// <param name="message">The selection message.</param>
        /// <returns></returns>
        public virtual Task InitializeMessageAsync(IUserMessage message)
            => Task.CompletedTask;

        /// <summary>
        /// Run additional checks on the <see cref="T1"/> to decide whether it is parseable or not.
        /// </summary>
        /// <param name="client">The standard discord client.</param>
        /// <param name="message">The selection message.</param>
        /// <param name="value">The value to run checks on.</param>
        /// <returns></returns>
        public virtual Task<bool> RunChecksAsync(BaseSocketClient client, T1 value)
            => Task.FromResult(true);

        /// <summary>
        /// Try to parse the user input to a <see cref="InteractivityResult{T}"/>. 
        /// </summary>
        /// <param name="value">The value to parse</param>
        /// <returns></returns>
        public abstract Task<Optional<InteractivityResult<T>>> ParseAsync(T1 value, DateTime startTime);
        #endregion
    }
}
