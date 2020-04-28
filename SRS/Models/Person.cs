using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRS.Models
{
    public class Person
    {
        public Int64 PersID { get; set; } //If Matched we set this       
        //public Int64 contract_unid { get; set; } 
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string SocialSecurityNumber { get; set; }
        public string Gender { get; set; }
        public string HomeEmail { get; set; }
        public string Status { get; set; } //Not sure if needed yet (should be applicant for new people
        public object ServiceComputationDateLeave { get; internal set; }
        public object Region { get; internal set; }
        public object JobTitle { get; internal set; }
        public Int64 contract_number { get; set; }
        public DateTime contract_date_end { get; set; }
        public string contract_name { get; set; }
    }
}
