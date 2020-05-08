using AutoMapper;
using SRS.Data;
using SRS.Lookups;
using SRS.Mapping;
using SRS.Process;
using SRS.Models;
using System.Diagnostics;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace SRS
{
    public static class Program
    { 
   //Reference to logger
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    //File paths from config file
    private static string ContractorFilePath = ConfigurationManager.AppSettings["ContractorFILEPATH"].ToString();
       // private static string ConnectionString = ConfigurationManager.ConnectionStrings["hspd"].ConnectionString;
    //Stopwatch objects
    private static Stopwatch timeForApp = new Stopwatch();

    private static Stopwatch timeForProcess = new Stopwatch();

    private static SRSMapper map = new SRSMapper();

    private static IMapper dataMapper;
    private static EmailData emailData = new EmailData();
    private static bool expiringContractor = false;
    private static bool expiredContractor = false;

    public static void Main(string[] args)
    {
        //Start timer
        timeForApp.Start();

        //Log start of application
        log.Info("Application Started: " + DateTime.Now);

       // CreateMaps();
             
        ProcessContractor processContractor = new ProcessContractor(dataMapper, ref emailData); 
        SendSummary sendSummary = new SendSummary(ref emailData);
            emailData.TimeBegin = DateTime.Now;
           // emailData.AccessingDate = emailData.AccessingDate.GetDateTimeFormats(args.Length < 1 ? null: args[0]);

            //Log action
            log.Info("First step of App Settings:" + DateTime.Now);
            StartProcessing();
            log.Info("Processing of App Settings:" + DateTime.Now);
            log.Info("Contractors File Processing:" + DateTime.Now);

            //Contractor file
            if (File.Exists(ContractorFilePath))
            {
                if (expiringContractor)
                {
                    log.Info("Time for Start Expiring Contractor Processing: " + DateTime.Now);
                    processContractor.ProcessExpiringContractor();
                    log.Info("Time to Stop Expiring Contractor Processing:" + DateTime.Now);
                }
                if(expiredContractor)
                {
                    log.Info("Time for Start Expired Contractor Processing: " + DateTime.Now);
                    processContractor.ProcessExpiredContractor();
                    log.Info("Time to Stop Expired Contractor Processing:" + DateTime.Now);
                }
                //log.Info("Start Contractor file Processing: " + DateTime.Now);

                //timeForProcess.Start();
                ////processContractor.ProcessContractorFile(ContractorFilePath);
                //timeForProcess.Stop();

                log.Info("Done Contractor File processing: " + DateTime.Now);
                log.Info("Contractor File Processing Time: " + timeForProcess.ElapsedMilliseconds);
                 
            }
        else
            {
                log.Error("Contractor File not found");
            }
         
        log.Info("Done Contractor File(s) Processing :" + DateTime.Now);

        log.Info("Sending Summary");

            sendSummary.SendSummaryEmail();

        log.Info("Summary sent");

        //Stop second timer
        timeForApp.Stop();

        //Log total time
        log.Info(string.Format("Contractor processing Completed in {0} milliseconds", timeForApp.ElapsedMilliseconds));

        //Log processing end
        log.Info("The end of processing Contractor: " + DateTime.Now);
    }

        private static void StartProcessing()
        {
            if (!Boolean.TryParse("EXPIRINGCONTRACTOR", out expiringContractor))
                expiringContractor = true;
            if (!Boolean.TryParse("EXPIREDCONTRACTOR", out expiredContractor))
                expiredContractor = true;
        }

        //private static void CreateMaps()
        //{
        //    map.CreateDataConfig();
        //    dataMapper = map.CreateDataMapping();
        //}

        //private static Lookup createLookups()
        //{
            //Lookup lookups;
            //SRSMapper contractormap = new SRSMapper();
            //IMapper lookupMapper;

            //contractormap.CreateLookupConfig();

            //lookupMapper = contractormap.CreateLookupMapping();

            //LoadLookupData loadLookupData = new LoadLookupData(lookupMapper);

            //lookups = loadLookupData.GetContractorLookupData();

            //return lookups;
        //}
   }

}
