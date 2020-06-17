using CsvHelper.Configuration;
using SRS.Models;

namespace SRS.Mapping
{
    internal sealed class ExpiringContractorSummaryMapping : ClassMap<ExpiringContractorSummary>
    {
       // private Lookup lookups;

        public ExpiringContractorSummaryMapping()
        { 
            Map(m => m.LastName).Name("Last Name");
            Map(m => m.Suffix).Name("Suffix");
            Map(m => m.FirstName).Name("First Name");
            Map(m => m.MiddleName).Name("Middle Name");
            Map(m => m.DaysToExpiration).Name("DaysToExpiration");
            Map(m => m.gpoc_emails).Name("gpoc email");
            Map(m => m.vpoc_emails).Name("conPOC Email");
            Map(m => m.RegionalEMails).Name("RegionalEMails");
            Map(m => m.MajorEMails).Name("MajorEMails");
            
        }

      }

    internal sealed class ExpiredContractorSummaryMapping : ClassMap<ExpiredContractorSummary>
    {
        public ExpiredContractorSummaryMapping()
        {
            Map(m => m.LastName).Name("Last Name");
            Map(m => m.Suffix).Name("Suffix");
            Map(m => m.FirstName).Name("First Name");
            Map(m => m.MiddleName).Name("Middle Name");
            Map(m => m.DaysToExpiration).Name("DaysToExpiration");
            Map(m => m.gpoc_emails).Name("gpoc email");
            Map(m => m.vpoc_emails).Name("conPOC Email");
            Map(m => m.RegionalEMails).Name("RegionalEMails");
            Map(m => m.MajorEMails).Name("MajorEMails");
        }
        
    }
}
    