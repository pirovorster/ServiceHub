using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace ServiceHub.Website
{
	public sealed class EmailSender
	{
		private readonly SmtpClient _smtpClient;
		public EmailSender()
		{
			_smtpClient = new SmtpClient();
		}

		public void SendMail(
			string toAddresses,
			string subject,
			string body)
		{
			SendMail(new string[] { toAddresses }, null, null, null, subject, body,true);
		}


		public void SendMail(
			IEnumerable<string> toAddresses,
			string subject,
			string body)
		{
			SendMail(toAddresses, null, null, null, subject, body, true);
		}


		public void SendMail(
			IEnumerable<string> toAddresses,
			IEnumerable<string> ccAddresses,
			IEnumerable<string> bccAddresses,
			IEnumerable<EmailAttachment> attachments,
			string subject,
			string body,
			bool isBodyHtml = false,
			string replyTo = null)
		{

			using (MailMessage message = new MailMessage
			{
				Subject = subject,
				Body = body,
				IsBodyHtml = isBodyHtml
			})
			{
				if (toAddresses != null)
				{
					foreach (var toAddress in toAddresses)
					{
						message.To.Add(toAddress);
					}
				}
				if (ccAddresses != null)
				{
					foreach (var ccAddress in ccAddresses)
					{
						message.CC.Add(ccAddress);
					}
				}
				if (bccAddresses != null)
				{
					foreach (var bccAddress in bccAddresses)
					{
						message.Bcc.Add(bccAddress);
					}
				}

				if (replyTo != null)
					message.ReplyToList.Add(new MailAddress(replyTo));

				if (attachments != null)
				{
					foreach (var attachment in attachments)
					{
						message.Attachments.Add(new Attachment(new System.IO.MemoryStream(attachment.Buffer), attachment.FileName));
					}
				}

				_smtpClient.Send(message);
			}
		}
	}
}
