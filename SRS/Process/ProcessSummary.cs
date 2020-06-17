﻿using SRS.Models;
using SRS.Mapping;
using SRS.Utilities;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace SRS.Process
{
    internal class ContractorSummary
    {
        //Reference to logger
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public readonly SummaryFileGenerator SummaryFileGenerator;
       
        public List<ExpiringContractorSummary> ExpiringSuccessfulProcessed { get; set; }
        public List<ExpiringContractorSummary> ExpiringUnsuccessfulProcessed { get; set; }
        public List<ExpiredContractorSummary> ExpiredSuccessfulProcessed { get; set; }
        public List<ExpiredContractorSummary> ExpiredUnsuccessfulProcessed { get; set; }

        public ContractorSummary()
        {
            SummaryFileGenerator = new SummaryFileGenerator();
              
            ExpiringSuccessfulProcessed = new List<ExpiringContractorSummary>();
             
            ExpiringUnsuccessfulProcessed = new List<ExpiringContractorSummary>();

            ExpiredSuccessfulProcessed = new List<ExpiredContractorSummary>();

            ExpiredUnsuccessfulProcessed = new List<ExpiredContractorSummary>();

        }
        public void GenerateSummaryFiles(EmailData emailData)
        {
            if (ExpiringSuccessfulProcessed.Count > 0)
            {
                ExpiringSuccessfulProcessed = ExpiringSuccessfulProcessed.OrderBy(o => o.RegionalEMails).ThenBy(t => t.LastName ).ToList();

                emailData.ExpiringContractorSuccessfulFileName = SummaryFileGenerator.GenerateSummaryFile<ExpiringContractorSummary, ExpiringContractorSummaryMapping>(ConfigurationManager.AppSettings["ExpiringSUCCESSFULSUMMARYFILENAME"].ToString(), ExpiringSuccessfulProcessed);
                log.Info("Expiring Contractor Successfull File: " + emailData.ExpiringContractorSuccessfulFileName);
            }

            //if (ExpiringUnsuccessfulProcessed.Count > 0)
            //{
            //    ExpiringUnsuccessfulProcessed = ExpiringUnsuccessfulProcessed.OrderBy(o => o.RegionalEMails).ThenBy(t => t.LastName ).ToList();

            //    emailData.ExpiringContractorUnsuccessfulFileName = SummaryFileGenerator.GenerateSummaryFile<ExpiringContractorSummary, ExpiringContractorSummaryMapping>(ConfigurationManager.AppSettings["ExpiringERRORSUMMARYFILENAME"].ToString(), ExpiringUnsuccessfulProcessed);
            //    log.Info("Contractors Error File: " + emailData.ExpiringContractorUnsuccessfulFileName);
            //}

            if (ExpiredSuccessfulProcessed.Count > 0)
            {
                ExpiredSuccessfulProcessed = ExpiredSuccessfulProcessed.OrderBy(o => o.RegionalEMails).ThenBy(t => t.LastName).ToList();

                emailData.ExpiredContractorSuccessfulFileName = SummaryFileGenerator.GenerateSummaryFile<ExpiredContractorSummary, ExpiredContractorSummaryMapping>(ConfigurationManager.AppSettings["ExpiredSUCCESSFULSUMMARYFILENAME"].ToString(), ExpiredSuccessfulProcessed);
                log.Info(" Expired Contractor Successfull File: " + emailData.ExpiredContractorSuccessfulFileName);
            }

            //if (ExpiredUnsuccessfulProcessed.Count > 0)
            //{
            //    ExpiredUnsuccessfulProcessed = ExpiredUnsuccessfulProcessed.OrderBy(o => o.RegionalEMails).ThenBy(t => t.LastName).ToList();

            //    emailData.ExpiredContractorUnsuccessfulFileName = SummaryFileGenerator.GenerateSummaryFile<ExpiredContractorSummary, ExpiredContractorSummaryMapping>(ConfigurationManager.AppSettings["ExpiredERRORSUMMARYFILENAME"].ToString(), ExpiredUnsuccessfulProcessed);
            //    log.Info("Contractors Error File: " + emailData.ExpiredContractorUnsuccessfulFileName);
            //}

        }
    }
}

