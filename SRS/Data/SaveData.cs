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
        private readonly MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["hspd"].ToString());

        private readonly MySqlCommand cmd = new MySqlCommand();

        public SaveData()
        {

        }
        public string InsertGSAPOC(Int64 PersID, string contract_POC_Email, string RegionalEmail, Int16 RoleTypeId, string LastName, string FirstName)
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
                        //cmd.CommandType = CommandType.StoredProcedure;
                       // cmd.CommandText = "SRS_InsertGSAPOC"; //this sp may be changed to a new one

                        cmd.Parameters.Clear();

                        MySqlParameter[] personParameters = new MySqlParameter[]
                        {
                            new MySqlParameter { ParameterName = "PersID", Value = PersID, MySqlDbType = MySqlDbType.Int64},
                            new MySqlParameter { ParameterName = "FirstName", Value = FirstName, MySqlDbType = MySqlDbType.String , Size = 20},
                            new MySqlParameter { ParameterName = "LastName", Value = LastName, MySqlDbType = MySqlDbType.String , Size = 20},
                       
                            new MySqlParameter { ParameterName = "contract_POC_Email", Value = contract_POC_Email, MySqlDbType = MySqlDbType.VarChar, Size = 64},
                            new MySqlParameter { ParameterName = "RegionalEmail", Value = RegionalEmail, MySqlDbType = MySqlDbType.VarChar, Size = 64},
                            //new MySqlParameter { ParameterName = "contract_vender_ID", Value = ContractVenderID, MySqlDbType = MySqlDbType.Int32, Size = 10},
                            //new MySqlParameter { ParameterName = "Contract_number", Value = ContractNumber, MySqlDbType = MySqlDbType.Int32, Size = 10},
                            //new MySqlParameter { ParameterName = "Contract_name", Value = ContractName, MySqlDbType = MySqlDbType.Int32, Size = 10},
                            //new MySqlParameter { ParameterName = "DaysUntilExpired", Value = DaysUntilExpired, MySqlDbType = MySqlDbType.Int32, Size = 10},
                            //new MySqlParameter { ParameterName = "Contract_date_end", Value = Contract_date_end, MySqlDbType = MySqlDbType.DateTime, Size = 10},
                            new MySqlParameter { ParameterName = "Result", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output},
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
                log.Error("Updating PersID: " + ex.Message + " - " + ex.InnerException);
                return ex.Message.ToString();
            }
        }

        internal void InsertGSAPOC(long persID1, long persID2)
        {
            throw new NotImplementedException();
        }
    }
}

