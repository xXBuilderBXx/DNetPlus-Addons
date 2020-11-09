using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using Discord.WebSocket;
using Interactivity.Extensions;
using Interactivity.Pagination;
using sys = System.Drawing;

namespace Interactivity
{
    /// <summary>
    /// The <see cref="PageBuilder"/> is used to create a <see cref="Page"/>.
    /// </summary>
    public class PageBuilder
    {
        /// <summary>
        /// Gets or sets the Text of the <see cref="PageBuilder"/>
        /// </summary>
        public string Text { get; set; } = null;

        /// <summary>
        /// Gets or sets the Color of the <see cref="PageBuilder"/>.
        /// </summary>
        public sys.Color? Color { get; set; } = null;

        /// <summary>
        /// Gets or sets the Description of the <see cref="PageBuilder"/>.
        /// </summary>
        public string Description { get; set; } = null;

        /// <summary>
        /// Gets or sets the Title of the <see cref="PageBuilder"/>.
        /// </summary>
        public string Title { get; set; } = null;

        /// <summary>
        /// Gets or sets the Thumbnailurl of the <see cref="PageBuilder"/>.
        /// </summary>
        public string ThumbnailUrl { get; set; } = null;

        /// <summary>
        /// Gets or sets the ImageUrl of the <see cref="PageBuilder"/>.
        /// </summary>
        public string ImageUrl { get; set; } = null;

        /// <summary>
        /// Gets or sets the Fields of the <see cref="PageBuilder"/>.
        /// </summary>
        public List<EmbedFieldBuilder> Fields { get; set; } = new List<EmbedFieldBuilder>();

        internal EmbedFooterBuilder Footer { get; set; } = new EmbedFooterBuilder();

        /// <summary>
        /// Creates a new <see cref="PageBuilder"/> from an <see cref="Embed"/>.
        /// </summary>
        /// <param name="embed"></param>
        /// <returns></returns>
        public static PageBuilder FromEmbed(Embed embed)
            => new PageBuilder()
                .WithColor(embed.Color ?? Discord.Color.Default)
                .WithDescription(embed.Description)
                .WithTitle(embed.Title)
                .WithThumbnailUrl(embed.Thumbnail?.Url)
                .WithImageUrl(embed.Image?.Url)
                .WithFields(embed.Fields.Select(x => x.ToBuilder()));

        /// <summary>
        /// Creates a new <see cref="PageBuilder"/> from an <see cref="EmbedBuilder"/>.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static PageBuilder FromEmbedBuilder(EmbedBuilder builder)
            => new PageBuilder()
                .WithColor(builder.Color ?? Discord.Color.Default)
                .WithDescription(builder.Description)
                .WithTitle(builder.Title)
                .WithThumbnailUrl(builder.ThumbnailUrl)
                .WithImageUrl(builder.ImageUrl)
                .WithFields(builder.Fields);

        /// <summary>
        /// Sets the Text of the <see cref="PageBuilder"/>.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public PageBuilder WithText(string text)
        {
            Text = text;
            return this;
        }

        /// <summary>
        /// Sets the Color of the <see cref="PageBuilder"/>.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public PageBuilder WithColor(sys.Color color)
        {
            Color = color;
            return this;
        }

        /// <summary>
        /// Sets the Description of the <see cref="PageBuilder"/>.
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public PageBuilder WithDescription(string description)
        {
            Description = description;
            return this;
        }

        /// <summary>
        /// Sets the Title of the <see cref="PageBuilder"/>.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public PageBuilder WithTitle(string title)
        {
            Title = title;
            return this;
        }

        /// <summary>
        /// Sets the Thumbnailurl of the <see cref="PageBuilder"/>.
        /// </summary>
        /// <param name="thumbnailUrl"></param>
        /// <returns></returns>
        public PageBuilder WithThumbnailUrl(string thumbnailUrl)
        {
            ThumbnailUrl = thumbnailUrl;
            return this;
        }

        /// <summary>
        /// Sets the ImageUrl of the <see cref="PageBuilder"/>.
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        public PageBuilder WithImageUrl(string imageUrl)
        {
            ImageUrl = imageUrl;
            return this;
        }

        /// <summary>
        /// Sets the Fields of the <see cref="PageBuilder"/>.
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public PageBuilder WithFields(params EmbedFieldBuilder[] fields)
        {
            Fields = fields?.ToList();
            return this;
        }

        /// <summary>
        /// Sets the Fields of the <see cref="PageBuilder"/>.
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public PageBuilder WithFields(IEnumerable<EmbedFieldBuilder> fields)
        {
            Fields = fields?.ToList();
            return this;
        }

        /// <summary>
        /// Adds a field to the <see cref="PageBuilder"/>.
        /// </summary>
        /// <param name="name">The name of the field.</param>
        /// <param name="value">The value of the field.</param>
        /// <param name="isInline">Whether the field is Inline.</param>
        /// <returns></returns>
        public PageBuilder AddField(string name, object value, bool isInline = false)
        {
            if (Fields.Count == 24)
            {
                throw new InvalidOperationException("The field limit is 24!");
            }

            Fields.Add(new EmbedFieldBuilder()
            {
                Name = name,
                Value = value,
                IsInline = isInline
            });
            return this;
        }

        internal PageBuilder WithPaginatorFooter(PaginatorFooter footer, int page, int totalPages, IList<SocketUser> users)
        {
            if (footer.HasFlag(PaginatorFooter.None))
            {
                Footer = new EmbedFooterBuilder();
                return this;
            }

            if (footer.HasFlag(PaginatorFooter.Users))
            {
                if (users.Count == 0)
                {
                    Footer.Text += "Interactors : Everyone\n";
                }
                if (users.Count == 1)
                {
                    var user = users.First();

                    Footer.IconUrl = user.GetAvatarUrl();
                    Footer.Text += $"Interactor : {user}\n";
                }
                else
                {
                    Footer.Text += $"Interactors : {string.Join(", ", users)}";
                }
            }

            if (footer.HasFlag(PaginatorFooter.PageNumber))
            {
                Footer.Text += $"Page {page + 1}/{totalPages + 1}";
            }

            return this;
        }

        public Page Build(string defaultText = null, sys.Color? defaultColor = null,
            string defaultDescription = null, string defaultTitle = null, string defaultThumbnailUrl = null, string defaultImageUrl = null,
            List<EmbedFieldBuilder> defaultFields = null)
            => new Page(Text ?? defaultText,
                Color ?? defaultColor,
                Description ?? defaultDescription,
                Title ?? defaultTitle,
                ThumbnailUrl ?? defaultThumbnailUrl,
                ImageUrl ?? defaultImageUrl,
                Fields ?? defaultFields,
                Footer);
    }
}