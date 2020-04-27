using SRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using SRS.Validation;
using SRS.Process;
using log4net;

namespace SRS.Utilities
{
    internal static class Helpers
    {
        /// <summary>
        /// Hashes SSN
        /// </summary>
        /// <param name="ssn"></param>
        /// <returns></returns>
        public static byte[] HashSsn(string ssn)
        {
            byte[] hashedFullSsn = null;

            SHA256 shaM = new SHA256Managed();

            ssn = ssn.Replace("-", string.Empty);

            using (shaM)
            {
                hashedFullSsn = shaM.ComputeHash(Encoding.UTF8.GetBytes(ssn));
            }

            return hashedFullSsn;
        }
        public static bool CheckErrors(ValidateMonster validate, Contractor contractorData, List<ProcessedSummary> unsuccessfullMonesterUsersProcessed, ref ILog log)
        {
            var validationHelper = new ValidationHelper();
            var criticalErrors = validate.ValidateEmployeeCriticalInfo(contractorData);

            if (criticalErrors.IsValid) return false;
            log.Warn("Errors found for user: " + contractorData.Person.PersID + "(" + criticalErrors.Errors + ")");

            unsuccessfullMonesterUsersProcessed.Add(new ProcessedSummary
            {
                GCIMSID = -1,
                FirstName = contractorData.Person.FirstName,
                MiddleName = contractorData.Person.MiddleName,
                LastName = contractorData.Person.LastName,
                Suffix = contractorData.Person.Suffix,
                Action = validationHelper.GetErrors(criticalErrors.Errors, ValidationHelper.Monster.MonsterFile).TrimEnd(',')
            });
            return true;

        }
        /// <summary>
        /// Adds a bad record to the summary
        /// </summary>
        /// <param name="badRecords"></param>
        /// <param name="summary"></param>
        public static void AddBadRecordsToSummary(IEnumerable<string> badRecords, ref MonsterSummary summary)
        {
            foreach (var item in badRecords)
            {
                var parts = new List<string>();
                // var s = item.removeItems(new[] { "\"" });
                // parts.AddRange(s.Split('~'));
                var obj = new ProcessedSummary
                {
                    GCIMSID = -1,
                    Action = "Invalid Record From CSV File",
                    LastName = parts.Count > 1 ? parts[1] : "Unknown Last Name",
                    Suffix = parts.Count > 2 ? parts[2] : "Unknown Suffix",
                    FirstName = parts.Count > 3 ? parts[3] : "Unknown First Name",
                    MiddleName = parts.Count > 4 ? parts[4] : "Unknown Middle Name"
                };
                summary.UnsuccessfulUsersProcessed.Add(obj);
            }
        }
        /// <summary>
        /// Returns an Employee object if match found in db
        /// </summary>
        /// <param name="contractorData"></param>
        /// <param name="allGcimsData"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static Contractor RecordFound(Contractor contractorData, List<Contractor> allGcimsData, ref ILog log)
        {
            var monsterMatch = allGcimsData.Where(w => contractorData.Person.PersID == w.Person.PersID).ToList();

            if (monsterMatch.Count > 1)
            {
                log.Info("Multiple contractors IDs Found: " + contractorData.Person.PersID);

                return null;
            }
            else if (monsterMatch.Count == 1)
            {
                log.Info("Matching record found by GCIMSID: " + contractorData.Person.PersID);

                return monsterMatch.Single();
            }
            else if (monsterMatch.Count == 0)
            {
                log.Info("Trying to match record by Lastname, Birth Date and SSN: " + contractorData.Person.PersID);

                var nameMatch = allGcimsData.Where(w =>
                    contractorData.Person.LastName.ToLower().Trim().Equals(w.Person.LastName.ToLower().Trim()) &&
                    contractorData.Person.SocialSecurityNumber.Equals(w.Person.SocialSecurityNumber) &&
                    contractorData.Birth.DateOfBirth.Equals(w.Birth.DateOfBirth)).ToList();

                if (nameMatch.Count == 0 || nameMatch.Count > 1)
                {
                    log.Info("Match not found by name for user: " + contractorData.Person.PersID);
                    return null;
                }
                else if (nameMatch.Count == 1)
                {
                    log.Info("Match found by name for user: " + contractorData.Person.PersID);
                    return nameMatch.Single();
                }
            }

            return null;
        }

    }
}