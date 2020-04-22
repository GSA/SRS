using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using SRS.Models;
using System.Text;
using System.Threading.Tasks;

namespace SRS.Data
{
    internal class SaveData
    {
        //Reference to logger
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //Set up connection
        private readonly MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["GCIMS"].ToString());

        private readonly MySqlCommand cmd = new MySqlCommand();

        public SaveData()
        {

        }
        public string InsertGCIMSID(Int64 persID, string GCIMSID)
        {
            try
            {
                using (conn)
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    using (cmd)
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "CORS_Expiring_Contracts";

                        cmd.Parameters.Clear();

                        MySqlParameter[] personParameters = new MySqlParameter[]
                        {
                            new MySqlParameter { ParameterName = "persID", Value = persID, MySqlDbType = MySqlDbType.Int64},
                            new MySqlParameter { ParameterName = "GCIMSID", Value = GCIMSID, MySqlDbType = MySqlDbType.VarChar, Size = 11},
                            new MySqlParameter { ParameterName = "SQLExceptionWarning", MySqlDbType=MySqlDbType.VarChar, Size=4000, Direction = ParameterDirection.Output },
                        };

                        cmd.Parameters.AddRange(personParameters);

                        cmd.ExecuteNonQuery();

                        return cmd.Parameters["SQLExceptionWarning"].Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Updating GCIMSID: " + ex.Message + " - " + ex.InnerException);
                return ex.Message.ToString();
            }
        }

        internal void InsertGCIMSID(long gCIMSID, Person person)
        {
            throw new NotImplementedException();
        }

        internal void InsertGCIMSID(long gCIMSID1, long gCIMSID2)
        {
            throw new NotImplementedException();
        }
    }
}

