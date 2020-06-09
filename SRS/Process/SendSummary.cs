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
        private static log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly EmailData emailData = new EmailData();

        static readonly string SummaryFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Summary");
       
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

            body = GenerateExpiringSAC();

            body = GenerateExpiredSAC();

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
                _log.Error("Error Sending Contractor Summary Email: " + ex.Message + " - " + ex.InnerException);
            }
            finally
            {
                _log.Info("Contractor Summary Email Sent");
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
                    _log.Error("Error Sending Contractor Summary Email: " + ex.Message + " - " + ex.InnerException);
                }
                finally
                {
                    _log.Info("Contractor Summary Email Sent");
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
                _log.Error("Error Sending Contractor Summary Email: " + ex.Message + " - " + ex.InnerException);
            }
            finally
            {
                _log.Info("Contractor Summary Email Sent");
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
                _log.Error("Error Sending Contractor Summary Email: " + ex.Message + " - " + ex.InnerException);
            }
            finally
            {
                _log.Info("Contractor Summary Email Sent");
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
    
    }
}
