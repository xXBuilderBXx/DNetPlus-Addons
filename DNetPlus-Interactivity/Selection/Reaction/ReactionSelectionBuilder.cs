using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Discord;
using Discord.WebSocket;
using Interactivity.Extensions;

namespace Interactivity.Selection
{
    /// <summary>
    /// Represents a builder class for making a <see cref="ReactionSelection{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of values to select from</typeparam>
    public sealed class ReactionSelectionBuilder<T> : SelectionBuilder<T, SocketReaction>
    {
        #region Fields
        /// <summary>
        /// Gets or sets the emotes which are used to select values.
        /// </summary>
        public List<IEmote> Emotes { get; set; } = new List<IEmote>();

        /// <summary>
        /// Gets or sets the function to convert the values into possibilites.
        /// </summary>
        public Func<T, string> StringConverter { get; set; } = x => x.ToString();

        /// <summary>
        /// Gets or sets the title of the selection.
        /// </summary>
        public string Title { get; set; } = "Select one of these:";

        /// <summary>
        /// Gets or sets whether the <see cref="Selection{T, T1}"/> allows for cancellation.
        /// </summary>
        public bool AllowCancel { get; set; } = true;

        /// <summary>
        /// Gets or sets the cancel emote if cancel is enabled in the <see cref="ReactionSelection{T}"/>.
        /// </summary>
        public IEmote CancelEmote { get; set; } = new Emoji("❌");

        /// <summary>
        /// Gets or sets whether the selectionembed will be added by a default value visualizer.
        /// </summary>
        public bool EnableDefaultSelectionDescription { get; set; } = true;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new <see cref="ReactionSelectionBuilder{T}"/> with default values.
        /// </summary>
        public ReactionSelectionBuilder()
        {
        }

        /// <summary>
        /// Creates a new <see cref="ReactionSelectionBuilder{T}"/> with default values.
        /// </summary>
        public static ReactionSelectionBuilder<T> Default => new ReactionSelectionBuilder<T>();
        #endregion

        #region Build
        /// <summary>
        /// Build the <see cref="ReactionSelectionBuilder{T}"/> to a immutable <see cref="ReactionSelection{T}"/>.
        /// </summary>
        /// <returns></returns>
        public override Selection<T, SocketReaction> Build()
        {
            if (Emotes.Count < Values.Count)
            {
                throw new InvalidOperationException("Value count larger than emote count! Please add more Emotes to the selection!");
            }
            if (Emotes.Contains(CancelEmote) == true)
            {
                throw new InvalidOperationException("Please remove the cancel emote from the selection emotes!");
            }

            if (EnableDefaultSelectionDescription == true)
            {
                var builder = new StringBuilder();

                for (int i = 0; i < Values.Count; i++)
                {
                    string possibility = StringConverter.Invoke(Values[i]);
                    builder.AppendLine($"{Emotes[i]} - {possibility}");
                }

                SelectionEmbed.AddField(Title, builder.ToString());
            }

            return new ReactionSelection<T>(
                Values?.AsReadOnlyCollection() ?? throw new ArgumentNullException(nameof(Values)),
                Users?.AsReadOnlyCollection() ?? throw new ArgumentNullException(nameof(Users)),
                SelectionEmbed?.Build() ?? throw new ArgumentNullException(nameof(SelectionEmbed)),
                CancelledEmbed?.Build() ?? throw new ArgumentNullException(nameof(CancelledEmbed)),
                TimeoutedEmbed?.Build() ?? throw new ArgumentNullException(nameof(TimeoutedEmbed)),
                Deletion,
                Emotes?.AsReadOnlyCollection() ?? throw new ArgumentNullException(nameof(Emotes)),
                CancelEmote ?? throw new ArgumentNullException(nameof(CancelEmote)),
                AllowCancel);
        }
        #endregion

        #region WithValue
        /// <summary>
        /// Sets the values to select from.
        /// </summary>
        public ReactionSelectionBuilder<T> WithValues(IEnumerable<T> values)
        {
            Values = values.ToList();
            return this;
        }

        /// <summary>
        /// Sets the values to select from.
        /// </summary>
        public ReactionSelectionBuilder<T> WithValues(params T[] values)
        {
            Values = values.ToList();
            return this;
        }

        /// <summary>
        /// Sets the users who can interact with the <see cref="Selection{T, T1}"/>.
        /// </summary>
        public ReactionSelectionBuilder<T> WithUsers(IEnumerable<SocketUser> users)
        {
            Users = users.ToList();
            return this;
        }

        /// <summary>
        /// Sets the users who can interact with the <see cref="Selection{T, T1}"/>.
        /// </summary>
        public ReactionSelectionBuilder<T> WithUsers(params SocketUser[] users)
        {
            Users = users.ToList();
            return this;
        }

        /// <summary>
        /// Sets what the <see cref="Selection{T, T1}"/> should delete.
        /// </summary>
        public ReactionSelectionBuilder<T> WithDeletion(DeletionOptions deletion)
        {
            Deletion = deletion;
            return this;
        }

        /// <summary>
        /// Sets the selection embed of the <see cref="Selection{T, T1}"/>.
        /// </summary>
        public ReactionSelectionBuilder<T> WithSelectionEmbed(EmbedBuilder embed)
        {
            SelectionEmbed = embed;
            return this;
        }

        /// <summary>
        /// Sets the embed which the selection embed gets modified to after the <see cref="Selection{T, T1}"/> has been cancelled.
        /// </summary>
        public ReactionSelectionBuilder<T> WithCancelledEmbed(EmbedBuilder cancelledEmbed)
        {
            CancelledEmbed = cancelledEmbed;
            return this;
        }

        /// <summary>
        /// Sets the embed which the selection embed gets modified to after the <see cref="Selection{T, T1}"/> has timed out.
        /// </summary>
        public ReactionSelectionBuilder<T> WithTimeoutedEmbed(EmbedBuilder timeoutedEmbed)
        {
            TimeoutedEmbed = timeoutedEmbed;
            return this;
        }

        /// <summary>
        /// Sets the emotes which are used to select values.
        /// </summary>
        public ReactionSelectionBuilder<T> WithEmotes(params IEmote[] emotes)
        {
            Emotes = emotes.Distinct().ToList();
            return this;
        }

        /// <summary>
        /// Sets the emotes which are used to select values.
        /// </summary>
        public ReactionSelectionBuilder<T> WithEmotes(IEnumerable<IEmote> emotes)
        {
            Emotes = emotes.ToList();
            return this;
        }

        /// <summary>
        /// Sets the function to convert the values into possibilites.
        /// </summary>
        public ReactionSelectionBuilder<T> WithStringConverter(Func<T, string> stringConverter)
        {
            StringConverter = stringConverter;
            return this;
        }

        /// <summary>
        /// Sets the title of the selection.
        /// </summary>
        public ReactionSelectionBuilder<T> WithTitle(string title)
        {
            Title = title;
            return this;
        }

        /// <summary>
        /// Sets whether the <see cref="Selection{T, T1}"/> allows for cancellation.
        /// </summary>
        public ReactionSelectionBuilder<T> WithAllowCancel(bool allowCancel)
        {
            AllowCancel = allowCancel;
            return this;
        }

        /// <summary>
        /// Sets the cancel emote if cancel is enabled in the <see cref="ReactionSelection{T}"/>.
        /// </summary>
        public ReactionSelectionBuilder<T> WithCancelEmote(IEmote cancelEmote)
        {
            CancelEmote = cancelEmote;
            return this;
        }

        /// <summary>
        /// Sets whether the selectionembed will be added by a default value visualizer.
        /// </summary>
        public ReactionSelectionBuilder<T> WithEnableDefaultSelectionDescription(bool enableDefaultSelectionDescription)
        {
            EnableDefaultSelectionDescription = enableDefaultSelectionDescription;
            return this;
        }
        #endregion
    }
}