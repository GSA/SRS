using CsvHelper.Configuration;
using SRS.Models;

namespace SRS.Mapping
{
    internal sealed class ExpiringContractorSummaryMapping : ClassMap<ExpiringContractorSummary>
    {
       // private Lookup lookups;

        public ExpiringContractorSummaryMapping()
        {
          
            Map(m => m.Pers_id).Name("Person ID");
            Map(m => m.FirstName).Name("First Name");
            Map(m => m.MiddleName).Name("Middle Name");
            Map(m => m.LastName).Name("Last Name");
            Map(m => m.Suffix).Name("Suffix"); 
            Map(m => m.vpoc_emails).Name("conPOC Email");
            Map(m => m.gpoc_emails).Name("gpoc email"); 
            Map(m => m.pers_investigation_date).Name("SAC Expiration DATE"); 
            Map(m => m.DaysToExpiration).Name("DaysToExpiration");
           // Map(m => m.conpoc_email).Name("vender email");
            Map(m => m.pers_status).Name("Contractor Status");
        }

      }

    internal sealed class ExpiredContractorSummaryMapping : ClassMap<ExpiredContractorSummary>
    {
        public ExpiredContractorSummaryMapping()
        {
            Map(m => m.Pers_id).Name("Person ID"); 
            Map(m => m.FirstName).Name("First Name");
            Map(m => m.MiddleName).Name("Middle Name");
            Map(m => m.LastName).Name("Last Name");
            Map(m => m.Suffix).Name("Suffix");
            Map(m => m.vpoc_emails).Name("conPOC Email");
            Map(m => m.gpoc_emails).Name("gpoc email");
            Map(m => m.pers_investigation_date).Name("Contract date end"); 
            Map(m => m.DaysToExpiration).Name("DaysToExpiration");
           // Map(m => m.conpoc_email).Name("vender email");
            Map(m => m.pers_status).Name("Contractor Status");
        }
        
    }
}
    