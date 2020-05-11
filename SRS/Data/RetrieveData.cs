using AutoMapper;
using SRS.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SRS.Process;
  
namespace SRS.Data
{
    internal class RetrieveData
    {
        //Reference to logger
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IMapper retrieveMapper;

        public RetrieveData(IMapper mapper)
        {
            retrieveMapper = mapper;

            //retrieveMapper.ConfigurationProvider.CompileMappings();
        }

        public List<ContractorData> ExpiringContractor(DateTime accessingDate)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["hspd"].ToString());

                MySqlCommand cmd = new MySqlCommand();
                //MySqlConnection conn = new MySqlConnection("Server=[IP Address]; database=hspd; UID=[username]; Password=[password]");

                //conn.Open();
                ////Using Parameters
                //MySqlCommand cmd = new MySqlCommand("SRS_GetContractors", conn);

                List<ContractorData> allExpiringContractorData = new List<ContractorData>();

                using (conn)
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    using (cmd)
                    {
                        MySqlDataReader contractorData;

                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "SRS_GetContractors";
                        cmd.Parameters.Clear();

                        MySqlDbType todaysDate = default(MySqlDbType);
                        cmd.Parameters.Add("DateTime", todaysDate);

                        log.Info("Contractor data of expiration: " + DateTime.Now);

                        contractorData = cmd.ExecuteReader();
                        var myReader = cmd.ExecuteReader();
                        if (myReader.HasRows)
                        {
                            //record found
                            myReader.Read();
                            var expiryDate = myReader.GetDateTime("expirationdate");

                            int daysToExpiration = (int)(DateTime.Today - expiryDate).TotalDays;
                            string labelCaption = String.Format("{0} day(s) left.", daysToExpiration);
                            //expiryDate = time.ToFileTimeUtc();
 
                            //else if (DateTime.Today < expiryDate)
                            // else if (daysToExpiration <= 30)
                            if (daysToExpiration <= 30)
                            {
                                string lableCaption = daysToExpiration + "days more to contract expired.";
                                DateTime time = DateTime.Today.AddDays(30);
                                //useSeconds = false;
                            }
                            else if (daysToExpiration <= 15)
                            {
                                string lableCaption = daysToExpiration + "days more to contract expired.";
                                DateTime time = DateTime.Today.AddDays(15);
                                //useSeconds = false;
                            }
                            else if (daysToExpiration <= 7)
                            {
                                string lableCaption = daysToExpiration + "days more to contract expired.";
                                DateTime time = DateTime.Today.AddDays(7);
                                //useSeconds = false;
                            }

                        }

                        using (contractorData)
                        {
                            if (contractorData.HasRows)
                            {
                                allExpiringContractorData = MapAllContractorData(contractorData);
                            }
                        }
                    }
                }

                return allExpiringContractorData;
            }
            catch (Exception ex)
            {
                log.Error("GetContractorRecord: " + " - " + ex.Message + " - " + ex.InnerException);
                return new List<ContractorData>();
            }
        }
             private List<ContractorData> MapAllContractorData(MySqlDataReader contractorData)
        {
            List<ContractorData> allRecords = new List<ContractorData>();

            log.Info("Contractor Retrieved Data: " + DateTime.Now);
            log.Info("Adding Contractor expiring data to object: " + DateTime.Now);
            while (contractorData.Read())
            {
                ContractorData allExpiringContractor = new ContractorData();
                allExpiringContractor.PersID = contractorData[0].ToString();
                allExpiringContractor.LastName = contractorData[1].ToString();
                allExpiringContractor.FirstName = contractorData[2].ToString();
                allExpiringContractor.MiddleName = contractorData[3].ToString();
                allExpiringContractor.Suffix = contractorData[4].ToString(); 
                allExpiringContractor.conpoc_email = contractorData[9].ToString(); 
                allExpiringContractor.pers_investigation_date = (DateTime)contractorData[5];
                allExpiringContractor.DaysToExpiration = contractorData.GetInt32(6);
                allExpiringContractor.RegionalEmail = contractorData[7].ToString();
                allExpiringContractor.pers_work_email = contractorData[8].ToString();

                allRecords.Add(allExpiringContractor);
            }
            return allRecords;
        }
        public List<ContractorData> allExpiredContractorData(DateTime accessingDate)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["hspd"].ToString());

                MySqlCommand cmd = new MySqlCommand();
                List<ContractorData> allExpiredContractorData = new List<ContractorData>();

                using (conn)
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    using (cmd)
                    {
                        MySqlDataReader contractorData;

                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "SRS_GetContractors";
                        cmd.Parameters.Clear();

                        MySqlDbType todaysDate = default(MySqlDbType);
                        cmd.Parameters.Add("DateTime", todaysDate);

                        log.Info("Contractor data of expiration: " + DateTime.Now);

                        contractorData = cmd.ExecuteReader();
                        var myReader = cmd.ExecuteReader();
                        if (myReader.HasRows)
                        {
                            //record found
                            myReader.Read();
                            var expiryDate = myReader.GetDateTime("expirationdate");

                            int daysToExpiration = (int)(DateTime.Today - expiryDate).TotalDays;
                            string labelCaption = String.Format("{0} day(s) left.", daysToExpiration);
                            // if (DateTime.Today <= expiryDate)
                            if (daysToExpiration <= 0)
                            {

                                string lableCaption = daysToExpiration + " Contract expired.";
                                DateTime time = DateTime.Today.AddDays(0);
                            }
                        }
                    }
                }

                return allExpiredContractorData;
            }
            catch (Exception ex)
            {
                log.Error("GetContractorRecord: " + " - " + ex.Message + " - " + ex.InnerException);
                return new List<ContractorData>();
            }

        }
        private List<ContractorData> MapAllExpiredContractorData(MySqlDataReader contractorData)
        {
            List<ContractorData> allRecords = new List<ContractorData>();

            log.Info("Contractor Retrieved Data: " + DateTime.Now);
            log.Info("Adding Contractor expiring data to object: " + DateTime.Now);
            while (contractorData.Read())
            {
                ContractorData allExpiredContractor = new ContractorData();
                allExpiredContractor.PersID = contractorData[0].ToString();
                allExpiredContractor.LastName = contractorData[1].ToString();
                allExpiredContractor.FirstName = contractorData[2].ToString();
                allExpiredContractor.MiddleName = contractorData[3].ToString();
                allExpiredContractor.Suffix = contractorData[4].ToString(); 
                allExpiredContractor.conpoc_email = contractorData.ToString(); 
                allExpiredContractor.pers_investigation_date = (DateTime)contractorData[5];
                allExpiredContractor.DaysToExpiration  = contractorData.GetInt32(6);
                allExpiredContractor.RegionalEmail = contractorData[7].ToString();
                allExpiredContractor.pers_work_email = contractorData[8].ToString();

                allRecords.Add(allExpiredContractor);
            }

            return allRecords;
        }

        
        }
    }

 