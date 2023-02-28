using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;

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
                From = new EmailAddress(fromMail, fromName),
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


    }

   
}
