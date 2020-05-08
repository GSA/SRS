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

        public string ExpiringContractorSuccessfulFilename { get; set; }
        public string ExpiredContractorSuccessfulFilename { get; set; }
        public string ExpiringContractorUnsuccessfulFilename { get; set; }
        public string ExpiredContractorUnsuccessfulFilename { get; set; }

        public string ExpiringContractorSummary { get; set; }
        public string ExpiredContractorSummary { get; set; }

        public string ExpiringContractorRecordsNotFoundFileName { get; set; }
        public string ExpiredContractorRecordsNotFoundFileName { get; set; }

        public bool ExpiringContractorHasErrors { get; set; }
        public bool ExpiredContractorHasErrors { get; set; }
        public DateTime  AccessingDate { get; internal set; }
        public DateTime TimeBegin { get; internal set; }
        public DateTime EndTime { get; internal set; }
        public DateTime  AccessingTime { get; internal set; }
        public object ContractorFilename { get; internal set; }
    }
}