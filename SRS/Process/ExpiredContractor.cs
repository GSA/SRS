using SRS.Data;
using SRS.Mapping;
using SRS.Models;
using SRS.Utilities;
using SRS.Validation;
using System;
using System.Collections.Generic;

namespace SRS.Process
{
    internal class ExpiredContractor
    {
        //Reference to logger
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly EmailData emailData;
        private RetrieveData retrieveData = new RetrieveData();
        private readonly AccessEmail accessEmail = new AccessEmail();
        private List<Contractor> expiredContractor = new List<Contractor>();

        public ExpiredContractor(ref EmailData emailData)
        {
           // retrieveData = new RetrieveData();
            this.emailData = emailData;
        }

        //public void getExpiredContractor()
        //{
        //    MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["hspd"].ToString());
        //    //try
        //    //{
        //        // MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["hspd"].ToString());

        //        MySqlCommand cmd = new MySqlCommand();
        //        List<Contractor> allExpiredContractorData = new List<Contractor>();

        //        using (conn)
        //        {
        //            if (conn.State == ConnectionState.Closed)
        //                conn.Open();

        //            using (cmd)
        //            {
        //                MySqlDataReader expiredContractorData;


        //                cmd.Connection = conn;
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.CommandText = "SRS_GetContractors";
        //                cmd.Parameters.Clear();

        //                //MySqlDbType todaysDate = default(MySqlDbType);
        //                //cmd.Parameters.Add("DateTime", todaysDate);
        //                cmd.Parameters.AddWithValue("inDate", "2020-06-06");// accessingDate); //"2020-06-06"
        //                _log.Info("Contractor data of expiration: " + DateTime.Now);
        //                expiredContractorData = cmd.ExecuteReader();
        //                _log.Info("Contractor Retrieved Data: " + DateTime.Now);
        //                _log.Info("Adding Contractor expiring data to object: " + DateTime.Now);
        //            }
        //        }
        //    }
        public void ProcessExpiredContractor()
        {
            _log.Info("Processing Expired Contractor File" + DateTime.Now);
            var summary = new ContractorSummary();

            try
            {
                expiredContractor = retrieveData.allExpiredContractorData(emailData.AccessingDate);
                _log.Info("Loading Expired Contractor File" + expiredContractor.Count + " expired contractor: " + DateTime.Now);

                foreach (Contractor person in expiredContractor)
                {
                    _log.Info("The expired Contractor email send " + person.Pers_id + "To" + person.vpoc_emails + "cc" + person.gpoc_emails);
                    accessEmail.SendExpiredContractorEmailTemplate(person);

                    _log.Info("The expired contractor email sent successfully " + person.Pers_id + "To" + person.vpoc_emails + "cc" + person.gpoc_emails);
                    summary.ExpiredSuccessfulProcessed.Add(new ExpiredContractorSummary
                    {
                        Pers_id = person.Pers_id,
                        LastName = person.Person.LastName,
                        FirstName = person.Person.FirstName,
                        MiddleName = person.Person.MiddleName,
                        Suffix = person.Person.Suffix,
                        pers_investigation_date = person.pers_investigation_date,
                        vpoc_emails = person.vpoc_emails,
                        gpoc_emails = person.gpoc_emails, 
                        DaysToExpiration = person.DaysToExpiration,
                        pers_status = person.pers_status
                    });
                }
                summary.GenerateSummaryFiles(emailData);
                emailData.ExpiredContractorRecords = expiredContractor.Count;

                // Contractor SRSRecord;
                var columnList = string.Empty;

                var fileReader = new FileReader();
                var validate = new ValidateContractor();
                var save = new SaveData();
                var em = new ExpiredContractorSummaryMapping();
                //List<string> badRecords;

                //var expiringProcess = fileReader.GetFileData<ContractorData, ExpiredContractorSummaryMapping>(ContractorFile, out badRecords, em);
                //Helpers.AddBadRecordsToSummary(badRecords, ref summary);

                _log.Info("Loading POCs Data");
                //var contractorData = RetrieveData.ContractorData();

            }
            catch (Exception ex)
            {
                _log.Error("Process contractor File Error:" + ex.Message + " " + ex.InnerException + " " + ex.StackTrace);
            }
        }
    }
}

