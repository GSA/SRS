using AutoMapper;
using AutoMapper.Data;

namespace SRS.Mapping
{
    internal class SRSMapper
    {
        
        private MapperConfiguration dataConfig;

         

        public void CreateDataConfig()
        {
            dataConfig = new MapperConfiguration(cfg =>
            {
                //cfg.AddDataReaderMapping();
                cfg.AllowNullCollections = true;

                //cfg.CreateMap<ExpiringContractorData, ExpiringContractorData>().ReverseMap();
                //cfg.CreateMap<ExpiredContractorData, ExpiredContractorData>().ReverseMap();

            });
        }
         
        public IMapper CreateDataMapping()
        {
            return dataConfig.CreateMapper();
        }
    }
}
