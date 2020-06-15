using AutoMapper;
using SRS.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using SRS.Process;

namespace SRS.Data
{
    internal class RetrieveData
    {
        //Reference to logger
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //private readonly IMapper retrieveMapper;
        ////private IMapper dataMapper;

        //public RetrieveData(IMapper mapper)
        //{
        //    retrieveMapper = mapper;

        //    retrieveMapper.ConfigurationProvider.CompileMappings();
        //}
        public List<Contractor> GetExpiringContractor(DateTime accessingDate)
        {

           // MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["hspd"].ToString());
            try
            {
                log.Info("Getting Contractor Data");
                MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["hspd"].ToString());

                MySqlCommand cmd = new MySqlCommand();
                  
                List<Contractor> allExpiringContractorData = new List<Contractor>();

                using (conn)
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    using (cmd)
                    {
                      MySqlDataReader expiringContractorData;
                       
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "SRS_GetContractors";
                        cmd.Parameters.Clear();

                        //MySqlDbType todaysDate = default(MySqlDbType);
                        //cmd.Parameters.Add("DateTime", todaysDate);
                        cmd.Parameters.AddWithValue("inDate", "2020-06-06");// accessingDate); //"2020-06-06"

                        log.Info("Contractor data of expiration: " + DateTime.Now);
                        expiringContractorData = cmd.ExecuteReader();
                        log.Info("Contractor Retrieved Data: " + DateTime.Now);
                        log.Info("Adding Contractor expiring data to object: " + DateTime.Now);

                        while (expiringContractorData.Read())
                        {
                            allExpiringContractorData.Add(    
                                new Contractor
                                 {                              
                                      Pers_id = expiringContractorData[0].ToString(),
                                      LastName = expiringContractorData[1].ToString(),
                                      FirstName = expiringContractorData[2].ToString(),
                                      MiddleName = expiringContractorData[3].ToString(),
                                      Suffix = expiringContractorData[4].ToString(),
                                      DaysToExpiration = expiringContractorData.GetInt32(5),
                                      vpoc_emails = expiringContractorData[6].ToString(),
                                      gpoc_emails = expiringContractorData[7].ToString(),
                                      pers_status = expiringContractorData[8].ToString(),
                                      pers_investigation_date = (DateTime)expiringContractorData[9]
                                                                 
                                  }
                               );
                        }
                        log.Info("Adding Contractor expired data to object: " + DateTime.Now);
                    }
                    return allExpiringContractorData;
                }
            }
            catch (Exception ex)
            {
                log.Error("GetContractorRecord: " + " - " + ex.Message + " - " + ex.InnerException);
                return new List<Contractor>();
            }
             
            }

            public List<Contractor> allExpiredContractorData(DateTime accessingDate)
              {
           // MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["hspd"].ToString());
            try
            {
                MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["hspd"].ToString());

                MySqlCommand cmd = new MySqlCommand();
                List<Contractor> allExpiredContractorData = new List<Contractor>();

                using (conn)
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    using (cmd)
                    {
                        MySqlDataReader expiredContractorData;


                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "SRS_GetContractors";
                        cmd.Parameters.Clear();

                        //MySqlDbType todaysDate = default(MySqlDbType);
                        //cmd.Parameters.Add("DateTime", todaysDate);
                        cmd.Parameters.AddWithValue("inDate", "2020-06-06");// accessingDate); //"2020-06-06"
                        log.Info("Contractor data of expiration: " + DateTime.Now);
                        expiredContractorData = cmd.ExecuteReader();
                        log.Info("Contractor Retrieved Data: " + DateTime.Now);
                        log.Info("Adding Contractor expiring data to object: " + DateTime.Now);
 
                        while (expiredContractorData.Read())
                        {
                            allExpiredContractorData.Add(
                                                            
                                new Contractor
                                  { 
                                       Pers_id = expiredContractorData[0].ToString(),
                                       LastName = expiredContractorData[1].ToString(),
                                       FirstName = expiredContractorData[2].ToString(),
                                       MiddleName = expiredContractorData[3].ToString(),
                                       Suffix = expiredContractorData[4].ToString(),
                                       DaysToExpiration = expiredContractorData.GetInt32(5),
                                       vpoc_emails = expiredContractorData[6].ToString(),
                                       gpoc_emails = expiredContractorData[7].ToString(),
                                       pers_status = expiredContractorData[8].ToString(),
                                       pers_investigation_date = (DateTime)expiredContractorData[9]
         
                                  }
                                    );
                        }
                 log.Info("Adding Contractor expired data to object: " + DateTime.Now);
                    }
                    return allExpiredContractorData;
                }
            }
            catch (Exception ex)
            {
                log.Error("GetContractorRecord: " + " - " + ex.Message + " - " + ex.InnerException);
                return new List<Contractor>();
            }
        }

    }
}
 