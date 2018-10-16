using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HL_V1.Utils
{
    public class EmailSender
    {


        // Please use your API KEY here.
        private const String API_KEY = "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";

        public static void Send(String toEmail, String subject, String contents)
        {
            var client = new SendGridClient(API_KEY);
            var from = new EmailAddress("noreply@gmail.com", "HealthyLiving");
            var to = new EmailAddress(toEmail, "");
            var plainTextContent = contents;
            var htmlContent = "<p>" + contents + "</p>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = client.SendEmailAsync(msg);
        }



    }
}