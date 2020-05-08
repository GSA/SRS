using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRS.Models
{
    internal class ExpiringContractorSummary
    {
        //TODO: Add Summary items here  
        public String PersID { get; set; }
        public String FirstName { get; set; }
        public String MiddleName { get; set; }
        public String LastName { get; set; }
        public String Suffix { get; set; } 
        public object pers_work_email { get; set; }
        public object RegionalEmail { get; set; } 
        public DateTime pers_investigation_date { get; internal set; }
        public Int64 DaysToExpiration { get; set; }
        public String conpoc_email { get; set; }

    }
    internal class ExpiredContractorSummary
    { 
        public String PersID { get; set; }
        public String  FirstName { get; set; }
        public String MiddleName { get; set; }
        public String LastName { get; set; }
        public String Suffix { get; set; }
        public String conpoc_email { get; set; }
        public object pers_work_email { get; set; }
        public object RegionalEmail { get; set; } 
        public DateTime pers_investigation_date { get; internal set; }
        public Int64 DaysToExpiration { get; set; } 

    }
    
    
}

