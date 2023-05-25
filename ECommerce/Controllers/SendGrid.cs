using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;
using System.Net;
using Microsoft.Build.Tasks;

namespace ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendGrid : ControllerBase
    {
        public readonly ISendGridClient _sendGridClient;
        public readonly IConfiguration _configuration;

        public SendGrid(ISendGridClient sendGridClient, IConfiguration configuration)
        {
            _sendGridClient = sendGridClient;
            _configuration = configuration;
        }


        [HttpGet("send-text-email")]
        public async Task<IActionResult>Get(string ToMail)
        {
            string fromMail = _configuration.GetSection("sendGridEmailSettings")
            .GetValue<string>("FromEmail");

            string fromName = _configuration.GetSection("sendGridEmailSettings")
            .GetValue<string>("Yash");


            var msg = new SendGridMessage()
            {
                From = new EmailAddress(fromMail,    fromName),
                Subject = "Mail by SendGrid",
                PlainTextContent = "Hello Welcome!!!",

                HtmlContent = HtmlContent()
            };

            msg.AddTo(ToMail);
            var response = await _sendGridClient.SendEmailAsync(msg);

            string message = response.IsSuccessStatusCode ? "Email Send":"Email Sending Failed";

            return Ok(message);
        }
        private string HtmlContent()
        {
            return "<h1>Hello<h1>";
        }



        [HttpGet("SMTP")]
        public async Task<IActionResult> Get1()
        {

            string fromMail = "malikyash67@gmail.com";
            string fromPassword = "cgdrlyrkgiwzaiwb";

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);

            message.Subject = "Test Subject";
            message.To.Add(new MailAddress("malikyash67@gmail.com"));
            message.Body = "<html><body> Test Body </body></html>";
            
            message.IsBodyHtml = true;
            System.Net.Mail.Attachment attachment;
            attachment = new System.Net.Mail.Attachment("hello");
            message.Attachments.Add(attachment);

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true,
            };
            smtpClient.Send(message);

            return Ok(message);
        }

    }

    //  cgdrlyrkgiwzaiwb

}
