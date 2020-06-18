using SRS.Utilities;
using SRS.Models;
using System;

namespace SRS.Process
{
    internal class ProcessContractor
    {
        //Reference to logger
        private static log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
         

        public void ProcessExpiringContractor(ref EmailData emailData)
        {
 
            ExpiringContractor expiringContractor = new ExpiringContractor(ref emailData);
         
            try
            {
                expiringContractor.ProcessExpiringContractor();
                var summary = new ExpiringContractorSummary();
                var fileReader = new FileReader(); 
            }
            catch(Exception ex)
            {
                _log.Error("Getting Contractor:" + "-" + ex.Message + "-" + ex.InnerException);
            }  
        } 
      public void ProcessExpiredContractor(ref EmailData emailData)
        {
            ExpiredContractor  expiredContractor = new ExpiredContractor(ref emailData);
            try
            {
                expiredContractor.ProcessExpiredContractor();
                var summary = new ExpiredContractorSummary();
                var fileReader = new FileReader();
            }
            catch(Exception ex)
            {
                _log.Error("Getting Contractor:" + "_" + ex.Message + "_" + ex.InnerException);
            }
        }
    }
     
}
