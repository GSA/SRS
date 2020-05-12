using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace SRS.Utilities
{
    internal class Email : IDisposable
    {
        protected string _strSmtpServer = string.Empty;
        protected string _strFrom = string.Empty;
        protected string _strTo = string.Empty;
        protected string _strCc = string.Empty;
        protected string _strBcc = string.Empty;
        protected string _strSubject = string.Empty;
        protected string _strBody = string.Empty;
        protected string _strAttachments = string.Empty;
        protected bool _IsBodyHtml = false;

        private MailMessage message = new MailMessage();
        private SmtpClient SmtpMail = new SmtpClient();

        public void Send(string strFrom, string strTo, string strCc, string strBcc, string strSubject,
                         string strBody, string strAttachments, string strSmtpServer, bool IsBodyHtml = false)
        {
            _strFrom = strFrom;
            _strTo = strTo;
            _strCc = strCc;
            _strBcc = strBcc;
            _strSubject = strSubject;
            _strBody = strBody;
            _strAttachments = strAttachments;
            _strSmtpServer = strSmtpServer;
            _IsBodyHtml = IsBodyHtml;

            SendEmail();
        }

        private void SendEmail()
        {
            // Don't attempt an email if there is no smtp server
            if ((!string.IsNullOrEmpty(_strSmtpServer)) && (_strSmtpServer != null))
            {
                try
                {
                    // Create Mail object
                    message = new MailMessage();

                    // Set properties needed for the email
                    MailAddress mail_from = new MailAddress(_strFrom);

                    message.From = mail_from;

                    if (_strTo.Trim().Length > 0)

                        message.To.Add(_strTo);

                    if (_strCc.Trim().Length > 0)

                        message.CC.Add(_strCc);

                    if (_strBcc.Trim().Length > 0)

                        message.Bcc.Add(_strBcc);

                    message.Subject = _strSubject;
                    message.Body = _strBody;
                    message.IsBodyHtml = _IsBodyHtml;

                    if (_strAttachments.Contains(";"))
                    {
                        // Split multiple attachments into a string array
                        Array a = _strAttachments.Split(';');

                        // Loop through attachments list and add to objMail.Attachments one at a time
                        for (int i = 0; i < a.Length; i++)
                        {
                            message.Attachments.Add(new Attachment(a.GetValue(i).ToString().Trim()));
                        }
                    }
                    else if (_strAttachments.Length > 0)  
                    {
                        message.Attachments.Add(new Attachment(_strAttachments.ToString().Trim()));
                    }

                    SmtpMail = new SmtpClient(_strSmtpServer);

                    SmtpMail.Send(message);

                    SmtpMail.Dispose();
                    message.Dispose();
                }
                catch (SmtpFailedRecipientsException ex)
                {
                    string smtpfailedrecipients_msg = string.Empty;

                    for (int i = 0; i < ex.InnerExceptions.Length; i++)
                    {
                        SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;

                        if (status == SmtpStatusCode.MailboxBusy ||
                            status == SmtpStatusCode.MailboxUnavailable)
                        {
                            // do nothing
                        }
                        else
                        {
                            smtpfailedrecipients_msg = string.Format("Failed to deliver message to {0}\n",
                                ex.InnerExceptions[i].FailedRecipient);
                        }
                    }
                    throw new SmtpException(smtpfailedrecipients_msg, ex);
                }
            }
        }

        /// <summary>
        /// Disposes of object
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                SmtpMail.Dispose();
                message.Dispose();
            }
            // free native resources
        }

        /// <summary>
        /// Disposes of object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
         
    }
}

