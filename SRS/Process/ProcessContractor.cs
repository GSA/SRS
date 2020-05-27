using AutoMapper;
using SRS.Data;
using SRS.Utilities;
using SRS.Mapping;
using SRS.Models;
using System;
using System.Collections.Generic;

namespace SRS.Process
{
    internal class ProcessContractor
    {
        //Reference to logger
        private static log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly RetrieveData retrieve;
        private readonly EmailData emailData;
    
        public void ProcessExpiringContractor(ref EmailData emailData)
        {
            ExpiringContractor expiringContractor = new ExpiringContractor(ref emailData);

            try
            {
                expiringContractor.ProcessExpiringContractor();
                var summary = new ContractorSummary();
                var fileReader = new FileReader();
            }
            catch(Exception ex)
            {
                _log.Error("Getting Contracts:" + "-" + ex.Message + "-" + ex.InnerException);
            }
        }
        
            
        public void ProcessExpiredContractor()
        {
            ExpiredContractorSummary expiredContractor = new ExpiredContractorSummary();
        }
    }
     
}
