using SRS.Models;
using SRS.Data;
using System;
using System.IO;

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
        private string AccessEmailTo(string to, Contractor contractorData, bool debug)
        {
           // return contractorData.RegionalEMails;
            return contractorData.gpoc_emails; 
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cc"></param>
        /// <param name="contractorData"></param>
        /// <param name="debug"></param>
        /// <returns></returns>
        private string AccessEmailCC(string cc, Contractor contractorData, bool debug)
        {
            return contractorData.gpoc_emails;
           // return contractorData.RegionalEMails;
        }
        //private string AccessEmailBCC(string bcc, Contractor contractorData, bool debug)
        //{
        //    return contractorData.gpoc_emails;
        //}
        private string AccessEmailSubject(String subject, Contractor contractorData, bool debug)
        {
            string eSubject = subject;

            eSubject = eSubject.Replace("[LastName]", contractorData.LastName);
            eSubject = eSubject.Replace("[Suffix]", contractorData.Suffix);
            eSubject = eSubject.Replace("[FirstName]", contractorData.FirstName);
            eSubject = eSubject.Replace("[MiddleName]", contractorData.MiddleName);
            eSubject = eSubject.Replace("[pers_investigation_date]", contractorData.pers_investigation_date.ToString("MM/DD/YYYY"));
              
            return eSubject;
        }

        private string AccessEmailBody(string body, Contractor contractorData, bool debug)
        {
            string eBody = body;

            eBody = eBody.Replace("[LastName]", contractorData.LastName);
            eBody = eBody.Replace("[Suffix]", contractorData.Suffix);
            eBody = eBody.Replace("[FirstName]", contractorData.FirstName);
            eBody = eBody.Replace("[MiddleName]", contractorData.MiddleName);
            eBody = eBody.Replace("[DaysToExpiration]", contractorData.DaysToExpiration.ToString());
            eBody = eBody.Replace("[pers_investigation_date]", contractorData.pers_investigation_date.ToString("MM/DD/YYYY"));
           
            return eBody;
        }
        internal bool AccessEmailTemplate(string templateName, ref string subject, ref string body)
        {
            try
            {
                switch (templateName)
                {
                    case EmailTemplate.ExpiringContractorEmailTemplate:
                        subject = "SACExpiringSubject".GetEmailSetting();
                        body = File.ReadAllText("ExpiringSACEmailTemplate".GetEmailSetting());
                        break;
                    case EmailTemplate.ExpiredContractorEmailTemplate:
                        subject = "SACExpiredSubject".GetEmailSetting();
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
            //BCC = AccessEmailBCC(BCC, row, Debug);
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
           // BCC = AccessEmailBCC(BCC, row, Debug);
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
 