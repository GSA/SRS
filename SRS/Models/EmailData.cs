using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRS.Models
{
    internal class EmailData
    {
        public string MonsterFilename { get; set; }

        public Int64 MonsterFailed { get; set; }
        public Int64 MonsterSocial { get; set; }
        public Int64 MonsterInactive { get; set; }
        public Int64 MonsterIdentical { get; set; }
        public Int64 MonsterAttempted { get; set; }
        public Int64 MonsterSucceeded { get; set; }
        public Int64 MonsterRecordsNotFound { get; set; }

        public string MonsterSuccessfulFilename { get; set; }
        public string MonsterUnsuccessfulFilename { get; set; }
        public string MonsterIdenticalFilename { get; set; }
        public string MonsterSocialSecurityNumberChangeFilename { get; set; }
        public string MonsterInactiveFilename { get; set; }
        public string MonsterRecordsNotFoundFileName { get; set; }

        public bool MonsterHasErrors { get; set; }

    }
}