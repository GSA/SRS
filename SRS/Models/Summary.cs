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
       // public Int64 Contract_id { get; set; }
        public String PersID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public Int64 contract_vender_ID { get; set; }
        public object contract_POC_Email { get; set; }
        public object RegionalEmail { get; set; }
        //public Int64 contract_number { get; set; }
        public DateTime contract_date_end { get; internal set; }
        public Int64 DaysUntilExpired { get; set; }
        //public string contract_name { get; set; }
        
    }
    internal class ExpiredContractorSummary
    {
       // public Int64 Contract_id { get; set; }
        public String PersID { get; set; }
        public long FirstName { get; set; }
        public long MiddleName { get; set; }
        public long LastName { get; set; }
        public string Suffix { get; set; }
        public Int64 contract_vender_ID { get; set; }
        public object contract_POC_Email { get; set; }
        public object RegionalEmail { get; set; }
       // public Int64 contract_number { get; set; }
        public DateTime contract_date_end { get; internal set; }
        public Int64 DaysUntilExpired { get; set; }
      //  public string contract_name { get; set; }

    }
    
    
}

