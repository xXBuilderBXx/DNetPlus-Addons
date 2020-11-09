using Discord;

namespace Interactivity.Extensions
{
    internal static partial class Extensions
    {
        public static EmbedFieldBuilder ToBuilder(this EmbedField field) => new EmbedFieldBuilder()
                .WithIsInline(field.Inline)
                .WithName(field.Name)
                .WithValue(field.Value);
    }
}
