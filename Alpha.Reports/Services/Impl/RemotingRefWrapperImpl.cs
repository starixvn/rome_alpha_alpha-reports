namespace Alpha.Reports.Services.Impl
{
    using System;
    using System.Threading;
    using Constants;
    using RemotingRef;

    public class RemotingRefWrapperImpl : RemotingRefWrapper
    {
        private Func<string, string, string, string, string> getResourceDataByRefIdAndResourceIdWithDefaultValue = RefData.GetResourceDataByRefIdAndResourceId;

        private string CurrentLanguageCode
        {
            get
            {
                var currentLanguage = Thread.CurrentThread.CurrentCulture.Name;
                return ShortLanguageCode.CodeFromCurrentLanguageMapping.ContainsKey(currentLanguage)
                    ? ShortLanguageCode.CodeFromCurrentLanguageMapping[currentLanguage]
                    : ShortLanguageCode.Default;
            }
        }

        public void SetRemotingRefFunctions(Func<string, string, string, string, string> getResourceDataByRefIdAndResourceIdWithDefaultValueFunc = null)
        {
            getResourceDataByRefIdAndResourceIdWithDefaultValue = getResourceDataByRefIdAndResourceIdWithDefaultValueFunc;
        }

        public string GetProductName(int productId)
        {
            return getResourceDataByRefIdAndResourceIdWithDefaultValue("Product", productId.ToString(), CurrentLanguageCode, string.Empty);
        }
    }
}
