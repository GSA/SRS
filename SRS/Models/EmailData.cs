using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRS.Models
{
    internal class EmailData
    {
        
        public Int64 ContractExpiringRecords { get; set; }
        public Int64 ContractExpiredRecords { get; set; }
 
        public Int64 ContractExpiringRecordsNotFound { get; set; }
        public Int64 ContractExpiredRecordsNotFound { get; set; }

        public string ContractExpiringSuccessfulFilename { get; set; }
        public string ContractExpiredSuccessfulFilename { get; set; }
        public string ContractExpiringUnsuccessfulFilename { get; set; }
        public string ContractExpiredUnsuccessfulFilename { get; set; }

        public string ContractExpiringSummary { get; set; }
        public string ContractExpiredSummary { get; set; }

        public string ContractExpiringRecordsNotFoundFileName { get; set; }
        public string ContractExpiredRecordsNotFoundFileName { get; set; }

        public bool ContractExpiringHasErrors { get; set; }
        public bool ContractExpiredHasErrors { get; set; }
        public DateTime  AccessingDate { get; internal set; }
        public DateTime TimeBegin { get; internal set; }
        public DateTime EndTime { get; internal set; }
        public DateTime  AccessingTime { get; internal set; }
    }
}