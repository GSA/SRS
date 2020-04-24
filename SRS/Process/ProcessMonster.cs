using AutoMapper;
using SRS.Data;
using SRS.Lookups;
using SRS.Utilities;
using SRS.Mapping;
using SRS.Models;
using SRS.Validation;
using MySql.Data.MySqlClient;
using System;
using log4net;
using System.Configuration;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRS.Process
{
    internal class ProcessMonster
    {
        //Reference to logger
        private static log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly EmailData emailData;
        private readonly RetrieveData retrieve;
        readonly Lookup lookups;

        //private bool useSeconds = false;


        public ProcessMonster(IMapper dataMapper, ref EmailData emailData, Lookup lookups)
        {
            //InitializeComponent();
            retrieve = new RetrieveData(dataMapper);
            this.lookups = lookups;
            this.emailData = emailData;
        }

        public void ProcessMonsterFile(string MonsterFile)
        {
            _log.Info("Processing Monster File");

            try
            {
                Contractor gcimsRecord;
                var columnList = string.Empty;
                var summary = new MonsterSummary();
                var fileReader = new FileReader();
                var validate = new ValidateMonster(lookups);
                var save = new SaveData();
                var em = new ContractorMapping(lookups);
                List<string> badRecords;

                _log.Info("Loading Monster Links File");

                var usersToProcess = fileReader.GetFileData<Contractor, ContractorMapping>(MonsterFile, out badRecords, em);
                Helpers.AddBadRecordsToSummary(badRecords, ref summary);

                _log.Info("Loading GCIMS Data");
                var hspd = retrieve.AllGCIMSData();

                ProcessResult updatedResults;

                //start processing the Monster data
                foreach (var contractorData in usersToProcess)
                {
                    _log.Info("Processing Monster user: " + contractorData.Person.GCIMSID);
                    //looking for matching record.
                    _log.Info("Looking for matching record: " + contractorData.Person.GCIMSID);
                    gcimsRecord = Helpers.RecordFound(contractorData, hspd, ref _log);

                    if ((gcimsRecord != null && (gcimsRecord.Person.GCIMSID != contractorData.Person.GCIMSID) && (!Convert.ToBoolean(ConfigurationManager.AppSettings["DEBUG"].ToString()))))
                    {
                        _log.Info("Adding GCIMSID to record: " + gcimsRecord.Person.GCIMSID);
                        save.InsertGCIMSID(gcimsRecord.Person.GCIMSID, contractorData.Person.GCIMSID);
                    }
                    //If no record found write to the record not found summary file
                    if (gcimsRecord == null)
                    {
                        summary.RecordsNotFound.Add(new RecordNotFoundSummary
                        {
                            GCIMSID = -1,
                            //FirstName = contractorData.Person.FirstName,
                            //MiddleName = contractorData.Person.MiddleName,
                            //LastName = contractorData.Person.LastName,
                            //Suffix = contractorData.Person.Suffix
                        });
                    }
                    //if there are critical errors write to the error summary and move to the next record
                    _log.Info("checking critical errors for users: " + contractorData.Person.GCIMSID);
                    if (Helpers.CheckErrors(validate, contractorData, summary.UnsuccessfulUsersProcessed, ref _log))
                        continue;
                }

            }
            catch (Exception ex)
            {
                _log.Error("Process Monster File Error:" + ex.Message + " " + ex.InnerException + " " + ex.StackTrace);
            }
        }
    }
}