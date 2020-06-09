﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRS.Models
{
    public class Person
    {
        
        public string Pers_id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Suffix { get; set; }
        public string vpoc_emails { get; set; }
        public string gpoc_emails { get; set; }
        //public string RegionalEmail { get; set; }
        public DateTime pers_investigation_date { get; set; }
        public int DaysToExpiration { get; set; }
        public string pers_status { get; set; }
 
    }
}
