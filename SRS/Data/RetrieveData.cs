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

            retrieveMapper.ConfigurationProvider.CompileMappings();
        }

        public List<ContractorData> ExpiringContractor(DateTime accessingDate)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["GCIMS"].ToString());

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

                            int daysUntilExpired = (int)(DateTime.Today - expiryDate).TotalDays;
                            string labelCaption = String.Format("{0} day(s) left.", daysUntilExpired);
                            //expiryDate = time.ToFileTimeUtc();

                            //storeExpiry(expiryDate.ToString());
                            //if (DateTime.Today > expiryDate)
                            //if (daysUntilExpired < 60)
                            //{
                            //    string lableCaption = daysUntilExpired + "days more to contract expired.";
                            //    //Contract Expired    
                            //    DateTime time = DateTime.Today.AddDays(60);
                            //    // expiryDate = time.ToFileTimeUtc();
                            //    //storeExpiry(expiryDate.ToString());
                            //    //useSeconds = false;

                            //}
                            if (daysUntilExpired <= 45)
                            {
                                string lableCaption = daysUntilExpired + "days more to contract expired.";
                                DateTime time = DateTime.Today.AddDays(45);
                                // useSeconds = false;
                            }
                            //else if (DateTime.Today < expiryDate)
                            else if (daysUntilExpired <= 30)
                            {
                                string lableCaption = daysUntilExpired + "days more to contract expired.";
                                DateTime time = DateTime.Today.AddDays(30);
                                //useSeconds = false;
                            }
                            else if (daysUntilExpired <= 15)
                            {
                                string lableCaption = daysUntilExpired + "days more to contract expired.";
                                DateTime time = DateTime.Today.AddDays(15);
                                //useSeconds = false;
                            }
                            else if (daysUntilExpired <= 7)
                            {
                                string lableCaption = daysUntilExpired + "days more to contract expired.";
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
                //allExpiringContractor.Contract_id = retrieveMapper.Map<IDataReader, >(contractorData);
                //allExpiringContractor.contract_unid = contractorData.GetInt64(2);
                allExpiringContractor.contract_vender_ID = contractorData.GetInt32(9);
                //allExpiringContractor.contract_number = contractorData.GetInt64(4);
                //allExpiringContractor.contract_name = contractorData[1].ToString();
                allExpiringContractor.contract_date_end = (DateTime)contractorData[5];
                allExpiringContractor.DaysUntilExpired = contractorData.GetInt32(6);
                allExpiringContractor.RegionalEmail = contractorData[7].ToString();
                allExpiringContractor.contract_POC_Email = contractorData[8].ToString();

                allRecords.Add(allExpiringContractor);
            }

            return allRecords;
        }
        
        //public List<Contractor> AllExpiredContractorData(DateTime accessingDate)
        //{
             
        //        cmd.CommandText = "SRS_Expired_Contracts";
        //        log.Info("Contractor data of expiration: " + DateTime.Now);
        //        AllExpiredContractorData = cmd.ExecuteReader();
        //        var myReader = cmd.ExecuteReader();
        //        if (myReader.HasRows)
        //        {
        //            //record found
        //            myReader.Read();
        //            var expiryDate = myReader.GetDateTime("expirationdate");

        //            int daysUntilExpired = (int)(DateTime.Today - expiryDate).TotalDays;
        //            string labelCaption = String.Format("{0} day(s) left.", daysUntilExpired);
        //            // if (DateTime.Today <= expiryDate)
        //            if (daysUntilExpired <= 0)
        //            {
                       
        //                string lableCaption = daysUntilExpired + " Contract expired.";
        //                DateTime time = DateTime.Today.AddDays(0);
        //            }
        //        }
               
        //     return AllExpiredContractorData;
        //    }
             
          }
    }

 