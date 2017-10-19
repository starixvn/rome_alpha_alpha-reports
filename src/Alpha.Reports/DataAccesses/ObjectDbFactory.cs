namespace Alpha.Reports.DataAccesses
{
    using System;
    using Fanex.Data;

    public static class ReportObjectDbFactory
    {
        [ThreadStatic]
        private static Func<string, IObjectDb> createInstance;

        public static Func<string, IObjectDb> CreateInstance
        {
            get { return createInstance ?? ((settingKey) => ObjectDbFactory.CreateInstance(settingKey)); }
            internal set { createInstance = value; }
        }
    }
}