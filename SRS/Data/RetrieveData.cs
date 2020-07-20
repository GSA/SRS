using SRS.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace SRS.Data
{
    internal class RetrieveData
    {
        //Reference to logger
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       
        public List<Contractor> GetExpiringContractor(DateTime accessingDate)
        {  
            try
            {
                _log.Info("Getting Contractor Data");
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

                        cmd.Parameters.AddWithValue("inDate", "2020-06-06");// accessingDate); //ACCESSINGDATE
                        MySqlDataReader expiringContractorData = cmd.ExecuteReader();

                        _log.Info("Contractor data of expiration: " + DateTime.Today);

                        _log.Info("Contractor Retrieved Data: " + DateTime.Today);
                        _log.Info("Adding Contractor expiring data to object: " + DateTime.Today);

                        while (expiringContractorData.Read())
                        {
                            allExpiringContractorData.Add(
                                new Contractor
                                {
                                      PersonID = expiringContractorData.GetInt32(0),
                                      LastName = expiringContractorData[1].ToString(),
                                      Suffix = expiringContractorData[2].ToString(),
                                      FirstName = expiringContractorData[3].ToString(),
                                      MiddleName = expiringContractorData[4].ToString(),
                                      InvestigationDate = (DateTime)expiringContractorData[5],
                                      DaysToExpiration = expiringContractorData.GetInt32(6),
                                      gpoc_emails = expiringContractorData[7].ToString(),
                                      vpoc_emails = expiringContractorData[8].ToString(),
                                      RegionalEMails = expiringContractorData[9].ToString(),
                                      MajorEMails = expiringContractorData[10].ToString()
                                                               
                                }
                               );
                    }
                        _log.Info("Adding Contractor expiring data to object: " + DateTime.Today);
                    }
                    return allExpiringContractorData;
                }
            }
            catch (Exception ex)
            {
                _log.Error("GetContractorRecord: " + " - " + ex.Message + " - " + ex.InnerException);
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

                        cmd.Parameters.AddWithValue("inDate", "2020-06-06");//accessingDate); //ACCESSINGDATE

                        MySqlDataReader expiredContractorData = cmd.ExecuteReader();
                        _log.Info("Contractor data of expiration: " + DateTime.Today);

                        _log.Info("Contractor Retrieved Data: " + DateTime.Today);
                        _log.Info("Adding Contractor expired data to object: " + DateTime.Today);

                        while (expiredContractorData.Read())
                        {
                            allExpiredContractorData.Add(

                                new Contractor
                                {
                                    PersonID = expiredContractorData.GetInt32(0),
                                    LastName = expiredContractorData[1].ToString(),
                                    Suffix = expiredContractorData[2].ToString(),
                                    FirstName = expiredContractorData[3].ToString(),
                                    MiddleName = expiredContractorData[4].ToString(),
                                    InvestigationDate = (DateTime)expiredContractorData[5],
                                    DaysToExpiration = expiredContractorData.GetInt32(6),
                                    gpoc_emails = expiredContractorData[7].ToString(),
                                    vpoc_emails = expiredContractorData[8].ToString(),
                                    RegionalEMails = expiredContractorData[9].ToString(),
                                    MajorEMails = expiredContractorData[10].ToString(),
                                   
                                });
                                   
                        }
                        _log.Info("Adding Contractor expired data to object: " + DateTime.Today);
                    }
                    return allExpiredContractorData;
                }
            }
            catch (Exception ex)
            {
                _log.Error("GetContractorRecord: " + " - " + ex.Message + " - " + ex.InnerException);
                return new List<Contractor>();
            }
        }

    }
}
 