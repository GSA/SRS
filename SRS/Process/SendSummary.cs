using SRS.Utilities;
using SRS.Models;
using System;
using System.Configuration;
using System.IO;
using System.Text;

namespace SRS.Process
{
    internal class SendSummary
    {
        //Reference to logger
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly EmailData emailData = new EmailData();

      //  static readonly string SummaryFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Summary");
       
        public SendSummary(ref EmailData emailData)
        {
            this.emailData = emailData;
        }


        public void SummaryEmailContent()
        {
            //Basic email class reference
            Email email = new Email();

            //variable declaration 
            string subject = string.Empty;
            string body = string.Empty;
            string attachments = string.Empty;
             
            
            subject = ConfigurationManager.AppSettings["SummarySubject"].ToString() + " - " + DateTime.Now.ToString("MMMM dd, yyyy HH:mm:ss");

            //Generate an email 
            body = GenerateTemplate();

            attachments = SummaryAttachments();

            //body = GenerateExpiringSAC();

            //body = GenerateExpiredSAC();

            try
            {
                using (email)
                {
                    email.Send(ConfigurationManager.AppSettings["DefaultEmail"].ToString(),
                               ConfigurationManager.AppSettings["To"].ToString(),
                               ConfigurationManager.AppSettings["Cc"].ToString(),
                               ConfigurationManager.AppSettings["Bcc"].ToString(),
                               subject, body, attachments.TrimEnd(';'), ConfigurationManager.AppSettings["SMTPServer"].ToString(), true);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error Sending Contractor Summary Email: " + ex.Message + " - " + ex.InnerException);
            }
            finally
            {
                log.Info("Contractor Summary Email Sent");
            }
            
        }
      
        public string GenerateTemplate()
        {
            StringBuilder errors = new StringBuilder();
            StringBuilder fileNames = new StringBuilder();

            {
                //string template = File.ReadAllText(@ConfigurationManager.AppSettings["Summary"] + "Summary.html");
                string template = @ConfigurationManager.AppSettings["Summary" + "Summary.html"];

                //replacing the parameters
                try
                {
                    string SummaryFilePath = AppDomain.CurrentDomain.BaseDirectory + "Summary.html";
                    using (StreamReader reader = new StreamReader(SummaryFilePath))
                    //using (StreamReader reader = new StreamReader("Summary.html"))

                    {
                        template = reader.ReadToEnd();
                    }
                    template = template.Replace("[ACCESSINGDATE]", emailData.AccessingDate.ToString());
                    template = template.Replace("[NUMBEROFRECORDS]", emailData.TotalContractorsProcessed.ToString());
                    template = template.Replace("[TIMEBEGIN]", emailData.TimeBegin.ToString());
                    template = template.Replace("[ENDTIME]", emailData.EndTime.ToString());
                    template = template.Replace("[ACCESSINGTIME]", emailData.TimeElapsed.ToString());

                }
                catch (Exception ex)
                {
                    log.Error("Error Sending Contractor Summary Email: " + ex.Message + " - " + ex.InnerException);
                }
                finally
                {
                    log.Info("Contractor Summary Email Sent");
                }
                errors.Clear();
                errors.Append("<b><font color='red'>Errors were found while processing the Contractor file</font></b><br />");
                errors.Append("<br />Please see the attached file: <b><font color='red'>");
                return template.ToString(); 
            }
        }
        private string SummaryAttachments()
        {
            StringBuilder attachments = new StringBuilder();

            //Contractor Summary Files
            if (emailData.ExpiringContractorSuccessfulFileName != null)
                attachments.Append(AddAttachment(emailData.ExpiringContractorSuccessfulFileName));

            if (emailData.ExpiringContractorUnsuccessfulFileName != null)
                attachments.Append(AddAttachment(emailData.ExpiringContractorUnsuccessfulFileName));

            if (emailData.ExpiredContractorSuccessfulFileName != null)
                attachments.Append(AddAttachment(emailData.ExpiredContractorSuccessfulFileName));

            if (emailData.ExpiredContractorUnsuccessfulFileName != null)
                attachments.Append(AddAttachment(emailData.ExpiredContractorUnsuccessfulFileName));

            return attachments.ToString();
        }

        private string AddAttachment(string fileName)
        {
            StringBuilder addAttachment = new StringBuilder();

            addAttachment.Append(ConfigurationManager.AppSettings["SummaryFilePath"]);
            addAttachment.Append(fileName);
            addAttachment.Append(";");

            return addAttachment.ToString();
        }
        private string GenerateExpiringSAC()
        {

            StringBuilder errors = new StringBuilder();
            StringBuilder expiringSAC = new StringBuilder();

            // string template = File.ReadAllText(@ConfigurationManager.AppSettings["ExpiringSACEmailTemplate"] + "ExpiringSAC.html");
            string template = @ConfigurationManager.AppSettings["ExpiringSACEmailTemplate"] + "ExpiringSAC.html";
            expiringSAC.Append(emailData.ExpiringContractorSummary == null ? "No Contractor File Found" : emailData.ExpiringContractorSummary.ToString());
            expiringSAC.Append(", ");

            try
            {
                string ExpiringSACFilePath = AppDomain.CurrentDomain.BaseDirectory + "ExpiringSAC.html";
                using (StreamReader reader = new StreamReader(ExpiringSACFilePath))
                //using (StreamReader reader = new StreamReader("ExpiringSAC.html"))
                {
                    template = reader.ReadToEnd();
                }

                object lastName = null;
                template = template.Replace("[LastName]", lastName.ToString());

            }
            catch (Exception ex)
            {
                log.Error("Error Sending Contractor Summary Email: " + ex.Message + " - " + ex.InnerException);
            }
            finally
            {
                log.Info("Contractor Summary Email Sent");
            }
            if (emailData.ExpiringContractorHasErrors)
            {
                errors.Clear();

                errors.Append("<b><font color='red'>Errors were found while processing the Contractor file</font></b><br />");
                errors.Append("<br />Please see the attached file: <b><font color='red'>");
                errors.Append(emailData.ExpiringContractorUnsuccessfulFileName);
                errors.Append("</font></b>");

                expiringSAC = expiringSAC.Replace("[IfExpiringContractorHasERRORS]", errors.ToString());
            }
            else
            {
                expiringSAC = expiringSAC.Replace("[IfExpiringContractorHasERRORS]", null);
            }
            return template.ToString();
        }
        private string GenerateExpiredSAC()
        {
            StringBuilder errors = new StringBuilder();
            StringBuilder expiredSAC = new StringBuilder();

           // string template = File.ReadAllText(@ConfigurationManager.AppSettings["ExpiredSACEmailTemplate"] + "ExpiredSAC.html");
            string template = @ConfigurationManager.AppSettings["ExpiredSACEmailTemplate"] + "ExpiredSAC.html";
            expiredSAC.Append(emailData.ExpiredContractorSummary == null ? "No Contractor File Found" : emailData.ExpiredContractorSummary.ToString());
            expiredSAC.Append(", ");

            try
            {
                string ExpiredSACFilePath = AppDomain.CurrentDomain.BaseDirectory + "ExpiredSAC.html";
                using (StreamReader reader = new StreamReader(ExpiredSACFilePath))
                //using (StreamReader reader = new StreamReader("ExpiredSAC.html"))
                {
                   template = reader.ReadToEnd();
                }

                object lastName = null;
                template = template.Replace("[LastName]", lastName.ToString());

            }
            catch (Exception ex)
            {
                log.Error("Error Sending Contractor Summary Email: " + ex.Message + " - " + ex.InnerException);
            }
            finally
            {
                log.Info("Contractor Summary Email Sent");
            }

            if (emailData.ExpiredContractorHasErrors)
            {
                errors.Clear();

                errors.Append("<b><font color='red'>Errors were found while processing the Contractor file</font></b><br />");
                errors.Append("<br />Please see the attached file: <b><font color='red'>");
                errors.Append(emailData.ExpiredContractorUnsuccessfulFileName);
                errors.Append("</font></b>");

                expiredSAC = expiredSAC.Replace("[IFExpiredContractorHasERRORS]", errors.ToString());
            }
            else
            {
                expiredSAC = expiredSAC.Replace("[IfExpiredContractorHasERRORS]", null);
            }
            return template.ToString();
        }
        //public void GetExpiringContractor()
        //{

        //    MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["hspd"].ToString());

        //    //try
        //    //{
        //    //    _log.Info("Getting Contractor Data");
        //    //    // MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["hspd"].ToString());

        //        MySqlCommand cmd = new MySqlCommand();

        //        List<Contractor> allExpiringContractorData = new List<Contractor>();

        //        using (conn)
        //        {
        //            if (conn.State == ConnectionState.Closed)
        //                conn.Open();

        //            using (cmd)
        //            {
        //                MySqlDataReader expiringContractorData;

        //                cmd.Connection = conn;
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.CommandText = "SRS_GetContractors";
        //                cmd.Parameters.Clear();

        //                //MySqlDbType todaysDate = default(MySqlDbType);
        //                //cmd.Parameters.Add("DateTime", todaysDate);
        //                cmd.Parameters.AddWithValue("inDate", "2020-06-06");// accessingDate); //"2020-06-06"
        //                _log.Info("Contractor data of expiration: " + DateTime.Now);
        //                expiringContractorData = cmd.ExecuteReader();
        //                _log.Info("Contractor Retrieved Data: " + DateTime.Now);
        //                _log.Info("Adding Contractor expiring data to object: " + DateTime.Now);

        //                while (expiringContractorData.Read())
        //                {
        //                    allExpiringContractorData.Add(
        //                        new Contractor
        //                        {
        //                            Pers_id = expiringContractorData[0].ToString(),
        //                            LastName = expiringContractorData[1].ToString(),
        //                            FirstName = expiringContractorData[2].ToString(),
        //                            MiddleName = expiringContractorData[3].ToString(),
        //                            Suffix = expiringContractorData[4].ToString(),
        //                            DaysToExpiration = expiringContractorData.GetInt32(5),
        //                            vpoc_emails = expiringContractorData[6].ToString(),
        //                            gpoc_emails = expiringContractorData[7].ToString(),
        //                            pers_status = expiringContractorData[8].ToString(),
        //                            pers_investigation_date = (DateTime)expiringContractorData[9]

        //                        }
        //                       );
        //                }
        //                _log.Info("Adding Contractor expired data to object: " + DateTime.Now);
        //            }
        //           // return allExpiringContractorData;
        //        }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    _log.Error("GetContractorRecord: " + " - " + ex.Message + " - " + ex.InnerException);
        //    //    return new List<Contractor>();
        //    //}

        //}

        //public List<Contractor> allExpiredContractorData(DateTime accessingDate)
        //{
        //    MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["hspd"].ToString());
        //    try
        //    {
        //        // MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["hspd"].ToString());

        //        MySqlCommand cmd = new MySqlCommand();
        //        List<Contractor> allExpiredContractorData = new List<Contractor>();

        //        using (conn)
        //        {
        //            if (conn.State == ConnectionState.Closed)
        //                conn.Open();

        //            using (cmd)
        //            {
        //                MySqlDataReader expiredContractorData;


        //                cmd.Connection = conn;
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.CommandText = "SRS_GetContractors";
        //                cmd.Parameters.Clear();

        //                //MySqlDbType todaysDate = default(MySqlDbType);
        //                //cmd.Parameters.Add("DateTime", todaysDate);
        //                cmd.Parameters.AddWithValue("inDate", "2020-06-06");// accessingDate); //"2020-06-06"
        //                _log.Info("Contractor data of expiration: " + DateTime.Now);
        //                expiredContractorData = cmd.ExecuteReader();
        //                _log.Info("Contractor Retrieved Data: " + DateTime.Now);
        //                _log.Info("Adding Contractor expiring data to object: " + DateTime.Now);

        //                while (expiredContractorData.Read())
        //                {
        //                    allExpiredContractorData.Add(

        //                        new Contractor
        //                        {
        //                            Pers_id = expiredContractorData[0].ToString(),
        //                            LastName = expiredContractorData[1].ToString(),
        //                            FirstName = expiredContractorData[2].ToString(),
        //                            MiddleName = expiredContractorData[3].ToString(),
        //                            Suffix = expiredContractorData[4].ToString(),
        //                            DaysToExpiration = expiredContractorData.GetInt32(5),
        //                            vpoc_emails = expiredContractorData[6].ToString(),
        //                            gpoc_emails = expiredContractorData[7].ToString(),
        //                            pers_status = expiredContractorData[8].ToString(),
        //                            pers_investigation_date = (DateTime)expiredContractorData[9]

        //                        }
        //                            );
        //                }
        //                _log.Info("Adding Contractor expired data to object: " + DateTime.Now);
        //            }
        //            return allExpiredContractorData;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.Error("GetContractorRecord: " + " - " + ex.Message + " - " + ex.InnerException);
        //        return new List<Contractor>();
        //    }
        //}
    }
}
