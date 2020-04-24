using AutoMapper;
using SRS.Lookups;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SRS.Process;

namespace SRS.Data
{
    internal class LoadLookupData
    {
        //Reference to logger
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //Set up connection
        private readonly MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["GCIMS"].ToString());

        private readonly MySqlCommand cmd = new MySqlCommand();

        private readonly IMapper lookupMapper;


        public LoadLookupData(IMapper mapper)
        {
            lookupMapper = mapper;

            lookupMapper.ConfigurationProvider.CompileMappings();
        }


        public Lookup GetContractorLookupData()
        {
            Lookup lookups = new Lookup();

            try
            {
                using (conn)
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    using (cmd)
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "CORS_Expiring_Contracts";

                        MySqlDataReader lookupData = cmd.ExecuteReader();

                        using (lookupData)
                        {
                            if (lookupData.HasRows)
                                lookups = MapContractorLookupData(lookupData);
                        }
                    }
                }

                return lookups;
            }
            catch (Exception ex)
            {
                log.Error("Something went wrong" + " - " + ex.Message + " - " + ex.InnerException);

                return lookups;
            }
        }

        private Lookup MapContractorLookupData(MySqlDataReader lookupData)
        {
            Lookup lookup = new Lookup();

            //lookup_country
            lookupData.NextResult();
            lookup.countryLookup = lookupMapper.Map<IDataReader, List<CountryLookup>>(lookupData);

            //lookup_state
            lookupData.NextResult();
            lookup.stateLookup = lookupMapper.Map<IDataReader, List<StateLookup>>(lookupData);

            //lookup_region
            lookupData.NextResult();
            lookup.regionLookup = lookupMapper.Map<IDataReader, List<RegionLookup>>(lookupData);

            //lookup_building
            lookupData.NextResult();
            lookup.BuildingLookup = lookupMapper.Map<IDataReader, List<BuildingLookup>>(lookupData);

            return lookup;
        }
    }
}
