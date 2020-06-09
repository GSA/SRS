using AutoMapper;
using SRS.Data;
using SRS.Mapping;
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
    class ExpiringContractor
    {
        //Reference to logger
        private static log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly EmailData emailData;
        private RetrieveData retrieveData; // = new RetrieveData();
        private readonly AccessEmail accessEmail = new AccessEmail();
        private List<Contractor> expiringContractor = new List<Contractor>();

        public ExpiringContractor( ref EmailData emailData)
        {
            //InitializeComponent();
          //  retrieveData = new RetrieveData();
            this.emailData = emailData;
        }
        public void ProcessExpiringContractor()
        {
            _log.Info("Processing Expiring Contractor File" + DateTime.Now);
            
            try
            {
                var summary = new ContractorSummary();

                expiringContractor = retrieveData.AllExpiringContractor(emailData.AccessingDate);
                _log.Info("Loading Expiring Contractor File" + expiringContractor.Count + " expiring contractor: " + DateTime.Now);

                foreach (var contractor in expiringContractor)
                {
                    _log.Info("The expiring Contractor email send " + contractor.Pers_id + "To" + contractor.vpoc_emails + "cc" + contractor.gpoc_emails);
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
                //var save = new SaveData();
                //var em = new ExpiringContractorSummaryMapping();
                //List<string> badRecords;

                //var expiringProcess = fileReader.GetFileData<Contractor, ExpiringContractorSummaryMapping>(ExpiringContractorData, out badRecords, em);
                //Helpers.AddBadRecordsToSummary(badRecords, ref summary);

                //_log.Info("Loading POCs Data");
                //var contractor = RetrieveData.ExpiringContractorData();

            }
            catch (Exception ex)
            {
                _log.Error("Process contractor File Error:" + ex.Message + " " + ex.InnerException + " " + ex.StackTrace);
            }
        }

    }
}
