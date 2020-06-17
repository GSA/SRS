﻿using AutoMapper;
using SRS.Mapping;
using SRS.Process;
using SRS.Models;
using SRS.Data;
using System.Diagnostics;
using System.IO;
using System;
using System.Configuration;
using SRS.Utilities;

namespace SRS
{
    public class Program
    {
        //Reference to logger
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //File paths from config file
        private static string ExpiringSACFilePath = ConfigurationManager.AppSettings["ExpiringSACFilePath"].ToString();
        private static string ExpiredSACFilePath = ConfigurationManager.AppSettings["ExpiredSACFilePath"].ToString();
        
        //Stopwatch objects
        private static Stopwatch timeForApp = new Stopwatch();

        private static Stopwatch timeForProcess = new Stopwatch();

        private static SRSMapper map = new SRSMapper();

        //private static IMapper dataMapper;
        private static EmailData emailData = new EmailData();
        private static bool expiringContractor = true;
        private static bool expiredContractor = true;
        private static object dataMapper;

        public static void Main(string[] args)
        {  
            ProcessContractor processContractor = new ProcessContractor();
            SendSummary sendSummary = new SendSummary(ref emailData);
            //Start timer
            timeForApp.Start();
             
            //Log start of application
            log.Info("Application Started: " + DateTime.Now);

            CreateMaps();

           
            emailData.TimeBegin = DateTime.Now;
            emailData.AccessingDate = emailData.AccessingDate.GetDateTime(args.Length <1 ? null : args[0]);

            //Log action
            log.Info("First step of App Settings:" + DateTime.Now);
            StartProcessing();
            log.Info("Processing of App Settings:" + DateTime.Now);
            log.Info("Contractors File Processing:" + DateTime.Now);
              
           //if (File.Exists(ExpiringSACFilePath))
           if (expiringContractor)
             {
                    log.Info("Time for Start Expiring Contractor Processing: " + DateTime.Now);
                    timeForProcess.Start(); 
                    processContractor.ProcessExpiringContractor(ref emailData);
                    timeForProcess.Stop();
                    log.Info("Time to Stop Expiring Contractor Processing:" + DateTime.Now);
                    log.Info("Expiring Contractor File processing time: " + timeForProcess.ElapsedMilliseconds);
              }
            else
              {
                log.Error("Expiring Contractor File not found");
              }
            //if (File.Exists(ExpiredSACFilePath))
                if (expiredContractor)
                {
                    log.Info("Time for Start Expired Contractor Processing: " + DateTime.Now);
                    timeForProcess.Start();
                    processContractor.ProcessExpiredContractor(ref emailData);
                    timeForProcess.Stop();
                    log.Info("Time to Stop Expired Contractor Processing:" + DateTime.Now);
                    log.Info("Expired Contractor File processing time: " + timeForProcess.ElapsedMilliseconds);
                }
            
            else
              {
                log.Error("Expired Contractor File not found");
              }

            log.Info("Done Contractor File(s) Processing :" + DateTime.Now);

            log.Info("Sending SummaryEmail");

            sendSummary.SummaryEmailContent();

            log.Info("SummaryEmail sent");

            //Stop second timer
            timeForApp.Stop();

            //Log total time
            log.Info(string.Format("Contractor processing Completed in {0} milliseconds", timeForApp.ElapsedMilliseconds));

            //Log processing end
            log.Info("The end of processing Contractor: " + DateTime.Now);
      
        }
        private static void CreateMaps()
        {
            map.CreateDataConfig();
            dataMapper = map.CreateDataMapping();
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
