using SRS.Mapping;
using SRS.Process;
using SRS.Models;
using System.Diagnostics;
using System;
using System.Configuration;
using SRS.Utilities;

namespace SRS
{
    public class Program
    {
        //Reference to logger
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //File paths from config file
        private static string ExpiringSACFilePath = ConfigurationManager.AppSettings["ExpiringSACFilePath"].ToString();
        private static string ExpiredSACFilePath = ConfigurationManager.AppSettings["ExpiredSACFilePath"].ToString();
        
        //Stopwatch objects
        private static Stopwatch timeForApp = new Stopwatch();

        private static Stopwatch timeForProcess = new Stopwatch();

        private static SRSMapper map = new SRSMapper();

        //private static IMapper dataMapper;
        private static EmailData emailData = new EmailData();
        private static bool expiringContractor = false;
        private static bool expiredContractor = false;
        //private static object dataMapper;

        public static void Main(string[] args)
        {   
            //Start timer
            timeForApp.Start();

            //Log start of application
            _log.Info("Application Started: " + DateTime.Now);
            ProcessContractor processContractor = new ProcessContractor();
            SendSummary sendSummary = new SendSummary(ref emailData);
            emailData.TIMEBEGIN = DateTime.Now;
            emailData.ACCESSINGDATE = emailData.ACCESSINGDATE.GetDateTime(args.Length <1 ? null : args[0]);

            //Log action
            _log.Info("First step of App Settings:" + DateTime.Now);
            StartProcessing();
            _log.Info("Processing of App Settings:" + DateTime.Now);
            _log.Info("Contractors File Processing:" + DateTime.Now);
               
           if (expiringContractor)
             {
                _log.Info("Time for Start Expiring Contractor Processing: " + DateTime.Now);
                    timeForProcess.Start(); 
                    processContractor.ProcessExpiringContractor(ref emailData);
                    timeForProcess.Stop();
                _log.Info("Time to Stop Expiring Contractor Processing:" + DateTime.Now);
                _log.Info("Expiring Contractor File processing time: " + timeForProcess.ElapsedMilliseconds);
              }
            else
              {
                _log.Error("Expiring Contractor File not found");
              }
            
             if (expiredContractor)
               {
                _log.Info("Time for Start Expired Contractor Processing: " + DateTime.Now);
                    timeForProcess.Start();
                    processContractor.ProcessExpiredContractor(ref emailData);
                    timeForProcess.Stop();
                _log.Info("Time to Stop Expired Contractor Processing:" + DateTime.Now);
                _log.Info("Expired Contractor File processing time: " + timeForProcess.ElapsedMilliseconds);
               }
            
            else
              {
                _log.Error("Expired Contractor File not found");
              }

            _log.Info("Done Contractor File(s) Processing :" + DateTime.Now);

            emailData.ENDTIME = DateTime.Now; 
           
            //Stop second timer
            timeForApp.Stop();
            emailData.ACCESSINGTIME = timeForApp.ElapsedMilliseconds;
            _log.Info("Sending SummaryEmail"); 
            sendSummary.SummaryEmailContent();

            _log.Info("SummaryEmail sent");
            //Log total time
            _log.Info(string.Format("Contractor processing Completed in {0} milliseconds", timeForApp.ElapsedMilliseconds));

            //Log processing end
            _log.Info("The end of processing Contractor: " + DateTime.Now);
             
        } 
        private static void StartProcessing()
        {
            if (!bool.TryParse("ExpiringContractor".GetEmailSetting(), out expiringContractor))
                expiringContractor = true;
            if (!bool.TryParse("ExpiredContractor".GetEmailSetting(), out expiredContractor))
                expiredContractor = true;
        }
        
    }
}
