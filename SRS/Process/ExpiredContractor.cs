using AutoMapper;
using SRS.Data;
using SRS.Models;
using SRS.Process;
using SRS.Utilities;
using SRS.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRS.Process
{
    internal class ExpiredContractor
    {
        //Reference to logger
        private static log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly EmailData emailData;
        private RetrieveData retrieveData;// = new RetrieveData();
        private readonly AccessEmail accessEmail = new AccessEmail();
        private List<Contractor> expiredContractor = new List<Contractor>();

        public ExpiredContractor(ref EmailData emailData)
        {
           // retrieveData = new RetrieveData();
            this.emailData = emailData;
        }
        public void ProcessExpiredContractor()
        {
            _log.Info("Processing Expired Contractor File" + DateTime.Now);
            var summary = new ContractorSummary();

            try
            {
                expiredContractor = retrieveData.allExpiredContractorData(emailData.AccessingDate);
                _log.Info("Loading Expired Contractor File" + expiredContractor.Count + " expired contractor: " + DateTime.Now);

                foreach (Contractor contractor in expiredContractor)
                {
                    _log.Info("The expired Contractor email send " + contractor.Pers_id + "To" + contractor.vpoc_emails + "cc" + contractor.gpoc_emails);
                    accessEmail.SendExpiredContractorEmailTemplate(contractor);

                    _log.Info("The expired contractor email sent successfully " + contractor.Pers_id + "To" + contractor.vpoc_emails + "cc" + contractor.gpoc_emails);
                    summary.ExpiredSuccessfulProcessed.Add(new ExpiredContractorSummary
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
                emailData.ExpiredContractorRecords = expiredContractor.Count;

                // Contractor SRSRecord;
                var columnList = string.Empty;

                var fileReader = new FileReader();
                var validate = new ValidateContractor();
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
    }
}

