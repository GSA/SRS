using AutoMapper;
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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //private readonly IMapper retrieveMapper;

        public RetrieveData()
        {
            //retrieveMapper = mapper;

           // retrieveMapper.ConfigurationProvider.CompileMappings();
        }

        public List<Contractor> ExpiringContractor(DateTime accessingDate)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["hspd"].ToString());

                MySqlCommand cmd = new MySqlCommand();
                //MySqlConnection conn = new MySqlConnection("Server=[IP Address]; database=hspd; UID=[username]; Password=[password]");

                //conn.Open();
                ////Using Parameters
                //MySqlCommand cmd = new MySqlCommand("SRS_GetContractors", conn);

                List<Contractor> allExpiringContractorData = new List<Contractor>();

                using (conn)
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
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
                        //                var myReader = cmd.ExecuteReader();
                        //                if (myReader.HasRows)
                        //                {
                        //                    //record found
                        //                    myReader.Read();
                        //                    var expiryDate = myReader.GetDateTime("expirationdate");

                        //                    int daysToExpiration = (int)(DateTime.Today - expiryDate).TotalDays;
                        //                    string labelCaption = String.Format("{0} day(s) left.", daysToExpiration);
                        //                    //expiryDate = time.ToFileTimeUtc();

                        //                    //else if (DateTime.Today < expiryDate)
                        //                    // else if (daysToExpiration <= 30)
                        //                    if (daysToExpiration <= 30)
                        //                    {
                        //                        string lableCaption = daysToExpiration + "days more to contract expired.";
                        //                        DateTime time = DateTime.Today.AddDays(30);
                        //                        //useSeconds = false;
                        //                    }
                        //                    else if (daysToExpiration <= 15)
                        //                    {
                        //                        string lableCaption = daysToExpiration + "days more to contract expired.";
                        //                        DateTime time = DateTime.Today.AddDays(15);
                        //                        //useSeconds = false;
                        //                    }
                        //                    else if (daysToExpiration <= 7)
                        //                    {
                        //                        string lableCaption = daysToExpiration + "days more to contract expired.";
                        //                        DateTime time = DateTime.Today.AddDays(7);
                        //                        //useSeconds = false;
                        //                    }

                        //                }

                        //                using (contractorData)
                        //                {
                        //                    if (contractorData.HasRows)
                        //                    {
                        //                        allExpiringContractorData = MapAllContractorData(contractorData);
                        //                    }
                        //                }
                        //            }
                        //        }

                        //        return allExpiringContractorData;
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        log.Error("GetContractorRecord: " + " - " + ex.Message + " - " + ex.InnerException);
                        //        return new List<Contractor>();
                        //    }
                        //}
                        //     private List<Contractor> MapAllContractorData(MySqlDataReader contractorData)
                        //{
                        // List<Contractor> allRecords = new List<Contractor>();

                        log.Info("Contractor Retrieved Data: " + DateTime.Now);
                        log.Info("Adding Contractor expiring data to object: " + DateTime.Now);

                        while (contractorData.Read())
                        {
                            allExpiringContractorData.Add(
                                new Contractor
                                {
                                    //Contractor allExpiringContractor = new Contractor();
                                    PersID = contractorData[0].ToString(),
                                    LastName = contractorData[1].ToString(),
                                    FirstName = contractorData[2].ToString(),
                                    MiddleName = contractorData[3].ToString(),
                                    Suffix = contractorData[4].ToString(),
                                    //pers_investigation_date = (DateTime)contractorData[5],
                                    DaysToExpiration = contractorData.GetInt32(5),
                                    RegionalEmail = contractorData[6].ToString(),
                                    pers_work_email = contractorData[7].ToString(),
                                    pers_status = contractorData[9].ToString(),
                            //allExpiringContractor.conpoc_email = contractorData[9].ToString()
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
                        MySqlDataReader contractorData;

                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "SRS_GetContractors";
                        cmd.Parameters.Clear();

                        MySqlDbType todaysDate = default(MySqlDbType);
                        cmd.Parameters.Add("DateTime", todaysDate);

                        log.Info("Contractor data of expiration: " + DateTime.Now);

                        contractorData = cmd.ExecuteReader();
                        //        var myReader = cmd.ExecuteReader();
                        //        if (myReader.HasRows)
                        //        {
                        //            //record found
                        //            myReader.Read();
                        //            var expiryDate = myReader.GetDateTime("expirationdate");

                        //            int daysToExpiration = (int)(DateTime.Today - expiryDate).TotalDays;
                        //            string labelCaption = String.Format("{0} day(s) left.", daysToExpiration);
                        //            // if (DateTime.Today <= expiryDate)
                        //            if (daysToExpiration <= 0)
                        //            {

                        //                string lableCaption = daysToExpiration + " Contract expired.";
                        //                DateTime time = DateTime.Today.AddDays(0);
                        //            }
                        //        }
                        //    }


                        log.Info("Contractor Retrieved Data: " + DateTime.Now);
                        log.Info("Adding Contractor expiring data to object: " + DateTime.Now);
                        while (contractorData.Read())
                        {
                            allExpiredContractorData.Add(
                                           new Contractor
                                           {
                                   //Contractor allExpiringContractor = new Contractor();
                                   PersID = contractorData[0].ToString(),
                                               LastName = contractorData[1].ToString(),
                                               FirstName = contractorData[2].ToString(),
                                               MiddleName = contractorData[3].ToString(),
                                               Suffix = contractorData[4].ToString(),
                                               //pers_investigation_date = (DateTime)contractorData[5],
                                               DaysToExpiration = contractorData.GetInt32(5),
                                               RegionalEmail = contractorData[6].ToString(),
                                               pers_work_email = contractorData[7].ToString(),
                                               pers_status = contractorData[9].ToString(),
                                               //allExpiringContractor.conpoc_email = contractorData[9].ToString()
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
 