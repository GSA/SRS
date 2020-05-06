using EASendMail;
using SRS.Data;
using SRS.Lookups;
using SRS.Utilities;
using SRS.Mapping;
using SRS.Models;
using SRS.Validation;
using MySql.Data.MySqlClient;
using System;
using log4net;
using System.Configuration;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            StringBuilder errors = new StringBuilder();
            StringBuilder fileNames = new StringBuilder();

            string template = File.ReadAllText(ConfigurationManager.AppSettings["SUMMARYTEMPLATE"]);

            //fileNames.Append(emailData.ContractorFilename == null ? "No Contractor File Found" : emailData.ContractorFilename.ToString());
            fileNames.Append(", ");


            template = template.Replace("[FILENAMES]", fileNames.ToString());
            template = template.Replace("[ACCESSINGDATE]", emailData.AccessingDate.ToString());
            template = template.Replace("[NUMBEROFRECORDS]", emailData.ContractExpiringRecords.ToString());
            template = template.Replace("[TIMEBEGIN]", emailData.TimeBegin.ToString());
            template = template.Replace("[ENDTIME]", emailData.EndTime.ToString());
            template = template.Replace("[ACCESSINGTIME]", emailData.AccessingTime.ToString());


            if (emailData.ContractExpiringHasErrors)
            {
                errors.Clear();

                errors.Append("<b><font color='red'>Errors were found while processing the Contractor file</font></b><br />");
                errors.Append("<br />Please see the attached file: <b><font color='red'>");
                errors.Append(emailData.ContractExpiringUnsuccessfulFilename);
                errors.Append("</font></b>");

                template = template.Replace("[IFContractExpiringHasERRORS]", errors.ToString());
            }
            else
            {
                template = template.Replace("[IFContractExpiringHasERRORS]", null);
            }
           
            if (emailData.ContractExpiredHasErrors)
            {
                errors.Clear();

                errors.Append("<b><font color='red'>Errors were found while processing the Contractor file</font></b><br />");
                errors.Append("<br />Please see the attached file: <b><font color='red'>");
                errors.Append(emailData.ContractExpiredUnsuccessfulFilename);
                errors.Append("</font></b>");

                template = template.Replace("[IFContractExpiredHasERRORS]", errors.ToString());
            }
            else
            {
                template = template.Replace("[IFContractExpiredHasERRORS]", null);
            }
            return template;
        }

        private string SummaryAttachments()
        {
            StringBuilder attachments = new StringBuilder();

            //Contractor Summary Files
            if (emailData.ContractExpiringSuccessfulFilename != null)
                attachments.Append(AddAttachment(emailData.ContractExpiringSuccessfulFilename));

            if (emailData.ContractExpiringUnsuccessfulFilename != null)
                attachments.Append(AddAttachment(emailData.ContractExpiringUnsuccessfulFilename));

            if (emailData.ContractExpiredSuccessfulFilename != null)
                attachments.Append(AddAttachment(emailData.ContractExpiredSuccessfulFilename));

            if (emailData.ContractExpiredUnsuccessfulFilename != null)
                attachments.Append(AddAttachment(emailData.ContractExpiredUnsuccessfulFilename));

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
