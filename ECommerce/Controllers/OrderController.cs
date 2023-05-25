using BusinessLayer.Repository;
using DataAcessLayer.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PdfSharpCore.Pdf;
using PdfSharpCore;
using System.Security.Claims;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using DataAcessLayer.Entity;
using DataAcessLayer.DBContext;
using SendGrid.Helpers.Mail;
using EllipticCurve.Utils;
using SendGrid;
using System.Collections;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;
using MimeKit;

namespace ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrders _order;
        private readonly EcDbContext _ecDbContext;
        public readonly ISendGridClient _sendGridClient;
        public readonly IConfiguration _configuration;

        public OrderController(IOrders orders,EcDbContext ecDbContext, ISendGridClient sendGridClient, IConfiguration configuration)
        {
            _sendGridClient = sendGridClient;
            _configuration = configuration;
            _order = orders;
            _ecDbContext = ecDbContext;

        }

        [HttpPost("order-products"),Authorize]
        public async Task<IActionResult> OrderProduct(OrderDTO obj)
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = Convert.ToInt32(id);

            var res = _order.order(obj, userId);
           // GenratePdf(res.Result);
            return Ok(res.Result);
        }

  

        [HttpGet("view-orders"), Authorize]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> ViewOrders()
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = Convert.ToInt32(id);
            var order = await _order.ViewOrders(userId);
            return Ok(order);
        }

        [HttpGet("View-suppilar-order")]
        public async Task<IActionResult> viewSuppilarOrder(int id)
        {
            var order = await _order.ViewSuppilerOrders(id);
            return Ok(order);
        }

        [HttpGet("jhbb")]
        public FileContentResult GenratePdf(int id)
        {
            var ordersTable = _order.ReturnOrder(id).Result;
            
            var document = new PdfDocument();
           string Content = HtmlContent(id);
          

            PdfGenerator.AddPdfPages(document,Content, PageSize.A4);
            byte[] response = null;
            using (MemoryStream ms = new MemoryStream())
            {
                document.Save(ms);
                response = ms.ToArray();

            }


            string fileName = "Innvoice.pdf";
         //   SendMail("malikyash67@gmail.com", Content);
            
            var file = File(response, "application/pdf", fileName);

            var stream = new MemoryStream(response);
            Get1(stream,fileName);
   
            EmailAttachment  e1= new EmailAttachment();
            e1.ImageFile = new FormFile(stream, 0, response.Length, "name", "fileName");
            e1.ToEmail = "malikyash67@gmail.com";
            SendEmailFileAttchement2(e1,Content);

            return file; 
        }
        [HttpGet("SMTP")]
        public async Task<IActionResult> Get1(MemoryStream stream , string fileName)
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
           // attachment = new System.Net.Mail.Attachment(,response,"application/pdf");
        //    message.Attachments.Add("f", response, "application/pdf");
            System.Net.Mail.Attachment att = new System.Net.Mail.Attachment(stream, fileName, "application/pdf");
              message.Attachments.Add(att);

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true,
            };
            smtpClient.Send(message);

            return Ok(message);

        }
        [HttpPost]
        [Route("send-mail-with-file-attachement2")]
        public async Task<IActionResult> SendEmailFileAttchement2([FromForm] EmailAttachment emailFile,string content)
        {
            string fromEmail = _configuration.GetSection("SendGridEmailSettings")
            .GetValue<string>("FromEmail");

            string fromName = _configuration.GetSection("SendGridEmailSettings")
            .GetValue<string>("FromName");

            var msg = new SendGridMessage()
            {
                From = new EmailAddress(fromEmail, fromName),
                Subject = "Mail by SendGrid",
                PlainTextContent = "Hello Welcome!!!",
                HtmlContent = content

            };

            var ec = emailFile.ImageFile.FileName;

            var ed = emailFile.ImageFile.OpenReadStream();
            var ee = "application/pdf";

            await msg.AddAttachmentAsync(
                ec, ed, ee,
                //emailFile.ImageFile.OpenReadStream(),
                //emailFile.ImageFile.ContentType,
                "attachment"
            );
            msg.AddTo(emailFile.ToEmail);

            var response = await _sendGridClient.SendEmailAsync(msg);
            string message = response.IsSuccessStatusCode ? "Email Send Successfully" :
            "Email Sending Failed";
            return Ok(message);
        }

        [HttpPost]
        [Route("send-mail-with-file-attachement")]
        public async Task<IActionResult> SendEmailFileAttchement([FromForm] EmailAttachment emailFile)
        {
            string fromEmail = _configuration.GetSection("SendGridEmailSettings")
            .GetValue<string>("FromEmail");

            string fromName = _configuration.GetSection("SendGridEmailSettings")
            .GetValue<string>("FromName");

            var msg = new SendGridMessage()
            {
                From = new EmailAddress(fromEmail, fromName),

                Subject = "File Attachement Email",
                PlainTextContent = "Check Attached File",

            };

            var ec = emailFile.ImageFile.FileName;

            var ed = emailFile.ImageFile.OpenReadStream();
            var ee = "application/pdf";

            await msg.AddAttachmentAsync(
                ec,ed,ee,
                //emailFile.ImageFile.OpenReadStream(),
                //emailFile.ImageFile.ContentType,
                "attachment"
            );
            msg.AddTo(emailFile.ToEmail);

            var response = await _sendGridClient.SendEmailAsync(msg);
            string message = response.IsSuccessStatusCode ? "Email Send Successfully" :
            "Email Sending Failed";
            return Ok(message);
        }
        //public bool SendEmailWithAttachment(EmailDataWithAttachment emailData)
        //{
        //    try
        //    {
        //        MimeMessage emailMessage = new MimeMessage();

        //        MailboxAddress emailFrom = new MailboxAddress(_emailSettings.Name, _emailSettings.EmailId);
        //        emailMessage.From.Add(emailFrom);

        //        MailboxAddress emailTo = new MailboxAddress(emailData.EmailToName, emailData.EmailToId);
        //        emailMessage.To.Add(emailTo);

        //        emailMessage.Subject = emailData.EmailSubject;

        //        BodyBuilder emailBodyBuilder = new BodyBuilder();

        //        if (emailData.EmailAttachments != null)
        //        {
        //            byte[] attachmentFileByteArray;
        //            foreach (IFormFile attachmentFile in emailData.EmailAttachments)
        //            {
        //                if (attachmentFile.Length > 0)
        //                {
        //                    using (MemoryStream memoryStream = new MemoryStream())
        //                    {
        //                        attachmentFile.CopyTo(memoryStream);
        //                        attachmentFileByteArray = memoryStream.ToArray();
        //                    }
        //                    emailBodyBuilder.Attachments.Add(attachmentFile.FileName, attachmentFileByteArray, ContentType.Parse(attachmentFile.ContentType));
        //                }
        //            }
        //        }

        //        emailBodyBuilder.TextBody = emailData.EmailBody;
        //        emailMessage.Body = emailBodyBuilder.ToMessageBody();

        //        SmtpClient emailClient = new SmtpClient();
        //        emailClient.Connect(_emailSettings.Host, _emailSettings.Port, _emailSettings.UseSSL);
        //        emailClient.Authenticate(_emailSettings.EmailId, _emailSettings.Password);
        //        emailClient.Send(emailMessage);
        //        emailClient.Disconnect(true);
        //        emailClient.Dispose();

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        //Log Exception Details
        //        return false;
        //    }
        //}
        private string HtmlContent(int id)
        {
            var ordersTable = _order.ReturnOrder(id).Result;
            string htmlcontent = "<div style='width:100%; text-align:center'>";
            // htmlcontent += "<img style='width:80px;height:80%' src='" + "'   />";

            htmlcontent += "<h2>Welcome to Ecommerce project</h2>";



            if (ordersTable != null)
            {
                htmlcontent += "<h2> Invoice of OrderId:" + ordersTable.ID + " & Invoice Date:" + ordersTable.CreatedAt + "</h2>";
                //   htmlcontent += "<h3> Customer : " + ordersTable.Users.Nam + "</h3>";
                // htmlcontent += "<p>" + .DeliveryAddress + "</p>";
                // htmlcontent += "<h3> Contact : 9898989898 & Email :ts@in.com </h3>";
                htmlcontent += "<div>";
            }



            htmlcontent += "<table style ='width:100%; border: 1px solid #000'>";
            htmlcontent += "<thead style='font-weight:bold'>";
            htmlcontent += "<tr>";
            htmlcontent += "<td style='border:1px solid #000'> Product Code </td>";
            htmlcontent += "<td style='border:1px solid #000'> Description </td>";
            htmlcontent += "<td style='border:1px solid #000'>Qty</td>";
            htmlcontent += "<td style='border:1px solid #000'>Price</td >";
            htmlcontent += "<td style='border:1px solid #000'>Total</td>";
            htmlcontent += "</tr>";
            htmlcontent += "</thead >";

            htmlcontent += "<tbody>";
            var od = ordersTable.OrderDetails.ToArray();
            if (ordersTable.OrderDetails != null && ordersTable.OrderDetails.Count > 0)
            {
                foreach (var item in od)
                {
                    var pro = _ecDbContext.Products.FirstOrDefault(c => c.Id == item.ProductId);
                    htmlcontent += "<tr>";
                    htmlcontent += "<td>" + item.ProductId + "</td>";
                    htmlcontent += "<td>" + pro.ProductName + "</td>";
                    //  htmlcontent += "<td>" + "to be processed"+ "</td>";
                    htmlcontent += "<td>" + "1" + "</td >";
                    htmlcontent += "<td>" + pro.price + "</td>";
                    htmlcontent += "<td> " + pro.price + "</td >";
                    htmlcontent += "</tr>";
                };
            }
            htmlcontent += "</tbody>";

            htmlcontent += "</table>";
            htmlcontent += "</div>";

            htmlcontent += "<div style='text-align:right'>";
            htmlcontent += "<h1> Summary Info </h1>";
            htmlcontent += "<table style='border:1px solid #000;float:right' >";
            htmlcontent += "<tr>";
            htmlcontent += "<td style='border:1px solid #000'> Summary Total </td>";
            htmlcontent += "<td style='border:1px solid #000'> Summary Tax </td>";
            htmlcontent += "<td style='border:1px solid #000'> Summary NetTotal </td>";
            htmlcontent += "</tr>";
            if (ordersTable != null)
            {
                htmlcontent += "<tr>";
                htmlcontent += "<td style='border: 1px solid #000'> " + ordersTable.TotalPrice + " </td>";
                htmlcontent += "<td style='border: 1px solid #000'>" + "00" + "</td>";
                htmlcontent += "<td style='border: 1px solid #000'> " + ordersTable.TotalPrice + "</td>";
                htmlcontent += "</tr>";
            }
            htmlcontent += "</table>";
            htmlcontent += "</div>";

            htmlcontent += "</div>";

            return htmlcontent;
        }

        private string SendMail(string ToMail,string HtmlContent)
        {
            string fromMail = _configuration.GetSection("sendGridEmailSettings")
            .GetValue<string>("FromEmail");

            string fromName = _configuration.GetSection("sendGridEmailSettings")
            .GetValue<string>("FromName");



            var msg = new SendGridMessage()
            {
                From = new EmailAddress(fromMail, fromName),
                Subject = "Mail by SendGrid",
                PlainTextContent = "Hello Welcome!!!",

                HtmlContent = HtmlContent,

                //Attachments = new System.Net.Mail.Attachment("


        };
         

            msg.AddTo(ToMail);
            var response = _sendGridClient.SendEmailAsync(msg);

            string message = response.IsCompletedSuccessfully ? "Email Send" : "Email Sending Failed";

            return (message);
        }
    }
}