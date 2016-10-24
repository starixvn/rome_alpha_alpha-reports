namespace Alpha.Reports.Constants
{
    using System.Collections.Generic;

    public static class ShortLanguageCode
    {
        public const string English = "en";
        public const string ChineseTraditional = "ch";
        public const string ChineseSimplified = "cs";
        public const string Japanese = "jp";
        public const string Korean = "ko";
        public const string Vietnamese = "vn";
        public const string Thai = "th";

        public const string Default = English;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Reviewed!")]
        public static readonly Dictionary<string, string> CodeFromCurrentLanguageMapping = new Dictionary<string, string>
        {
            { LanguageCode.English, English },
            { LanguageCode.ChineseTraditional, ChineseTraditional },
            { LanguageCode.ChineseSimplified, ChineseSimplified },
            { LanguageCode.Japanese, Japanese },
            { LanguageCode.Korean, Korean },
            { LanguageCode.Vietnamese, Vietnamese },
            { LanguageCode.Thai, Thai }
        };
    }
}
