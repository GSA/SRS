using AutoMapper;
using SRS.Data;
using SRS.Utilities;
using SRS.Mapping;
using SRS.Models;
using System;
using System.Collections.Generic;

namespace SRS.Process
{
    internal class ProcessContractor
    {
        //Reference to logger
        private static log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly EmailData emailData;
        private readonly RetrieveData retrieve;
        private readonly AccessEmail accessEmail = new AccessEmail();
        private List<ContractorData> expiringContractor = new List<ContractorData>();
      
        public ProcessContractor(IMapper dataMapper, ref EmailData emailData)
        {
            //InitializeComponent();
            retrieve = new RetrieveData(dataMapper);
            this.emailData = emailData;
        }

        public void ExpiringContractor(ref EmailData emailData)
        {
           // ExpiringContractor expiringContractor = new ExpiringContractor(ref emailData);

            try
            {
                //expiringContractor.ProcessExpiringContractor();
            }
            catch(Exception ex)
            {
                _log.Error("Getting Contracts:" + "-" + ex.Message + "-" + ex.InnerException);
            }
        }
        public void ProcessExpiringContractor()
        {  
            _log.Info("Processing Expiring Contractor File" + DateTime.Now);
            var summary = new ContractorSummary();

            try
            {
                expiringContractor = retrieve.ExpiringContractor(emailData.AccessingDate);
                _log.Info("Loading Expiring Contractor File" + expiringContractor.Count + " expiring contractor: " + DateTime.Now);

                foreach (ContractorData contractor in expiringContractor)
                {
                    _log.Info("The expiring Contractor email send " + contractor.PersID + "To" + contractor.pers_work_email + "cc" + contractor.RegionalEmail);
                    accessEmail.SendExpiringContractorEmailTemplate(contractor);

                    _log.Info("The expiring contractor email sent successfully " + contractor.PersID + "To" + contractor.pers_work_email + "cc" + contractor.RegionalEmail);
                    summary.SuccessfulProcessed.Add(new ExpiringContractorSummary
                    {
                        PersID = contractor.PersID, 
                        LastName = contractor.LastName,
                        FirstName = contractor.FirstName,
                        Suffix = contractor.Suffix,
                        pers_work_email = contractor.pers_work_email,
                        RegionalEmail = contractor.RegionalEmail,
                        pers_investigation_date = contractor.pers_investigation_date,
                        DaysToExpiration = contractor.DaysToExpiration, 
                    });

                }
                summary.GenerateSummaryFiles(emailData);
                emailData.ExpiringContractorRecords = expiringContractor.Count;
            
                // Contractor SRSRecord;
                var columnList = string.Empty;
               
                var fileReader = new FileReader();
                //var validate = new ValidateContractor();
                //var save = new SaveData();
                //var em = new ExpiringContractorSummaryMapping();
                //List<string> badRecords;

                //var expiringProcess = fileReader.GetFileData<ContractorData, ExpiringContractorSummaryMapping> (ContractorFile, out badRecords, em);
                //Helpers.AddBadRecordsToSummary(badRecords, ref summary);

                //_log.Info("Loading POCs Data");
                //var contractorData = RetrieveData.ContractorData();

            }
            catch (Exception ex)
            {
                _log.Error("Process contractor File Error:" + ex.Message + " " + ex.InnerException + " " + ex.StackTrace);
            }
        }
        
            public void ProcessExpiredContractor()
        {
            ExpiredContractorSummary expiredContractor = new ExpiredContractorSummary();
        }
    }
}
