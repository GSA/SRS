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
 
        public ExpiringContractor( ref EmailData emailData)
        {
            //InitializeComponent();
            retrieveData = new RetrieveData();
            this.emailData = emailData;
        }
        public void ProcessExpiringContractor()
        {
            _log.Info("Processing Expiring Contractor File" + DateTime.Now);
            var summary = new ContractorSummary();
            try
            {   
                expiringContractor = retrieveData.GetExpiringContractor(emailData.AccessingDate);
                _log.Info("Loading Expiring Contractor File" + expiringContractor.Count + " expiring contractor: " + DateTime.Now);

                foreach (Contractor contractor in expiringContractor)
                {
                    _log.Info("The expiring Contractor email send " + contractor.LastName + "To" + contractor.RegionalEMails + "cc" + contractor.RegionalEMails);
                    accessEmail.SendExpiringContractorEmailTemplate(contractor);

                    _log.Info("The expiring contractor email sent successfully " + contractor.LastName + "To" + contractor.RegionalEMails + "cc" + contractor.RegionalEMails);
                    summary.ExpiringSuccessfulProcessed.Add(new ExpiringContractorSummary
                    {
                        LastName = contractor.LastName,
                        Suffix = contractor.Suffix,
                        FirstName = contractor.FirstName,
                        MiddleName = contractor.MiddleName,
                        DaysToExpiration = contractor.DaysToExpiration,
                        gpoc_emails = contractor.gpoc_emails,
                        vpoc_emails = contractor.vpoc_emails,
                        RegionalEMails = contractor.RegionalEMails,
                        MajorEMails = contractor.MajorEMails,
                       pers_investigation_date = contractor.pers_investigation_date
                    });

                }
                summary.GenerateSummaryFiles(emailData);
                emailData.ExpiringContractorRecords = expiringContractor.Count;
 
            }
            catch (Exception ex)
            {
                _log.Error("Process contractor File Error:" + ex.Message + " " + ex.InnerException + " " + ex.StackTrace);
            }
        }

    }
}
