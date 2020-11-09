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
    /// Represents a builder class for making a <see cref="MessageSelection{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of values to select from</typeparam>
    public sealed class MessageSelectionBuilder<T> : SelectionBuilder<T, SocketMessage>
    {
        #region Fields
        /// <summary>
        /// Gets or sets the function to convert the values into possibilites.
        /// </summary>
        public Func<T, string> StringConverter { get; set; } = x => x.ToString();

        /// <summary>
        /// Gets or sets the cancel display name if cancel is enabled in the <see cref="MessageSelection{T}"/>.
        /// </summary>
        public string CancelDisplayName { get; set; } = "Cancel";

        /// <summary>
        /// Gets or sets whether the <see cref="Selection{T, T1}"/> allows for cancellation.
        /// </summary>
        public bool AllowCancel { get; set; } = true;

        /// <summary>
        /// Gets or sets whether the selectionembed will be added by a default value visualizer.
        /// </summary>
        public bool EnableDefaultSelectionDescription { get; set; } = true;

        /// <summary>
        /// Gets or sets the title of the <see cref="Selection{T, T1}"/>.
        /// </summary>
        public string Title { get; set; } = "Select one of these";
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new <see cref="MessageSelectionBuilder{T}"/> with default values.
        /// </summary>
        public MessageSelectionBuilder() { }

        /// <summary>
        /// Creates a new <see cref="MessageSelectionBuilder{T}"/> with default values.
        /// </summary>
        public static MessageSelectionBuilder<T> Default() => new MessageSelectionBuilder<T>();
        #endregion

        #region Build
        /// <summary>
        /// Build the <see cref="MessageSelectionBuilder{T}"/> to a immutable <see cref="MessageSelection{T}"/>.
        /// </summary>
        /// <returns></returns>
        public override Selection<T, SocketMessage> Build()
        {
            if (Values.Count == 0)
            {
                throw new InvalidOperationException("Your Selection needs at least one value");
            }

            var possibilities = new List<string>();
            var sBuilder = new StringBuilder();

            for (int i = 0; i < Values.Count; i++)
            {
                string possibility = StringConverter.Invoke(Values[i]);
                sBuilder.AppendLine($"#{i + 1} - {possibility}");
                possibilities.Add($"{i + 1}");
                possibilities.Add($"#{i + 1}");
                possibilities.Add(possibility);
                possibilities.Add($"#{i + 1} - {possibility}");
            }
            if (AllowCancel == true)
            {
                sBuilder.Append($"#{Values.Count + 1} - {CancelDisplayName}");
                possibilities.Add($"{Values.Count + 1}");
                possibilities.Add($"#{Values.Count + 1}");
                possibilities.Add(CancelDisplayName);
                possibilities.Add($"#{Values.Count + 1} - {CancelDisplayName}");
            }

            if (EnableDefaultSelectionDescription == true)
            {
                SelectionEmbed.AddField(Title, sBuilder.ToString());
            }

            return new MessageSelection<T>(
                Values?.AsReadOnlyCollection() ?? throw new ArgumentNullException(nameof(Values)),
                Users?.AsReadOnlyCollection() ?? throw new ArgumentNullException(nameof(Users)),
                SelectionEmbed?.Build() ?? throw new ArgumentNullException(nameof(SelectionEmbed)),
                CancelledEmbed?.Build() ?? throw new ArgumentNullException(nameof(CancelledEmbed)),
                TimeoutedEmbed?.Build() ?? throw new ArgumentNullException(nameof(TimeoutedEmbed)),
                Deletion,
                possibilities?.AsReadOnlyCollection(),
                CancelDisplayName);
        }
        #endregion

        #region WithValue
        /// <summary>
        /// Sets the values to select from.
        /// </summary>
        public MessageSelectionBuilder<T> WithValues(IEnumerable<T> values)
        {
            Values = values.ToList();
            return this;
        }

        /// <summary>
        /// Sets the values to select from.
        /// </summary>
        public MessageSelectionBuilder<T> WithValues(params T[] values)
        {
            Values = values.ToList();
            return this;
        }

        /// <summary>
        /// Sets the users who can interact with the <see cref="Selection{T, T1}"/>.
        /// </summary>
        public MessageSelectionBuilder<T> WithUsers(IEnumerable<SocketUser> users)
        {
            Users = users.ToList();
            return this;
        }

        /// <summary>
        /// Sets the users who can interact with the <see cref="Selection{T, T1}"/>.
        /// </summary>
        public MessageSelectionBuilder<T> WithUsers(params SocketUser[] users)
        {
            Users = users.ToList();
            return this;
        }

        /// <summary>
        /// Sets what the <see cref="Selection{T, T1}"/> should delete.
        /// </summary>
        public MessageSelectionBuilder<T> WithDeletion(DeletionOptions deletion)
        {
            Deletion = deletion;
            return this;
        }

        /// <summary>
        /// Sets the selection embed of the <see cref="Selection{T, T1}"/>.
        /// </summary>
        public MessageSelectionBuilder<T> WithSelectionEmbed(EmbedBuilder embed)
        {
            SelectionEmbed = embed;
            return this;
        }

        /// <summary>
        /// Sets the embed which the selection embed gets modified to after the <see cref="Selection{T, T1}"/> has been cancelled.
        /// </summary>
        public MessageSelectionBuilder<T> WithCancelledEmbed(EmbedBuilder cancelledEmbed)
        {
            CancelledEmbed = cancelledEmbed;
            return this;
        }

        /// <summary>
        /// Sets the embed which the selection embed gets modified to after the <see cref="Selection{T, T1}"/> has timed out.
        /// </summary>
        public MessageSelectionBuilder<T> WithTimeoutedEmbed(EmbedBuilder timeoutedEmbed)
        {
            TimeoutedEmbed = timeoutedEmbed;
            return this;
        }

        /// <summary>
        /// Sets the function to convert the values into possibilites.
        /// </summary>
        public MessageSelectionBuilder<T> WithStringConverter(Func<T, string> stringConverter)
        {
            StringConverter = stringConverter;
            return this;
        }

        /// <summary>
        /// Sets the cancel display name if cancel is enabled in the <see cref="MessageSelection{T}"/>.
        /// </summary>
        public MessageSelectionBuilder<T> WithCancelDisplayName(string cancelDisplayName)
        {
            CancelDisplayName = cancelDisplayName;
            return this;
        }

        /// <summary>
        /// Sets whether the <see cref="Selection{T, T1}"/> allows for cancellation.
        /// </summary>
        public MessageSelectionBuilder<T> WithAllowCancel(bool allowCancel)
        {
            AllowCancel = allowCancel;
            return this;
        }

        /// <summary>
        /// Sets whether the selectionembed will be added by a default value visualizer.
        /// </summary>
        public MessageSelectionBuilder<T> WithEnableDefaultSelectionDescription(bool enableDefaultSelectionDescription)
        {
            EnableDefaultSelectionDescription = enableDefaultSelectionDescription;
            return this;
        }

        /// <summary>
        /// Sets the title of the <see cref="Selection{T, T1}"/>.
        /// </summary>
        public MessageSelectionBuilder<T> WithTitle(string title)
        {
            Title = title;
            return this;
        }
        #endregion
    }
}
