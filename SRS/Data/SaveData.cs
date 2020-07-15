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
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //Set up connection
        private readonly MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["hspd"].ToString());

        private readonly MySqlCommand cmd = new MySqlCommand(); 
    }
}

