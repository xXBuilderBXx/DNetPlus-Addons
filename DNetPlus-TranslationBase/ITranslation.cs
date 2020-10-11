using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace DNetPlus_TranslationBase
{
    public class ITranslation
    {
        public string name;

        public static Dictionary<string, JObject> Translations = new Dictionary<string, JObject>();
    }
}
