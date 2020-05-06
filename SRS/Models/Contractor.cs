using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRS.Models
{
    public class ContractorData
    {
       //public Person Person { get; set; }

        public String PersID { get; set; }
        public String LastName { get; set; }
        public String FirstName { get; set; }
        public String MiddleName { get; set; }
        public String Suffix { get; set; } 
        public Int64 contract_vender_ID { get; set; }
        public String  contract_POC_Email { get; set; }
        public String  RegionalEmail { get; set; }
       // public Int64 contract_number { get; set; }
        public DateTime contract_date_end { get; internal set; }
        public Int64 DaysUntilExpired { get; set; }
       // public string contract_name { get; set; }
       
    }
}