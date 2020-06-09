using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using SRS.Process;

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

        //internal static void AddBadRecordsToSummary(List<string> badRecords, ref ContractorSummary summary)
        //{
        //    throw new NotImplementedException();
        //}
    }
    }