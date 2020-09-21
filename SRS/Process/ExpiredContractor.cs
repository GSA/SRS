using SRS.Data;
using SRS.Models;
using SRS.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;

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
        private Suitability.SendNotification sendNotification;

        public ExpiredContractor(ref EmailData emailData)
        {
            retrieveData = new RetrieveData();
            this.emailData = emailData;
        } 
        public void ProcessExpiredContractor()
        {
            _log.Info("Processing Expired Contractor File" + " " + DateTime.Today);
            var summary = new ContractorSummary();

            try
            {
                expiredContractor = retrieveData.allExpiredContractorData(emailData.ACCESSINGDATE);
                _log.Info("Loading Expired Contractor File" + " " + expiredContractor.Count + " expired contractors: " + DateTime.Today);
                 
                foreach (Contractor contractor in expiredContractor)
                {
                    _log.Info("The expired Contractor email send " + contractor.LastName + contractor.FirstName + "To" + contractor.gpoc_emails + "cc" + contractor.vpoc_emails);
                    //accessEmail.SendExpiredContractorEmailTemplate(contractor);

                    sendNotification = new Suitability.SendNotification(
                        ConfigurationManager.AppSettings["DEFAULTEMAIL"],
                        contractor.PersonID,
                        ConfigurationManager.ConnectionStrings["hspd"].ToString(),
                        ConfigurationManager.AppSettings["SMTPSERVER"],
                        "");
                    sendNotification.SendSRSNotification();

                    _log.Info("The expired contractor email sent successfully " + contractor.LastName + contractor.FirstName + "To" + contractor.gpoc_emails + "cc" + contractor.vpoc_emails);

                    if (expiredContractor.Count > 0)
                    {
                        summary.ExpiredSuccessfulProcessed.Add(new ExpiredContractorSummary
                        {
                            PersonID = contractor.PersonID,
                            LastName = contractor.LastName,
                            Suffix = contractor.Suffix,
                            FirstName = contractor.FirstName,
                            MiddleName = contractor.MiddleName,
                            InvestigationDate = contractor.InvestigationDate.ToString(),
                            DaysToExpiration = contractor.DaysToExpiration,
                            gpoc_emails = contractor.gpoc_emails,
                            vpoc_emails = contractor.vpoc_emails,
                            //RegionalEMails = contractor.RegionalEMails,
                            HSSOEMails = contractor.HSSOEMails
                        });

                    }
                    else
                    {
                        summary.ExpiredUnsuccessfulProcessed.Add(new ExpiredContractorSummary
                        {
                            PersonID = contractor.PersonID,
                            LastName = contractor.LastName,
                            Suffix = contractor.Suffix,
                            FirstName = contractor.FirstName,
                            MiddleName = contractor.MiddleName,
                            InvestigationDate = contractor.InvestigationDate.ToString(),
                            DaysToExpiration = contractor.DaysToExpiration,
                            gpoc_emails = contractor.gpoc_emails,
                            vpoc_emails = contractor.vpoc_emails,
                            //RegionalEMails = contractor.RegionalEMails,
                            HSSOEMails = contractor.HSSOEMails
                        });
                      
                    }

                }

                emailData.ExpiredContractorRecords = expiredContractor.Count;
                summary.GenerateSummaryFiles(emailData);

                _log.Info("Successfull processed Expired contractors: " + $"{summary.ExpiredSuccessfulProcessed.Count}");
                _log.Info("Unsuccessfull processed Expired contractors: " + $"{summary.ExpiredUnsuccessfulProcessed.Count}");

            }
            catch (Exception ex)
            {
                _log.Error("Process contractor File Error:" + ex.Message + " " + ex.InnerException + " " + ex.StackTrace);
            }
        }
    }
}

