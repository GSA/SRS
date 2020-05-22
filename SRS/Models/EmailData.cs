using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRS.Models
{
    internal class EmailData
    {
        
        public Int64 ExpiringContractorRecords { get; set; }
        public Int64 ExpiredContractorRecords { get; set; }
 
        public Int64 ExpiringContractorRecordsNotFound { get; set; }
        public Int64 ExpiredContractorRecordsNotFound { get; set; }

        public String ExpiringContractorSuccessfulFileName { get; set; }
        public String ExpiredContractorSuccessfulFileName { get; set; }
        public String ExpiringContractorUnsuccessfulFileName { get; set; }
        public String ExpiredContractorUnsuccessfulFileName { get; set; }

        public String ExpiringContractorSummary { get; set; }
        public String ExpiredContractorSummary { get; set; }

        public String ExpiringContractorRecordsNotFoundFileName { get; set; }
        public String ExpiredContractorRecordsNotFoundFileName { get; set; }

        public bool ExpiringContractorHasErrors { get; set; }
        public bool ExpiredContractorHasErrors { get; set; }
        public DateTime  AccessingDate { get; set; }
        public DateTime TimeBegin { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime  AccessingTime { get; set; }
        public object ContractorFileName { get; set; }
        public string FileName { get; }
    }
}