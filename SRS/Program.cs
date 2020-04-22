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
    internal static class Program
    { 
   //Reference to logger
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    //File paths from config file
    private static string MonsterFilePath = ConfigurationManager.AppSettings["MonsterFILE"].ToString();

    //Stopwatch objects
    private static Stopwatch timeForApp = new Stopwatch();

    private static Stopwatch timeForProcess = new Stopwatch();

    private static SRSMapper map = new SRSMapper();

    private static IMapper dataMapper;
    private static EmailData emailData = new EmailData();

        public static void Main(string[] args)
    {
        //Start timer
        timeForApp.Start();

        //Log start of application
        log.Info("Application Started: " + DateTime.Now);

        CreateMaps();

        Lookup lookups = createLookups();

        ProcessMonster processMonster = new ProcessMonster(dataMapper, ref emailData, lookups);
        SendSummary sendSummary = new SendSummary(ref emailData);

            //Log action
            log.Info("Processing Monster File:" + DateTime.Now);

            //Monster file
            if (File.Exists(MonsterFilePath))
            {
                log.Info("Start Monster file Processing: " + DateTime.Now);

                timeForProcess.Start();
                processMonster.ProcessMonsterFile(MonsterFilePath);
                timeForProcess.Stop();

                log.Info("Done Monster File processing: " + DateTime.Now);
                log.Info("Monster File Processing Time: " + timeForProcess.ElapsedMilliseconds);
                 
            }
        else
            {
                log.Error("Monster File not found");
            }
         
        log.Info("Done Monster File(s) Processing :" + DateTime.Now);

        log.Info("Sending Summary File");

            sendSummary.SendSummaryEmail();

        log.Info("Summary file sent");

        //Stop second timer
        timeForApp.Stop();

        //Log total time
        log.Info(string.Format("Application Completed in {0} milliseconds", timeForApp.ElapsedMilliseconds));

        //Log application end
        log.Info("Application Done: " + DateTime.Now);
    }

    private static void CreateMaps()
        {
            map.CreateDataConfig();
            dataMapper = map.CreateDataMapping();
        }

        private static Lookup createLookups()
        {
            Lookup lookups;
            SRSMapper monstermap = new SRSMapper();
            IMapper lookupMapper;

            monstermap.CreateLookupConfig();

            lookupMapper = monstermap.CreateLookupMapping();

            LoadLookupData loadLookupData = new LoadLookupData(lookupMapper);

            lookups = loadLookupData.GetEmployeeLookupData();

            return lookups;
        }
   }

}
