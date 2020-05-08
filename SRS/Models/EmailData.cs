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

        public String ExpiringContractorSuccessfulFilename { get; set; }
        public String ExpiredContractorSuccessfulFilename { get; set; }
        public String ExpiringContractorUnsuccessfulFilename { get; set; }
        public String ExpiredContractorUnsuccessfulFilename { get; set; }

        public String ExpiringContractorSummary { get; set; }
        public String ExpiredContractorSummary { get; set; }

        public String ExpiringContractorRecordsNotFoundFileName { get; set; }
        public String ExpiredContractorRecordsNotFoundFileName { get; set; }

        public bool ExpiringContractorHasErrors { get; set; }
        public bool ExpiredContractorHasErrors { get; set; }
        public DateTime  AccessingDate { get; internal set; }
        public DateTime TimeBegin { get; internal set; }
        public DateTime EndTime { get; internal set; }
        public DateTime  AccessingTime { get; internal set; }
        public object ContractorFilename { get; internal set; }
    }
}