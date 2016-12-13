namespace Alpha.Reports.Services
{
    using System;
    using Fanex.Data;

    public static class ObjectDbFactory
    {
        private static Func<string, IObjectDb> createObjectDbFunc;

        public static Func<string, IObjectDb> CreateObjectDbFunc
        {
            get { return createObjectDbFunc ?? ((settingKey) => new ObjectDb(settingKey)); }
            set { createObjectDbFunc = value; }
        }
    }
}
