using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SRS.Lookups;
using SRS.Models;

namespace SRS.Mapping
{
    internal class SRSMapper
    {
        private MapperConfiguration lookupConfig;
        private MapperConfiguration dataConfig;

        public void CreateLookupConfig()
        {
            lookupConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Lookup, CountryLookup>().ReverseMap();
                cfg.CreateMap<Lookup, StateLookup>().ReverseMap();
                cfg.CreateMap<Lookup, RegionLookup>().ReverseMap();

                // cfg.AddDataReaderMapping();
                cfg.AllowNullCollections = true;
            });
        }

        public void CreateDataConfig()
        {
            dataConfig = new MapperConfiguration(cfg =>
            {
                // cfg.AddDataReaderMapping();
                cfg.AllowNullCollections = true;

                cfg.CreateMap<Contractor, Person>().ReverseMap();
                cfg.CreateMap<Contractor, Address>().ReverseMap();
                cfg.CreateMap<Contractor, Birth>().ReverseMap();
                cfg.CreateMap<Contractor, Position>().ReverseMap();
                cfg.CreateMap<Contractor, Phone>().ReverseMap();
                cfg.CreateMap<Contractor, Building>().ReverseMap();
            });
        }

        public IMapper CreateLookupMapping()
        {
            return lookupConfig.CreateMapper();
        }

        public IMapper CreateDataMapping()
        {
            return dataConfig.CreateMapper();
        }
    }
}
