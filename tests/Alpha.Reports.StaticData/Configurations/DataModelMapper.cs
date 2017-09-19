namespace Alpha.Reports.StaticData.Configurations
{
    using System.Collections.Generic;
    using Alpha.StaticData.Mappers;

    public class DataModelMapper : IDataModelMapper
    {
        public IDictionary<string, string> Maps
            => StaticDataMapper.ModelMaps;
    }
}
