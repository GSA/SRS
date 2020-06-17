using SRS.Models;
using SRS.Utilities;
using SRS.Data;
using SRS.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using System.Diagnostics.Contracts;

namespace SRS.Utilities
{
    public class AccessEmail
    {
        private static log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Email email = new Email();
        private bool Debug;
        string from, to, cc, bcc, subject, body, server;
        private RetrieveData retrieveData = new RetrieveData();

        private void setEmailDefaults()
        {
            from = "DEFAULTEMAIL".GetEmailSetting();
            to = "TO".GetEmailSetting();
            cc = "CC".GetEmailSetting();
            bcc = "BCC".GetEmailSetting();
            subject = "EmailSubject".GetEmailSetting();
            body = File.ReadAllText("Summary".GetEmailSetting());
            server = "SMTPServer".GetEmailSetting();
        }
        private bool SendEmail(string to, string cc, string bcc, string subject, string body)
        {
            _log.Info("Email Configuration");
            setEmailDefaults();

            string From, To, CC, BCC, Subject, Body, Attachments, SMTPServer;

            From = from;
            To = to;
            CC = cc;
            BCC = bcc;
            Subject = subject;
            Body = body;
            Attachments = "";
            SMTPServer = server;

            email.Send(From, To, CC, BCC, Subject, Body, Attachments, SMTPServer, true);
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="to"></param>
        /// <param name="contractorData"></param>
        /// <param name="debug"></param>
        /// <returns></returns>
        private string AccessEmailTo(string to, Contractor contractorData, bool Debug)
        {
            return contractorData.gpoc_emails;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cc"></param>
        /// <param name="contractorData"></param>
        /// <param name="debug"></param>
        /// <returns></returns>
        private string AccessEmailCC(string cc, Contractor contractorData, bool Debug)
        {
            return contractorData.gpoc_emails;
        }
        private string AccessEmailBCC(string bcc, Contractor contractorData, bool Debug)
        {
            return contractorData.gpoc_emails;
        }
        private string AccessEmailSubject(String subject, Contractor contractorData, bool Debug)
        {
            string eSubject = subject;

            eSubject = eSubject.Replace("[RegionalEMails]", contractorData.RegionalEMails); 
            eSubject = eSubject.Replace("[ContractorDateEnd]", contractorData.pers_investigation_date.ToString("MM/DD/YYYY"));

            return eSubject;
        }

        private string AccessEmailBody(string body, Contractor contractorData, bool Debug)
        {
            string eBody = body;

            eBody = eBody.Replace("[RegionalEMails]", contractorData.RegionalEMails);
         
            eBody = eBody.Replace("[ContractDateEnd]", contractorData.pers_investigation_date.ToString("MM/DD/YYYY"));

            return eBody;
        }
        internal bool AccessEmailTemplate(string templateName, ref string subject, ref string body)
        {
            try
            {
                switch (templateName)
                {
                    case EmailTemplate.ExpiringContractorEmailTemplate:
                        subject = "EMAILSUBJECT".GetEmailSetting();
                        body = File.ReadAllText("ExpiringSACEmailTemplate".GetEmailSetting());
                        break;
                    case EmailTemplate.ExpiredContractorEmailTemplate:
                        subject = "EMAILSUBJECT".GetEmailSetting();
                        body = File.ReadAllText("ExpiredSACEmailTemplate".GetEmailSetting());
                        break;
                    default:
                        break;

                }
                return true;
            }
            catch (Exception e)
            {
                _log.Error(e.Message);
                return false;
            }
        }
        internal string prependStatusMessage(bool debug, string message)
        {
            return message.Prepend(debug ? "**DEBUG** " : "");
        }

        internal string SendExpiringContractorEmailTemplate(Contractor row)
        {
            string Subject = "", Body = "", To = "", CC = "", BCC = "";

            if (Debug)
            {
                _log.Info("Sending debug email");
                AccessEmailTemplate(EmailTemplate.DebugExpiringContractorEmailTemplate, ref Subject, ref Body);

            }
            else
            {
                _log.Info("Sending email.");
                AccessEmailTemplate(EmailTemplate.ExpiringContractorEmailTemplate, ref Subject, ref Body);
            }

            To = AccessEmailTo(To, row, Debug);
            CC = AccessEmailCC(CC, row, Debug);
            BCC = AccessEmailBCC(BCC, row, Debug);
            Subject = AccessEmailSubject(Subject, row, Debug);
            Body = AccessEmailBody(Body, row, Debug);

            _log.Info("The function of email sending call");

            bool Result = SendEmail(To, CC, BCC, Subject, Body);

            if (Result)
            {
                _log.Info("Sent email Successfully.");
                return prependStatusMessage(Debug, "The email sent successfully.");
            }
            else
            {
                _log.Info("Faild to send email.");
                return prependStatusMessage(Debug, "Failed to send email.");
            }
        }
         internal string SendExpiredContractorEmailTemplate(Contractor row)
        {
            string Subject = "", Body = "", To = "", CC = "", BCC = "";

            if (Debug)
            {
                _log.Info("Sending debug email");
                AccessEmailTemplate(EmailTemplate.DebugExpiredContractorEmailTemplate, ref Subject, ref Body);

            }
            else
            {
                _log.Info("Sending email.");
                AccessEmailTemplate(EmailTemplate.ExpiredContractorEmailTemplate, ref Subject, ref Body);
            }

            To = AccessEmailTo(To, row, Debug);
            CC = AccessEmailCC(CC, row, Debug);
            BCC = AccessEmailBCC(BCC, row, Debug);
            Subject = AccessEmailSubject(Subject, row, Debug);
            Body = AccessEmailBody(Body, row, Debug);

            _log.Info("The function of email sending call");

            bool Result = SendEmail(To, CC, BCC, Subject, Body);

            if (Result)
            {
                _log.Info("Sent email Successfully.");
                return prependStatusMessage(Debug, "The email sent successfully.");
            }
            else
            {
                _log.Info("Faild to send email.");
                return prependStatusMessage(Debug, "Failed to send email.");
            }
        }
    }
}
 