using Alpha.Reports.StaticData.Configurations;
using Alpha.StaticData.Mappers;
using Alpha.StaticData.Repositories;
using Fanex.Data.Repository;
using SimpleInjector;

namespace Alpha.Reports.StaticData.AppStart
{
    public static class DependencyRegister
    {
        public static void Register(Container container)
        {
            container.Register<IRepositoryMapper, RepositoryMapper>(Lifestyle.Singleton);
            container.Register<IDynamicRepository, StaticRepository>(Lifestyle.Singleton);
        }
    }
}
