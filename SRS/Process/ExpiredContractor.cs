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
            retrieveData = new RetrieveData();
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
                    _log.Info("The expired Contractor email send " + contractor.RegionalEMails + "To" + contractor.RegionalEMails + "cc" + contractor.RegionalEMails);
                    accessEmail.SendExpiredContractorEmailTemplate(contractor);

                    _log.Info("The expired contractor email sent successfully " + contractor.RegionalEMails + "To" + contractor.RegionalEMails + "cc" + contractor.RegionalEMails);
                    summary.ExpiredSuccessfulProcessed.Add(new ExpiredContractorSummary
                    { 
                        LastName = contractor.Person.LastName,
                        Suffix = contractor.Person.Suffix,
                        FirstName = contractor.Person.FirstName,
                        MiddleName = contractor.Person.MiddleName,
                        DaysToExpiration = contractor.DaysToExpiration,
                        gpoc_emails = contractor.gpoc_emails,
                        vpoc_emails = contractor.vpoc_emails,
                        RegionalEMails = contractor.RegionalEMails,
                        MajorEMails = contractor.MajorEMails, 
                        pers_investigation_date = contractor.pers_investigation_date
                    });
                }
                summary.GenerateSummaryFiles(emailData);
                emailData.ExpiredContractorRecords = expiredContractor.Count;
  
            }
            catch (Exception ex)
            {
                _log.Error("Process contractor File Error:" + ex.Message + " " + ex.InnerException + " " + ex.StackTrace);
            }
        }
    }
}

