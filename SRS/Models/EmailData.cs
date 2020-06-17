using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRS.Models
{
    internal class EmailData
    {
        
        public long ExpiringContractorRecords { get; set; }
        public long ExpiredContractorRecords { get; set; }
 
        public long ExpiringContractorRecordsNotFound { get; set; }
        public long ExpiredContractorRecordsNotFound { get; set; }

        public string ExpiringContractorSuccessfulFileName { get; set; }
        public string ExpiredContractorSuccessfulFileName { get; set; }
        public string ExpiringContractorUnsuccessfulFileName { get; set; }
        public string ExpiredContractorUnsuccessfulFileName { get; set; }

        public string ExpiringContractorSummary { get; set; }
        public string ExpiredContractorSummary { get; set; }

        public string ExpiringContractorRecordsNotFoundFileName { get; set; }
        public string ExpiredContractorRecordsNotFoundFileName { get; set; }

        public bool ExpiringContractorHasErrors { get; set; }
        public bool ExpiredContractorHasErrors { get; set; }
        public DateTime  AccessingDate { get; set; }
        public DateTime TimeBegin { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime  TimeElapsed { get; set; }
       // public long NumberOfRecords { get; set; } 
    }
}