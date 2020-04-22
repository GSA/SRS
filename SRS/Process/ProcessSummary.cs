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
    internal class MonsterSummary
    {
        //Reference to logger
        private static log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public readonly SummaryFileGenerator SummaryFileGenerator;
        public List<InactiveSummary> InactiveRecords { get; set; }
        public List<RecordNotFoundSummary> RecordsNotFound { get; set; }
        public List<IdenticalRecordSummary> IdenticalRecords { get; set; }
        public List<ProcessedSummary> SuccessfulUsersProcessed { get; set; }
        public List<ProcessedSummary> UnsuccessfulUsersProcessed { get; set; }
        public List<SocialSecurityNumberChangeSummary> SocialSecurityNumberChanges { get; set; }

        public MonsterSummary()
        {
            SummaryFileGenerator = new SummaryFileGenerator();

            InactiveRecords = new List<InactiveSummary>();
            SuccessfulUsersProcessed = new List<ProcessedSummary>();
            IdenticalRecords = new List<IdenticalRecordSummary>();
            UnsuccessfulUsersProcessed = new List<ProcessedSummary>();
            RecordsNotFound = new List<RecordNotFoundSummary>();
            SocialSecurityNumberChanges = new List<SocialSecurityNumberChangeSummary>();
        }
        public void GenerateSummaryFiles(EmailData emailData)
        {
            if (SuccessfulUsersProcessed.Count > 0)
            {
                SuccessfulUsersProcessed = SuccessfulUsersProcessed.OrderBy(o => o.LastName).ThenBy(t => t.FirstName).ToList();

                emailData.MonsterSuccessfulFilename = SummaryFileGenerator.GenerateSummaryFile<ProcessedSummary, ProcessedSummaryMapping>(ConfigurationManager.AppSettings["SUCCESSSUMMARYFILENAME"].ToString(), SuccessfulUsersProcessed);
                _log.Info("Monster Success File: " + emailData.MonsterSuccessfulFilename);
            }

            if (UnsuccessfulUsersProcessed.Count > 0)
            {
                UnsuccessfulUsersProcessed = UnsuccessfulUsersProcessed.OrderBy(o => o.LastName).ThenBy(t => t.FirstName).ToList();

                emailData.MonsterUnsuccessfulFilename = SummaryFileGenerator.GenerateSummaryFile<ProcessedSummary, ProcessedSummaryMapping>(ConfigurationManager.AppSettings["ERRORSUMMARYFILENAME"].ToString(), UnsuccessfulUsersProcessed);
                _log.Info("Monster Error File: " + emailData.MonsterUnsuccessfulFilename);
            }

            if (IdenticalRecords.Count > 0)
            {
                IdenticalRecords = IdenticalRecords.OrderBy(o => o.LastName).ThenBy(t => t.FirstName).ToList();

                emailData.MonsterIdenticalFilename = SummaryFileGenerator.GenerateSummaryFile<IdenticalRecordSummary, IdenticalRecordSummaryMapping>(ConfigurationManager.AppSettings["IDENTICALSUMMARYFILENAME"].ToString(), IdenticalRecords);
                _log.Info("HR Identical File:" + emailData.MonsterIdenticalFilename);
            }

            if (SocialSecurityNumberChanges.Count > 0)
            {
                SocialSecurityNumberChanges = SocialSecurityNumberChanges.OrderBy(o => o.LastName).ThenBy(t => t.FirstName).ToList();

                emailData.MonsterSocialSecurityNumberChangeFilename = SummaryFileGenerator.GenerateSummaryFile<SocialSecurityNumberChangeSummary, SocialSecurityNumberChangeSummaryMapping>(ConfigurationManager.AppSettings["SOCIALSECURITYNUMBERCHANGESUMMARYFILENAME"].ToString(), SocialSecurityNumberChanges);
                _log.Info("HR Social Security Number Change File: " + emailData.MonsterSocialSecurityNumberChangeFilename);
            }

            if (InactiveRecords.Count > 0)
            {
                InactiveRecords = InactiveRecords.OrderBy(o => o.LastName).ThenBy(t => t.FirstName).ToList();

                emailData.MonsterInactiveFilename = SummaryFileGenerator.GenerateSummaryFile<InactiveSummary, InactiveSummaryMapping>(ConfigurationManager.AppSettings["INACTIVESUMMARYFILENAME"].ToString(), InactiveRecords);
                _log.Info("Monster Inactive File: " + emailData.MonsterInactiveFilename);
            }

            if (RecordsNotFound.Count > 0)
            {
                RecordsNotFound = RecordsNotFound.OrderBy(o => o.LastName).ThenBy(t => t.FirstName).ToList();

                emailData.MonsterRecordsNotFoundFileName = SummaryFileGenerator.GenerateSummaryFile<RecordNotFoundSummary, RecordNotFoundSummaryMapping>(ConfigurationManager.AppSettings["RECORDNOTFOUNDSUMMARYFILENAME"].ToString(), RecordsNotFound);
                _log.Info("Monster Name Not Found File: " + emailData.MonsterRecordsNotFoundFileName);
            }
        }
    }
}

