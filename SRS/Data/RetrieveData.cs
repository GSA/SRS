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
        private IMapper dataMapper;
        private readonly IMapper retrieveMapper;

        public RetrieveData(IMapper mapper)
        {
            retrieveMapper = mapper;

            retrieveMapper.ConfigurationProvider.CompileMappings();
        }

        public List<Employee> AllGCIMSData()
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["GCIMS"].ToString());

                MySqlCommand cmd = new MySqlCommand();
                //MySqlConnection conn = new MySqlConnection("Server=[IP Address]; database=hspd; UID=[username]; Password=[password]");

                //conn.Open();
                ////Using Parameters
                //MySqlCommand cmd = new MySqlCommand("CORS_Expiring_Contracts", conn);

                List<Employee> allGCIMSData = new List<Employee>();

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

                            int remainingDays = (int)(DateTime.Today - expiryDate).TotalDays;
                            string labelCaption = String.Format("{0} day(s) left.", remainingDays);
                            //expiryDate = time.ToFileTimeUtc();

                            //storeExpiry(expiryDate.ToString());
                            //if (DateTime.Today > expiryDate)
                            if (remainingDays < 60)
                            {
                                string lableCaption = remainingDays + "days more to contract expired.";
                                //Contract Expired    
                                DateTime time = DateTime.Today.AddDays(60);
                                // expiryDate = time.ToFileTimeUtc();
                                //storeExpiry(expiryDate.ToString());
                                //useSeconds = false;

                            }
                            else if (remainingDays < 45)
                            {
                                string lableCaption = remainingDays + "days more to contract expired.";
                                DateTime time = DateTime.Today.AddDays(45);
                                //useSeconds = false;
                            }
                            //else if (DateTime.Today < expiryDate)
                            else if (remainingDays < 30)
                            {
                                string lableCaption = remainingDays + "days more to contract expired.";
                                DateTime time = DateTime.Today.AddDays(30);
                                //useSeconds = false;
                            }
                            else if (remainingDays < 15)
                            {
                                string lableCaption = remainingDays + "days more to contract expired.";
                                DateTime time = DateTime.Today.AddDays(15);
                                //useSeconds = false;
                            }
                            else if (remainingDays < 7)
                            {
                                string lableCaption = remainingDays + "days more to contract expired.";
                                DateTime time = DateTime.Today.AddDays(7);
                                //useSeconds = false;
                            }
                            //else if (DateTime.Today <= expiryDate)
                            else if (remainingDays <= 0)
                            {
                                string lableCaption = remainingDays + " Contract expired.";
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
                return new List<Employee>();
            }

        }

        private List<Employee> MapAllGCIMSData(MySqlDataReader gcmisData)
        {
            List<Employee> allRecords = new List<Employee>();

            while (gcmisData.Read())
            {
                Employee employee = new Employee();

                employee.Address = retrieveMapper.Map<IDataReader, Address>(gcmisData);
                employee.Building = retrieveMapper.Map<IDataReader, Building>(gcmisData);
                employee.Birth = retrieveMapper.Map<IDataReader, Birth>(gcmisData);
                employee.Person = retrieveMapper.Map<IDataReader, Person>(gcmisData);
                employee.Phone = retrieveMapper.Map<IDataReader, Phone>(gcmisData);
                employee.Position = retrieveMapper.Map<IDataReader, Position>(gcmisData); //Need to fix SupervisorID

                allRecords.Add(employee);
            }

            return allRecords;
        }
    }
}