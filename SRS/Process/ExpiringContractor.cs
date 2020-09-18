using SRS.Data;
using SRS.Models;
using SRS.Utilities; 
using System;
using System.Collections.Generic;
using System.Configuration; 

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
        private static Suitability.SendNotification sendNotification;
        
        public ExpiringContractor( ref EmailData emailData)
        {
            //InitializeComponent();
            retrieveData = new RetrieveData();
            this.emailData = emailData;
        }
        public void ProcessExpiringContractor()
        {
            _log.Info("Processing Expiring Contractor File" + " " +  DateTime.Today);
            var summary = new ContractorSummary();
            try
            {    
                expiringContractor = retrieveData.allExpiringContractor(emailData.ACCESSINGDATE);
                _log.Info("Loading Expiring Contractor File" + " " + expiringContractor.Count + " expiring contractors: " + DateTime.Today);
                
                foreach (Contractor contractor in expiringContractor)
                {
                    _log.Info("The expiring Contractor email send " + contractor.LastName + contractor.FirstName + "To" + contractor.gpoc_emails + "cc" + contractor.vpoc_emails);
                    //accessEmail.SendExpiringContractorEmailTemplate(contractor); 
                    sendNotification = new Suitability.SendNotification(
                        ConfigurationManager.AppSettings["DEFAULTEMAIL"],
                        contractor.PersonID,
                        ConfigurationManager.ConnectionStrings["hspd"].ToString(),
                        ConfigurationManager.AppSettings["SMTPSERVER"],
                        "");
                    sendNotification.SendSRSNotification();
                    //sendNotification.SendExpiringContractorEmailTemplate();

                    _log.Info("The expiring contractor email sent successfully " + contractor.LastName + contractor.FirstName + "To" + contractor.gpoc_emails + "cc" + contractor.vpoc_emails);

                    if (expiringContractor.Count > 0)
                    {
 
                        summary.ExpiringSuccessfulProcessed.Add(new ExpiringContractorSummary
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
                            MajorEMails = contractor.MajorEMails 
                        });
                      
                    }
                    else
                    {
                        summary.ExpiringUnsuccessfulProcessed.Add(new ExpiringContractorSummary
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
                           MajorEMails = contractor.MajorEMails
                        }); 
                    }                  
                }

                emailData.ExpiringContractorRecords = expiringContractor.Count;
                summary.GenerateSummaryFiles(emailData);

                _log.Info("Successfull processed Expiring contractors: " + $"{summary.ExpiringSuccessfulProcessed.Count}");
                _log.Info("Unsuccessfull processed Expiring contractors: " + $"{summary.ExpiringUnsuccessfulProcessed.Count}");
                 
            }
            catch (Exception ex)
            {
                _log.Error("Process contractor File Error:" + ex.Message + " " + ex.InnerException + " " + ex.StackTrace);
            }
        }

    }
}
