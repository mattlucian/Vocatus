using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;


namespace Vocatus.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "About";

            return View();
        }

        public void SendEmail(string message, string senderEmail, string name)
        {

            MailMessage mail = new MailMessage();
            mail.To.Add("matt@vocatus.net");
            mail.From = new MailAddress("vocatus.contact@gmail.com");//vocatus email used my gmail
            string emailSubject = "Message From User: " + name + " Email: " + senderEmail;
            mail.Subject= emailSubject;
            mail.Body = message;
            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";//smtp.gmail.co
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("vocatus.contact@gmail.com", "!dr0wssaP");//vocatus user name and password used my personal gmail
            smtp.EnableSsl = true;
            smtp.Send(mail);

        }
    }
}