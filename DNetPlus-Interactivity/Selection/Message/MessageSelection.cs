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
    /// Represents the default selection which uses messages. This class is immutable!
    /// </summary>
    /// <typeparam name="T">The type of values to select from.</typeparam>
    public sealed class MessageSelection<T> : Selection<T, SocketMessage>
    {
        #region Fields
        /// <summary>
        /// The possibilites to select from.
        /// </summary>
        public IReadOnlyCollection<string> Possibilities { get; }

        /// <summary>
        /// Gets the cancel display name if cancel is enabled in the selection.
        /// </summary>
        public string CancelDisplayName { get; }
        #endregion

        #region Constructor
        internal MessageSelection(IReadOnlyCollection<T> values, IReadOnlyCollection<SocketUser> users,
            Embed selectionEmbed, Embed cancelledEmbed, Embed timeoutedEmbed, DeletionOptions deletion,
            IReadOnlyCollection<string> possabilies, string cancelDisplayName)
            : base(values, users, selectionEmbed, cancelledEmbed, timeoutedEmbed, deletion)
        {
            Possibilities = possabilies;
            CancelDisplayName = cancelDisplayName;
        }
        #endregion

        #region Methods
        public override Task<Optional<InteractivityResult<T>>> ParseAsync(SocketMessage value, DateTime startTime)
        {
            int index = Possibilities.FindIndex(x => x == value.Content) / 4;

            return Task.FromResult(Optional.Create(
                index >= Values.Count
                ? new InteractivityResult<T>(default, value.Timestamp.UtcDateTime - startTime, false, true)
                : new InteractivityResult<T>(Values.ElementAt(index), value.Timestamp.UtcDateTime - startTime, false, false)
                ));
        }

        public override Task<bool> RunChecksAsync(BaseSocketClient client, SocketMessage value)
            => Task.FromResult(Possibilities.Contains(value.Content));
        #endregion
    }
}
