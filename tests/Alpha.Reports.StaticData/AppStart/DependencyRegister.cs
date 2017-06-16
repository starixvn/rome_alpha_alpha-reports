using Alpha.IntegrationTest.Mapper;
using Alpha.IntegrationTest.Repository;
using Alpha.Reports.StaticData.Configurations;
using Fanex.Data.Repository;
using SimpleInjector;

namespace Alpha.Reports.StaticData.AppStart
{
    public static class DependencyRegister
    {
        public static void Register(Container container)
        {
            container.Register<IMapperForStaticData, MapperForStaticData>(Lifestyle.Singleton);
            container.Register<IDynamicRepository, StaticRepository>(Lifestyle.Singleton);
        }
    }
}
