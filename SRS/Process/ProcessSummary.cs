using System;
using SRS.Models;
using SRS.Mapping;
using SRS.Utilities; 
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                emailData.ContractExpiringSuccessfulFilename = SummaryFileGenerator.GenerateSummaryFile<ExpiringContractorSummary, ContractExpiringSummaryMapping>(ConfigurationManager.AppSettings["SUCCESSFULSUMMARYFILENAME"].ToString(), SuccessfulProcessed);
                _log.Info("Contract Expiring Successfull File: " + emailData.ContractExpiringSuccessfulFilename);
            }

            if (UnsuccessfulProcessed.Count > 0)
            {
                UnsuccessfulProcessed = UnsuccessfulProcessed.OrderBy(o => o.PersID ).ThenBy(t => t.LastName ).ToList();

                emailData.ContractExpiringUnsuccessfulFilename = SummaryFileGenerator.GenerateSummaryFile<ExpiringContractorSummary, ContractExpiringSummaryMapping>(ConfigurationManager.AppSettings["ERRORSUMMARYFILENAME"].ToString(), UnsuccessfulProcessed);
                _log.Info("Contractors Error File: " + emailData.ContractExpiringUnsuccessfulFilename);
            }
 
        }
    }
}

