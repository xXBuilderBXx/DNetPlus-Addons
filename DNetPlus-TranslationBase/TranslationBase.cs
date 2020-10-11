using Discord.Commands;

namespace DNetPlus_TranslationBase
{
    public abstract class TranslationBase<T1> : ModuleBase<TranslationContext>
    {
       public T1 Lang { get; set; }

       
        protected override void BeforeExecute(CommandInfo command)
        {
            if (!(Lang.GetType() == typeof(ITranslation)))
            {
                if (ITranslation.Translations.ContainsKey(Context.Language))
                    Lang = ITranslation.Translations[Context.Language].ToObject<T1>();
            }
        }
    }
}
