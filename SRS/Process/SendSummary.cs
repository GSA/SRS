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

        public void SendSummaryEmail()
        {
            Email email = new Email();

            string subject = string.Empty;
            string body = string.Empty;
            string attachments = string.Empty;
            subject = ConfigurationManager.AppSettings["SUMMARYSUBJECT"].ToString() + " - " + DateTime.Now.ToString("MMMM dd, yyyy HH:mm:ss");

            body = GenerateEmailBody();
            attachments = SummaryAttachments();

            try
            {
                using (email)
                {
                    email.Send(ConfigurationManager.AppSettings["DEFAULTEMAIL"].ToString(),
                               ConfigurationManager.AppSettings["TO"].ToString(),
                               ConfigurationManager.AppSettings["CC"].ToString(),
                               ConfigurationManager.AppSettings["BCC"].ToString(),
                               subject, body, attachments.TrimEnd(';'), ConfigurationManager.AppSettings["SMTPSERVER"].ToString(), true);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error Sending Contractor Summary E-Mail: " + ex.Message + " - " + ex.InnerException);
            }
            finally
            {
                _log.Info("Contractor Summary E-Mail Sent");
            }
        }
        public string GenerateEmailBody()
        {
            StringBuilder fileNames = new StringBuilder();
            StringBuilder errors = new StringBuilder();
             
            string template = File.ReadAllText(ConfigurationManager.AppSettings["SUMMARYEMAILTEMPLATE"]);
             

            fileNames.Append(emailData.ContractorFilename == null ? "No Contractor File Found" : emailData.ContractorFilename.ToString());
            fileNames.Append(", ");


            template = template.Replace("[FILENAMES]", fileNames.ToString());
            template = template.Replace("[ACCESSINGDATE]", emailData.AccessingDate.ToString());
            template = template.Replace("[NUMBEROFRECORDS]", emailData.ExpiringContractorRecords.ToString());
            template = template.Replace("[TIMEBEGIN]", emailData.TimeBegin.ToString());
            template = template.Replace("[ENDTIME]", emailData.EndTime.ToString());
            template = template.Replace("[ACCESSINGTIME]", emailData.AccessingTime.ToString());


            if (emailData.ExpiringContractorHasErrors)
            {
                errors.Clear();

                errors.Append("<b><font color='red'>Errors were found while processing the Contractor file</font></b><br />");
                errors.Append("<br />Please see the attached file: <b><font color='red'>");
                errors.Append(emailData.ExpiringContractorUnsuccessfulFilename);
                errors.Append("</font></b>");

                template = template.Replace("[IfExpiringContractorHasERRORS]", errors.ToString());
            }
            else
            {
                template = template.Replace("[IfExpiringContractorHasERRORS]", null);
            }
           
            if (emailData.ExpiredContractorHasErrors)
            {
                errors.Clear();

                errors.Append("<b><font color='red'>Errors were found while processing the Contractor file</font></b><br />");
                errors.Append("<br />Please see the attached file: <b><font color='red'>");
                errors.Append(emailData.ExpiredContractorUnsuccessfulFilename);
                errors.Append("</font></b>");

                template = template.Replace("[IFExpiredContractorHasERRORS]", errors.ToString());
            }
            else
            {
                template = template.Replace("[IFExpiredContractorHasERRORS]", null);
            }
            return template;
        }

        private string SummaryAttachments()
        {
            StringBuilder attachments = new StringBuilder();

            //Contractor Summary Files
            if (emailData.ExpiringContractorSuccessfulFilename != null)
                attachments.Append(AddAttachment(emailData.ExpiringContractorSuccessfulFilename));

            if (emailData.ExpiringContractorUnsuccessfulFilename != null)
                attachments.Append(AddAttachment(emailData.ExpiringContractorUnsuccessfulFilename));

            if (emailData.ExpiredContractorSuccessfulFilename != null)
                attachments.Append(AddAttachment(emailData.ExpiredContractorSuccessfulFilename));

            if (emailData.ExpiredContractorUnsuccessfulFilename != null)
                attachments.Append(AddAttachment(emailData.ExpiredContractorUnsuccessfulFilename));

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
