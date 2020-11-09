using System;
using System.Collections.Generic;
using Discord;
using Discord.WebSocket;

namespace Interactivity.Selection
{
    /// <summary>
    /// Represents a <see langword="abstract"/> class for creating custom selection builders.
    /// </summary>
    /// <typeparam name="T">The type of values to select from.</typeparam>
    /// <typeparam name="T1">The way of selecting in discord. Either <see cref="SocketMessage"/> or <see cref="SocketReaction"/>.</typeparam>
    /// <typeparam name="TAppearanceBuilder">The custom appearance builder of the selection.</typeparam>
    /// <typeparam name="TAppearance">The custom appearance of the selection.</typeparam>
    public abstract class SelectionBuilder<T, T1>
        where T1 : class
    {
        #region Fields
        /// <summary>
        /// Gets or sets the values to select from.
        /// </summary>
        public List<T> Values { get; set; } = new List<T>();

        /// <summary>
        /// Gets or sets the users who can interact with the <see cref="Selection{T, T1}"/>.
        /// </summary>
        public List<SocketUser> Users { get; set; } = new List<SocketUser>();

        /// <summary>
        /// Gets or sets what the <see cref="Selection{T, T1}"/> should delete.
        /// </summary>
        public DeletionOptions Deletion { get; set; } = DeletionOptions.Invalids;

        /// <summary>
        /// Gets or sets the selection embed of the <see cref="Selection{T, T1}"/>.
        /// </summary>
        public EmbedBuilder SelectionEmbed { get; set; } = new EmbedBuilder().WithColor(Color.Blue);

        /// <summary>
        /// Gets or sets the embed which the selection embed gets modified to after the <see cref="Selection{T, T1}"/> has been cancelled.
        /// </summary>
        public EmbedBuilder CancelledEmbed { get; set; } = new EmbedBuilder().WithColor(Color.Orange).WithTitle("Cancelled! :thumbsup:");

        /// <summary>
        /// Gets or sets the embed which the selection embed gets modified to after the <see cref="Selection{T, T1}"/> has timed out.
        /// </summary>
        public EmbedBuilder TimeoutedEmbed { get; set; } = new EmbedBuilder().WithColor(Color.Red).WithTitle("Timed out! :alarm_clock:");

        /// <summary>
        /// Gets whether the selection is user restricted.
        /// </summary>
        public bool IsUserRestricted => Users.Count > 0;
        #endregion

        #region Constructor
        protected SelectionBuilder()
        {
            if (typeof(T1) != typeof(SocketReaction) && typeof(T1) != typeof(SocketMessage))
            {
                throw new InvalidOperationException($"{nameof(T1)} can ONLY be SocketMessage or SocketReaction!");
            }
        }
        #endregion

        #region Build
        /// <summary>
        /// Build the <see cref="SelectionBuilder{T, T1}"/> to a immutable <see cref="Selection{T, T1}"/>.
        /// </summary>
        /// <returns></returns>
        public abstract Selection<T, T1> Build();
        #endregion
    }
}
