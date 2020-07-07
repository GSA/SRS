using CsvHelper.Configuration;
using SRS.Models;

namespace SRS.Mapping
{
    internal sealed class ExpiringContractorSummaryMapping : ClassMap<ExpiringContractorSummary>
    { 
        public ExpiringContractorSummaryMapping()
        { 
            Map(m => m.LastName).Name("Last Name");
            Map(m => m.Suffix).Name("Suffix");
            Map(m => m.FirstName).Name("First Name");
            Map(m => m.MiddleName).Name("Middle Name");
            Map(m => m.DaysToExpiration).Name("DaysToExpiration");
            Map(m => m.gpoc_emails).Name("gpoc email");
            Map(m => m.vpoc_emails).Name("vpoc Email");
            Map(m => m.RegionalEMails).Name("Regional EMails");
            Map(m => m.MajorEMails).Name("Major EMails");
            Map(m => m.pers_investigation_date).Name("Investigation Date");
        }
    }

    internal sealed class ExpiredContractorSummaryMapping : ClassMap<ExpiredContractorSummary>
    {
        public ExpiredContractorSummaryMapping()
        {
            Map(m => m.LastName).Name("LastName");
            Map(m => m.Suffix).Name("Suffix");
            Map(m => m.FirstName).Name("FirstName");
            Map(m => m.MiddleName).Name("MiddleName");
            Map(m => m.DaysToExpiration).Name("DaysToExpiration");
            Map(m => m.gpoc_emails).Name("gpoc_email");
            Map(m => m.vpoc_emails).Name("vpoc_email");
            Map(m => m.RegionalEMails).Name("RegionalEMails");
            Map(m => m.MajorEMails).Name("MajorEMails");
            Map(m => m.pers_investigation_date).Name("Investigation Date");
        } 
    }
}
    