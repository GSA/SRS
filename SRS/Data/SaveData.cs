using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using SRS.Models;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;

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
        public string InsertGSAPOC(Int64 PersID, string @Sponsor, Int16 RoleTypeId, string ContractID)
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
                        cmd.CommandText = "CIW_InsertGSAPOC";

                        cmd.Parameters.Clear();

                        MySqlParameter[] personParameters = new MySqlParameter[]
                        {
                            new MySqlParameter { ParameterName = "PersID", Value = PersID, MySqlDbType = MySqlDbType.Int64},
                            new MySqlParameter { ParameterName = "SponsorEmail", Value = @Sponsor, MySqlDbType = MySqlDbType.VarChar, Size = 64},
                            new MySqlParameter { ParameterName = "RoleTypeID", Value = RoleTypeId, MySqlDbType = MySqlDbType.Int16, Size = 3},
                            new MySqlParameter { ParameterName = "ContractID", Value = ContractID, MySqlDbType = MySqlDbType.Int16, Size = 10},
                            new MySqlParameter { ParameterName = "Result", MySqlDbType = MySqlDbType.Int16, Direction = ParameterDirection.Output},
                            new MySqlParameter { ParameterName = "SQLExceptionWarning", MySqlDbType = MySqlDbType.VarChar, Size=4000, Direction = ParameterDirection.Output },
                        };

                        cmd.Parameters.AddRange(personParameters);

                        cmd.ExecuteNonQuery();

                        return cmd.Parameters["SQLExceptionWarning"].Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Updating contractID: " + ex.Message + " - " + ex.InnerException);
                return ex.Message.ToString();
            }
        }

        internal void InsertGSAPOC(long persID1, long persID2)
        {
            throw new NotImplementedException();
        }
    }
}

