using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DerekApplication.Content;

namespace DerekApplication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            var db = new DB_84924_vocatusEntities();
            var allCompanies = db.Companies;

            return View(allCompanies);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Details(int id)
        {
            var db = new DB_84924_vocatusEntities();
            Company model = db.Companies.Where(x => x.company_id == id).FirstOrDefault();
            return View(model);
        }

        public ActionResult Add()
        {
            return View();
        }

        public void Insert(String name, String phone, String address1, String address2, String city, String state, String country, String zip)
        {
            var db = new DB_84924_vocatusEntities();
            int newID = db.Companies.OrderByDescending(c => c.company_id).FirstOrDefault().company_id + 1;

            var companyToInsert = new Company()
            {
                company_name = name,
                company_phone = phone,
                company_address1 = address1,
                company_address2 = address2,
                company_city = city,
                company_state = state,
                company_country = country,
                company_zip = zip
            };
            try
            {
                db.Companies.Add(companyToInsert);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.Write(e.GetBaseException());
            }
            
        }

    }
}