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
       
        string from, to, cc, bcc, subject, body, server;
        private RetrieveData rd;
        private bool Debug;

 
        private void setEmailDefaults()
        {
            from = "FROM".GetEmailSetting();
            to = "TO".GetEmailSetting();
            cc = "CC".GetEmailSetting();
            bcc = "BCC".GetEmailSetting();
            subject = "EmailSubject".GetEmailSetting();
            body = File.ReadAllText("EmailTemplate".GetEmailSetting());
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
        private string AccessEmailTo(string to, ContractorData contractorData, bool debug)
        {
            return contractorData.contract_POC_Email;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cc"></param>
        /// <param name="contractorData"></param>
        /// <param name="debug"></param>
        /// <returns></returns>
        private string AccessEmailCC(string cc, ContractorData contractorData, bool debug)
        {
            return contractorData.RegionalEmail;
        }
        private string AccessEmailBCC(string bcc, ContractorData contractorData, bool debug)
        {
            return contractorData.RegionalEmail;
        }
        private string AccessEmailSubject(String subject, ContractorData contractorData, bool debug)
        {
            string eSubject = subject;

            eSubject = eSubject.Replace("[PersID]", contractorData.PersID);
            //eSubject = eSubject.Replace("[ContractNumber]", contractorData.contract_number);
            eSubject = eSubject.Replace("[ContractDateEnd]", contractorData.contract_date_end.ToString("MM/DD/YYYY"));

            return eSubject;
        }

        private string AccessEmailBody(string body, ContractorData contractorData, bool debug)
        {
            string eBody = body;

            eBody = eBody.Replace("[PersID]", contractorData.PersID);
            //eBody = eBody.Replace("[ContractNumber]", contractorData.contract_number);
            eBody = eBody.Replace("[ContractDateEnd]", contractorData.contract_date_end.ToString("MM/DD/YYYY"));

            return eBody;
        }
        internal bool AccessEmailTemplate(string emailTemplateName, ref string subject, ref string body)
        {
            try
            {
                switch (emailTemplateName)
                {
                    case EmailTemplate.ExpiringContractorEmailTemplate:
                        subject = "".GetEmailSetting();
                        body = File.ReadAllText("".GetEmailSetting());
                        break;
                    case EmailTemplate.ExpiredContractorEmailTemplate:
                        subject = "".GetEmailSetting();
                        body = File.ReadAllText("".GetEmailSetting());
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

        internal string SendExpiringContractorEmailTemplate(ContractorData row)
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
    }
    public static class SettingMethods
    {
        public static string GetEmailSetting(this string setting)
        {
            return ConfigurationManager.AppSettings[setting];
        }
        public static string Prepend(this string x, string pre)
        {
            return pre + x;

        }
        public static DateTime GetDateTime(this DateTime date, string arg)
        {
            if (string.IsNullOrWhiteSpace(arg)) return DateTime.Now;
            DateTime dt;
            var isValidDate = DateTime.TryParse(arg, out dt);
            if (isValidDate)
                return dt;
            Console.WriteLine("Invalid DATE");
            throw new ArgumentException("Invalid date argument exception");
        }
    }
}
 