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
       
        public List<Contractor> GetExpiringContractor(DateTime accessingDate)
        {  
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
                         
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "SRS_GetExpiringContractors";
                        cmd.Parameters.Clear();

                        cmd.Parameters.AddWithValue("inDate", "2020-06-06");// accessingDate); //"2020-06-06"
                        MySqlDataReader expiringContractorData = cmd.ExecuteReader();

                        log.Info("Contractor data of expiration: " + DateTime.Now);
                        
                        log.Info("Contractor Retrieved Data: " + DateTime.Now);
                           
                        while (expiringContractorData.Read())
                        {
                            allExpiringContractorData.Add(
                                new Contractor
                                {
                                     LastName = expiringContractorData[0].ToString(),
                                      Suffix = expiringContractorData[1].ToString(),
                                      FirstName = expiringContractorData[2].ToString(),
                                      MiddleName = expiringContractorData[3].ToString(), 
                                      DaysToExpiration = expiringContractorData.GetInt32(4),
                                      gpoc_emails = expiringContractorData[5].ToString(),
                                      vpoc_emails = expiringContractorData[6].ToString(),
                                      RegionalEMails = expiringContractorData[7].ToString(),
                                      MajorEMails = expiringContractorData[8].ToString(), 
                                     // pers_investigation_date = (DateTime)expiringContractorData[9]                           
                                  }
                               );
                    }
                        log.Info("Adding Contractor expiring data to object: " + DateTime.Now);
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

                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "SRS_GetExpiredContractors";
                        cmd.Parameters.Clear();

                        cmd.Parameters.AddWithValue("inDate", "2020-06-06");//accessingDate); //"2020-06-06"

                        MySqlDataReader expiredContractorData = cmd.ExecuteReader();
                        log.Info("Contractor data of expiration: " + DateTime.Now);
                         
                        log.Info("Contractor Retrieved Data: " + DateTime.Now);
                        log.Info("Adding Contractor expired data to object: " + DateTime.Now);

                        while (expiredContractorData.Read())
                        {
                            allExpiredContractorData.Add(

                                new Contractor
                                {
                                    LastName = expiredContractorData[0].ToString(),
                                    Suffix = expiredContractorData[1].ToString(),
                                    FirstName = expiredContractorData[2].ToString(),
                                    MiddleName = expiredContractorData[3].ToString(),
                                    DaysToExpiration = expiredContractorData.GetInt32(4),
                                    gpoc_emails = expiredContractorData[5].ToString(),
                                    vpoc_emails = expiredContractorData[6].ToString(),
                                    RegionalEMails = expiredContractorData[7].ToString(),
                                    MajorEMails = expiredContractorData[8].ToString(),
                                    //pers_investigation_date = (DateTime)expiredContractorData[9]

                                } );
                                   
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
 