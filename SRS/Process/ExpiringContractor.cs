using SRS.Data;
using SRS.Mapping;
using SRS.Models;
using SRS.Utilities;
using SRS.Validation;
using System;
using System.Collections.Generic;

namespace SRS.Process
{
    internal class ExpiringContractor
    {
        //Reference to logger
        private static log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly EmailData emailData;
        private RetrieveData retrieveData = new RetrieveData();
        private readonly AccessEmail accessEmail = new AccessEmail();
        private List<Contractor> expiringContractor = new List<Contractor>();

        //public void GetExpiringContractor()
        //{

        //    MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["hspd"].ToString());

        //    //try
        //    //{
        //    //    _log.Info("Getting Contractor Data");
        //    //    // MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["hspd"].ToString());

        //    MySqlCommand cmd = new MySqlCommand();

        //    List<Contractor> allExpiringContractorData = new List<Contractor>();

        //    using (conn)
        //    {
        //        if (conn.State == ConnectionState.Closed)
        //            conn.Open();

        //        using (cmd)
        //        {
        //            MySqlDataReader expiringContractorData;

        //            cmd.Connection = conn;
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.CommandText = "SRS_GetContractors";
        //            cmd.Parameters.Clear();

        //            //MySqlDbType todaysDate = default(MySqlDbType);
        //            //cmd.Parameters.Add("DateTime", todaysDate);
        //            cmd.Parameters.AddWithValue("inDate", "2020-06-06");// accessingDate); //"2020-06-06"
        //            _log.Info("Contractor data of expiration: " + DateTime.Now);
        //            expiringContractorData = cmd.ExecuteReader();
        //            _log.Info("Contractor Retrieved Data: " + DateTime.Now);
        //            _log.Info("Adding Contractor expiring data to object: " + DateTime.Now);
        //        }
        //    }
        //}

        public ExpiringContractor( ref EmailData emailData)
        {
            //InitializeComponent();
           // retrieveData = new RetrieveData();
            this.emailData = emailData;
        }
        public void ProcessExpiringContractor()
        {
            _log.Info("Processing Expiring Contractor File" + DateTime.Now);
            
            try
            {
                var summary = new ContractorSummary();

                expiringContractor = retrieveData.GetExpiringContractor(emailData.AccessingDate);
                _log.Info("Loading Expiring Contractor File" + expiringContractor.Count + " expiring contractor: " + DateTime.Now);

                foreach (Contractor contractor in expiringContractor)
                {
                    _log.Info("The expiring Contractor email send " + contractor .Pers_id + "To" + contractor.vpoc_emails + "cc" + contractor.gpoc_emails);
                    accessEmail.SendExpiringContractorEmailTemplate(contractor);

                    _log.Info("The expiring contractor email sent successfully " + contractor.Pers_id + "To" + contractor.vpoc_emails + "cc" + contractor.gpoc_emails);
                    summary.SuccessfulProcessed.Add(new ExpiringContractorSummary
                    {
                        Pers_id = contractor.Pers_id,
                        LastName = contractor.Person.LastName,
                        FirstName = contractor.Person.FirstName,
                        MiddleName = contractor.Person.MiddleName,
                        Suffix = contractor.Person.Suffix,
                        pers_investigation_date = contractor.pers_investigation_date,
                        vpoc_emails = contractor.vpoc_emails,
                        gpoc_emails = contractor.gpoc_emails,
                        DaysToExpiration = contractor.DaysToExpiration,
                        pers_status = contractor.pers_status
                    });

                }
                summary.GenerateSummaryFiles(emailData);
                emailData.ExpiringContractorRecords = expiringContractor.Count;

                // Contractor SRSRecord;
                var columnList = string.Empty;

                var fileReader = new FileReader();
                var validate = new ValidateContractor();
                var save = new SaveData();
                var em = new ExpiringContractorSummaryMapping();
                //List<string> badRecords;

              //  var expiringProcess = fileReader.GetFileData<Contractor, ExpiringContractorSummaryMapping>(ExpiringContractorData, out badRecords, em);
              // Helpers.AddBadRecordsToSummary(badRecords, ref summary);

                _log.Info("Loading POCs Data");
                //var contractor = RetrieveData.ExpiringContractorData();

            }
            catch (Exception ex)
            {
                _log.Error("Process contractor File Error:" + ex.Message + " " + ex.InnerException + " " + ex.StackTrace);
            }
        }

    }
}
