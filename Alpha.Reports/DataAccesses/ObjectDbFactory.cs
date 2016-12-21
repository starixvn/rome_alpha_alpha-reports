namespace Alpha.Reports.DataAccesses
{
    using System;
    using Fanex.Data;

    public static class ObjectDbFactory
    {
        private static Func<string, IObjectDb> createInstance;

        public static Func<string, IObjectDb> CreateInstance
        {
            get { return createInstance ?? ((settingKey) => new ObjectDb(settingKey)); }
            set { createInstance = value; }
        }
    }
}
