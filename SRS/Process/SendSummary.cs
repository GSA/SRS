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
            StringBuilder fileNames = new StringBuilder();
            StringBuilder errors = new StringBuilder(); 
            {
            string template = File.ReadAllText(ConfigurationManager.AppSettings["Summary"]);// + "Summary.html");
                fileNames.Append(emailData.ExpiringContractorFileName == null ? "Expiring contractor file is not found" : emailData.ExpiringContractorFileName.ToString());
                fileNames.Append(",");
                fileNames.Append(emailData.ExpiredContractorFileName == null ? "Expired contractor file is not found" : emailData.ExpiredContractorFileName.ToString());
                fileNames.Append(",");
 
                //replacing the parameters
                try
            {

                using (StreamReader reader = new StreamReader("Summary.html"))

            {
                template = reader.ReadToEnd();
            }
                    template = template.Replace("[FILENAMES]", fileNames.ToString());
                    template = template.Replace("[ACCESSINGDATE]", emailData.ACCESSINGDATE.ToString());
                    template = template.Replace("[ExpiringNUMBEROFRECORDS]", emailData.ExpiringContractorRecords.ToString());
                    template = template.Replace("[ExpiredNUMBEROFRECORDS]", emailData.ExpiredContractorRecords.ToString());
                    template = template.Replace("[TIMEBEGIN]", emailData.TIMEBEGIN.ToString());
                    template = template.Replace("[ENDTIME]", emailData.ENDTIME.ToString());
                    template = template.Replace("[ACCESSINGTIME]", emailData.ACCESSINGTIME.ToString()); 
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
                return template;//.ToString(); 
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

            addAttachment.Append(ConfigurationManager.AppSettings["SUMMARYFILEPATH"]);
            addAttachment.Append(fileName);
            addAttachment.Append(";");

            return addAttachment.ToString();
        }
         
    }
}
