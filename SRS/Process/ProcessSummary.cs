using SRS.Models;
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
       
        public List<ExpiringContractorSummary> SuccessfulProcessed { get; set; }
        public List<ExpiringContractorSummary> UnsuccessfulProcessed { get; set; }
        public List<ExpiredContractorSummary> ExpiredSuccessfulProcessed { get; set; }
        public List<ExpiredContractorSummary> ExpiredUnsuccessfulProcessed { get; set; }

        public ContractorSummary()
        {
            SummaryFileGenerator = new SummaryFileGenerator();
              
            SuccessfulProcessed = new List<ExpiringContractorSummary>();
             
            UnsuccessfulProcessed = new List<ExpiringContractorSummary>();

            ExpiredSuccessfulProcessed = new List<ExpiredContractorSummary>();

            ExpiredUnsuccessfulProcessed = new List<ExpiredContractorSummary>();

        }
        public void GenerateSummaryFiles(EmailData emailData)
        {
            if (SuccessfulProcessed.Count > 0)
            {
                SuccessfulProcessed = SuccessfulProcessed.OrderBy(o => o.Pers_id ).ThenBy(t => t.LastName ).ToList();

                emailData.ExpiringContractorSuccessfulFileName = SummaryFileGenerator.GenerateSummaryFile<ExpiringContractorSummary, ExpiringContractorSummaryMapping>(ConfigurationManager.AppSettings["ExpiringSUCCESSFULSUMMARYFILENAME"].ToString(), SuccessfulProcessed);
                log.Info("Expiring Contractor Successfull File: " + emailData.ExpiringContractorSuccessfulFileName);
            }

            if (UnsuccessfulProcessed.Count > 0)
            {
                UnsuccessfulProcessed = UnsuccessfulProcessed.OrderBy(o => o.Pers_id ).ThenBy(t => t.LastName ).ToList();

                emailData.ExpiringContractorUnsuccessfulFileName = SummaryFileGenerator.GenerateSummaryFile<ExpiringContractorSummary, ExpiringContractorSummaryMapping>(ConfigurationManager.AppSettings["ExpiringERRORSUMMARYFILENAME"].ToString(), UnsuccessfulProcessed);
                log.Info("Contractors Error File: " + emailData.ExpiringContractorUnsuccessfulFileName);
            }

            if (SuccessfulProcessed.Count > 0)
            {
                SuccessfulProcessed = SuccessfulProcessed.OrderBy(o => o.Pers_id).ThenBy(t => t.LastName).ToList();

                emailData.ExpiredContractorSuccessfulFileName = SummaryFileGenerator.GenerateSummaryFile<ExpiredContractorSummary, ExpiredContractorSummaryMapping>(ConfigurationManager.AppSettings["ExpiredSUCCESSFULSUMMARYFILENAME"].ToString(), ExpiredSuccessfulProcessed);
                log.Info(" Expired Contractor Successfull File: " + emailData.ExpiredContractorSuccessfulFileName);
            }

            if (UnsuccessfulProcessed.Count > 0)
            {
                UnsuccessfulProcessed = UnsuccessfulProcessed.OrderBy(o => o.Pers_id).ThenBy(t => t.LastName).ToList();

                emailData.ExpiredContractorUnsuccessfulFileName = SummaryFileGenerator.GenerateSummaryFile<ExpiredContractorSummary, ExpiredContractorSummaryMapping>(ConfigurationManager.AppSettings["ExpiredERRORSUMMARYFILENAME"].ToString(), ExpiredUnsuccessfulProcessed);
                log.Info("Contractors Error File: " + emailData.ExpiredContractorUnsuccessfulFileName);
            }

        }
    }
}

