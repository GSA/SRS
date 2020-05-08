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
        private static log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public readonly SummaryFileGenerator SummaryFileGenerator;
       
        public List<ExpiringContractorSummary> SuccessfulProcessed { get; set; }
        public List<ExpiringContractorSummary> UnsuccessfulProcessed { get; set; }

        public ContractorSummary()
        {
            SummaryFileGenerator = new SummaryFileGenerator();
              
            SuccessfulProcessed = new List<ExpiringContractorSummary>();
             
            UnsuccessfulProcessed = new List<ExpiringContractorSummary>();
            
        }
        public void GenerateSummaryFiles(EmailData emailData)
        {
            if (SuccessfulProcessed.Count > 0)
            {
                SuccessfulProcessed = SuccessfulProcessed.OrderBy(o => o.PersID ).ThenBy(t => t.LastName ).ToList();

                emailData.ExpiringContractorSuccessfulFilename = SummaryFileGenerator.GenerateSummaryFile<ExpiringContractorSummary, ExpiringContractorSummaryMapping>(ConfigurationManager.AppSettings["SUCCESSFULSUMMARYFILENAME"].ToString(), SuccessfulProcessed);
                _log.Info(" Expiring Contractor Successfull File: " + emailData.ExpiringContractorSuccessfulFilename);
            }

            if (UnsuccessfulProcessed.Count > 0)
            {
                UnsuccessfulProcessed = UnsuccessfulProcessed.OrderBy(o => o.PersID ).ThenBy(t => t.LastName ).ToList();

                emailData.ExpiringContractorUnsuccessfulFilename = SummaryFileGenerator.GenerateSummaryFile<ExpiringContractorSummary, ExpiringContractorSummaryMapping>(ConfigurationManager.AppSettings["ERRORSUMMARYFILENAME"].ToString(), UnsuccessfulProcessed);
                _log.Info("Contractors Error File: " + emailData.ExpiringContractorUnsuccessfulFilename);
            }
 
        }
    }
}

