using System;

namespace SRS.Models
{
    internal class ExpiringContractorSummary
    {
        public Person Person { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string InvestigationDate { get; set; }
        public int DaysToExpiration { get; set; }
        public string gpoc_emails { get; set; }
        public string vpoc_emails { get; set; }
        public string RegionalEMails { get; set; }
        public string MajorEMails { get; set; } 
    }
    internal class ExpiredContractorSummary
    {
        public Person Person { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string InvestigationDate { get; set; }
        public int DaysToExpiration { get; set; }
        public string gpoc_emails { get; set; }
        public string vpoc_emails { get; set; }
        public string RegionalEMails { get; set; }
        public string MajorEMails { get; set; } 
    }


}

