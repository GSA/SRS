using SRS.Data;
using SRS.Models;
using SRS.Process;
using SRS.Utilities;
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
        private readonly RetrieveData retrieveData = new RetrieveData();
        private readonly AccessEmail accessEmail = new AccessEmail();
        private List<Contractor> expiringContractor = new List<Contractor>();

        public ExpiringContractor(ref EmailData emailData)
        {
            //InitializeComponent();
            retrieveData = new RetrieveData();
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
                        //pers_investigation_date = contractor.pers_investigation_date,
                        DaysToExpiration = contractor.DaysToExpiration,
                        pers_status = contractor.pers_status 
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

    }
}
