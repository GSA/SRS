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
            Email email = new Email();

            string subject = string.Empty;
            string body = string.Empty;
            string attachments = string.Empty;
            //string fileNames = string.Empty;
           
                subject = ConfigurationManager.AppSettings["SummarySubject"].ToString() + " - " + DateTime.Now.ToString("MMMM dd, yyyy HH:mm:ss");
            
            body = GenerateEmailBody();

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

        public string GenerateEmailBody()
        {

            StringBuilder errors = new StringBuilder();
            StringBuilder fileNames = new StringBuilder();

            //GenerateEmailBody message = new GenerateEmailBody();
            //message.Body = fileNames.ToString();

            string template = File.ReadAllText(ConfigurationManager.AppSettings["Summary"].ToString());

            fileNames.Append(emailData.ExpiringContractorSummary == null ? "No Contractor File Found" : emailData.ExpiringContractorSummary.ToString());
            fileNames.Append(", ");
            fileNames.Append(emailData.ExpiredContractorSummary == null ? "No Contractor File Found" : emailData.ExpiredContractorSummary.ToString());
            fileNames.Append(", ");
            //replacing the parameters
            //using (StreamReader reader = new StreamReader("Summary.html"))
            //{
            template = template.Replace("[FileName]", fileNames.ToString());
            template = template.Replace("[AccessingDate]", emailData.AccessingDate.ToString());
            template = template.Replace("[NumberOfRecords]", emailData.ExpiringContractorRecords.ToString());
            template = template.Replace("[TimeBegin]", emailData.TimeBegin.ToString());
            template = template.Replace("[EndTime]", emailData.EndTime.ToString());
            template = template.Replace("[AccessingTime", emailData.AccessingTime.ToString());

       // }
             
            if (emailData.ExpiringContractorHasErrors)
            {
                errors.Clear();

                errors.Append("<b><font color='red'>Errors were found while processing the Contractor file</font></b><br />");
                errors.Append("<br />Please see the attached file: <b><font color='red'>");
                errors.Append(emailData.ExpiringContractorUnsuccessfulFileName);
                errors.Append("</font></b>");

                template = template.Replace("[IfExpiringContractorHasERRORS]", errors.ToString());
            }
            else
            {
                template = template .Replace("[IfExpiringContractorHasERRORS]", null);
            }

            if (emailData.ExpiredContractorHasErrors)
            {
                errors.Clear();

                errors.Append("<b><font color='red'>Errors were found while processing the Contractor file</font></b><br />");
                errors.Append("<br />Please see the attached file: <b><font color='red'>");
                errors.Append(emailData.ExpiredContractorUnsuccessfulFileName);
                errors.Append("</font></b>");

                template = template.Replace("[IFExpiredContractorHasERRORS]", errors.ToString());
            }
            else
            {
                template = template.Replace("[IfExpiredContractorHasERRORS]", null);
            }
            return template.ToString();
             
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
    }
}
