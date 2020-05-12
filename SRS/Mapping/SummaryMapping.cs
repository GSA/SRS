using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using SRS.Lookups;
using SRS.Models;

namespace SRS.Mapping
{
    internal sealed class ExpiringContractorSummaryMapping : ClassMap<ExpiringContractorSummary>
    {
        private Lookup lookups;

        public ExpiringContractorSummaryMapping()
        {
           // Map(m => m.contract_unid).Name("Contract unid");
            Map(m => m.PersID).Name("Person ID");
            Map(m => m.FirstName).Name("First Name");
            Map(m => m.MiddleName).Name("Middle Name");
            Map(m => m.LastName).Name("Last Name");
            Map(m => m.Suffix).Name("Suffix"); 
            Map(m => m.pers_work_email).Name("person POC Email");
            Map(m => m.RegionalEmail).Name("Reginal email"); 
            Map(m => m.pers_investigation_date).Name("SAC Expiration DATE"); 
            Map(m => m.DaysToExpiration).Name("DaysToExpiration");
            Map(m => m.conpoc_email).Name("vender email"); 
        }

        public ExpiringContractorSummaryMapping(Lookup lookups)
        {
            this.lookups = lookups;
        }
    }

    internal sealed class ExpiredContractorSummaryMapping : ClassMap<ExpiredContractorSummary>
    {
        public ExpiredContractorSummaryMapping()
        {
            Map(m => m.PersID).Name("Person ID"); 
            Map(m => m.FirstName).Name("First Name");
            Map(m => m.MiddleName).Name("Middle Name");
            Map(m => m.LastName).Name("Last Name");
            Map(m => m.Suffix).Name("Suffix");
            Map(m => m.pers_work_email).Name("person POC Email");
            Map(m => m.RegionalEmail).Name("Reginal email"); 
            Map(m => m.pers_investigation_date).Name("Contract date end"); 
            Map(m => m.DaysToExpiration).Name("DaysToExpiration");
            Map(m => m.conpoc_email).Name("vender email");
             
        }
        
    }
}
    