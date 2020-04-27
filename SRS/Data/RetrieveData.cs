﻿using AutoMapper;
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
        private IMapper dataMapper;
        private readonly IMapper retrieveMapper;

        public RetrieveData(IMapper mapper)
        {
            retrieveMapper = mapper;

            retrieveMapper.ConfigurationProvider.CompileMappings();
        }

        public List<Contractor> AllGCIMSData()
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["GCIMS"].ToString());

                MySqlCommand cmd = new MySqlCommand();
                //MySqlConnection conn = new MySqlConnection("Server=[IP Address]; database=hspd; UID=[username]; Password=[password]");

                //conn.Open();
                ////Using Parameters
                //MySqlCommand cmd = new MySqlCommand("CORS_Expiring_Contracts", conn);

                List<Contractor> allGCIMSData = new List<Contractor>();

                using (conn)
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    using (cmd)
                    {
                        MySqlDataReader gcimsData;

                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "CORS_Expiring_Contracts";
                        cmd.Parameters.Clear();
                        MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        sda.Fill(ds);

                        gcimsData = cmd.ExecuteReader();
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
                            if (daysUntilExpired < 60)
                            {
                                string lableCaption = daysUntilExpired + "days more to contract expired.";
                                //Contract Expired    
                                DateTime time = DateTime.Today.AddDays(60);
                                // expiryDate = time.ToFileTimeUtc();
                                //storeExpiry(expiryDate.ToString());
                                //useSeconds = false;

                            }
                            //else if (daysUntilExpired < 45)
                            //{
                            //    string lableCaption = daysUntilExpired + "days more to contract expired.";
                            //    DateTime time = DateTime.Today.AddDays(45);
                            //   // useSeconds = false;
                            //}
                            //else if (DateTime.Today < expiryDate)
                            else if (daysUntilExpired < 30)
                            {
                                string lableCaption = daysUntilExpired + "days more to contract expired.";
                                DateTime time = DateTime.Today.AddDays(30);
                                //useSeconds = false;
                            }
                            //else if (daysUntilExpired < 15)
                            //{
                            //    string lableCaption = daysUntilExpired + "days more to contract expired.";
                            //    DateTime time = DateTime.Today.AddDays(15);
                            //    //useSeconds = false;
                            //}
                            //else if (daysUntilExpired < 7)
                            //{
                            //    string lableCaption = daysUntilExpired + "days more to contract expired.";
                            //    DateTime time = DateTime.Today.AddDays(7);
                            //    //useSeconds = false;
                            //}
                            //else if (DateTime.Today <= expiryDate)
                            else if (daysUntilExpired <= 0)
                            {
                                string lableCaption = daysUntilExpired + " Contract expired.";
                                DateTime time = DateTime.Today.AddDays(0);
                            }
                        }

                        using (gcimsData)
                        {
                            if (gcimsData.HasRows)
                            {
                                allGCIMSData = MapAllGCIMSData(gcimsData);
                            }
                        }
                    }
                }

                return allGCIMSData;
            }
            catch (Exception ex)
            {
                log.Error("GetGCIMSRecord: " + " - " + ex.Message + " - " + ex.InnerException);
                return new List<Contractor>();
            }

        }

        private List<Contractor> MapAllGCIMSData(MySqlDataReader gcimsData)
        {
            List<Contractor> allRecords = new List<Contractor>();

            while (gcimsData.Read())
            {
                Contractor contractor = new Contractor();

                contractor.Address = retrieveMapper.Map<IDataReader, Address>(gcimsData);
                contractor.Building = retrieveMapper.Map<IDataReader, Building>(gcimsData);
                contractor.Birth = retrieveMapper.Map<IDataReader, Birth>(gcimsData);
                contractor.Person = retrieveMapper.Map<IDataReader, Person>(gcimsData);
                contractor.Phone = retrieveMapper.Map<IDataReader, Phone>(gcimsData);
                contractor.Position = retrieveMapper.Map<IDataReader, Position>(gcimsData); //Need to fix SupervisorID

                allRecords.Add(contractor);
            }

            return allRecords;
        }
    }
}