namespace Alpha.Reports.Site.Tests.Helpers
{
    using System.Linq;
    using System.Reflection;

    public static class Reflect
    {
        private static MethodInfo GetMethod(object target, string name)
        {
            var type = target.GetType();
            var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            return methods.Single(x => x.Name == name);
        }

        public static object Invoke(object target, string name, params object[] args)
        {
            var method = GetMethod(target, name);
            return method.Invoke(target, args);
        }
    }
}
