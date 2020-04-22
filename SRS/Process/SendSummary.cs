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
            subject = ConfigurationManager.AppSettings["EMAILSUBJECT"].ToString() + " - " + DateTime.Now.ToString("MMMM dd, yyyy HH:mm:ss");

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
                _log.Error("Error Sending Monster Summary E-Mail: " + ex.Message + " - " + ex.InnerException);
            }
            finally
            {
                _log.Info("Monster Summary E-Mail Sent");
            }
        }
        public string GenerateEmailBody()
        {
            StringBuilder errors = new StringBuilder();
            StringBuilder fileNames = new StringBuilder();

            string template = File.ReadAllText(ConfigurationManager.AppSettings["SUMMARYTEMPLATE"]);

            fileNames.Append(emailData.MonsterFilename == null ? "No HR Links File Found" : emailData.MonsterFilename.ToString());
            fileNames.Append(", ");


            template = template.Replace("[FILENAMES]", fileNames.ToString());

            template = template.Replace("[MonsterATTEMPTED]", emailData.MonsterAttempted.ToString());
            template = template.Replace("[MonsterSUCCEEDED]", emailData.MonsterSucceeded.ToString());
            template = template.Replace("[MonsterIDENTICAL]", emailData.MonsterIdentical.ToString());
            template = template.Replace("[MonsterINACTIVE]", emailData.MonsterInactive.ToString());
            template = template.Replace("[MonsterRECORDSNOTFOUND]", emailData.MonsterRecordsNotFound.ToString());
            template = template.Replace("[MonsterFAILED]", emailData.MonsterFailed.ToString());

            if (emailData.MonsterHasErrors)
            {
                errors.Clear();

                errors.Append("<b><font color='red'>Errors were found while processing the Monster file</font></b><br />");
                errors.Append("<br />Please see the attached file: <b><font color='red'>");
                errors.Append(emailData.MonsterUnsuccessfulFilename);
                errors.Append("</font></b>");

                template = template.Replace("[IFMonsterERRORS]", errors.ToString());
            }
            else
            {
                template = template.Replace("[IFMonsterERRORS]", null);
            }
            return template;
        }

        private string SummaryAttachments()
        {
            StringBuilder attachments = new StringBuilder();

            //Monster Summary Files
            if (emailData.MonsterSuccessfulFilename != null)
                attachments.Append(AddAttachment(emailData.MonsterSuccessfulFilename));

            if (emailData.MonsterUnsuccessfulFilename != null)
                attachments.Append(AddAttachment(emailData.MonsterUnsuccessfulFilename));

            if (emailData.MonsterIdenticalFilename != null)
                attachments.Append(AddAttachment(emailData.MonsterIdenticalFilename));

            if (emailData.MonsterInactiveFilename != null)
                attachments.Append(AddAttachment(emailData.MonsterInactiveFilename));

            if (emailData.MonsterRecordsNotFoundFileName != null)
                attachments.Append(AddAttachment(emailData.MonsterRecordsNotFoundFileName));

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
