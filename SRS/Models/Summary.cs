using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRS.Models
{
    internal class ProcessedSummary
    {
        //TODO: Add Summary items here
        //public int ItemsProcessed { get; set; }
        public int GCIMSID { get; set; }
        public Int64 PersID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string HomeEmail { get; set; }
        public string Status { get; set; }
        public string Action { get; set; }
        public Int64 contract_number { get; set; }
        public DateTime contract_date_end { get; set; }
        public string contract_name { get; set; }
    }
    internal class RecordNotFoundSummary
    {
        public Int64 PersID { get; set; }
        public long FirstName { get; set; }
        public int GCIMSID { get; set; }
        public long HomeEmail { get; set; }
        public long LastName { get; set; }
        public long MiddleName { get; set; }
        public object Suffix { get; internal set; }
        public Int64 contract_number { get; set; }
        public DateTime contract_date_end { get; set; }
        public string contract_name { get; set; }
    }
    internal class IdenticalRecordSummary
    {
        public Int64 PersID { get; set; }
        public Int64 GCIMSID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string Status { get; set; }
        public Int64 contract_number { get; set; }
        public DateTime contract_date_end { get; set; }
        public string contract_name { get; set; }
    }

    internal class SocialSecurityNumberChangeSummary
    {
        public Int64 GCIMSID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string Status { get; set; }
        public Int64 contract_number { get; set; }
        public DateTime contract_date_end { get; set; }
        public string contract_name { get; set; }
    }

    internal class InactiveSummary
    {
        public Int64 PersID { get; set; }
        public Int64 GCIMSID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string Status { get; set; }
        public Int64 contract_number { get; set; }
        public DateTime contract_date_end { get; set; }
        public string contract_name { get; set; }

    }
}

