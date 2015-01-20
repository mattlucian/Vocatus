using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vocatus.Controllers
{
    public class DrinkController : Controller
    {
        //
        // GET: /Drink/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MyDrinks()
        {
            return View();
        }

        public ActionResult Explore()
        {
            //TODO:
                //pull list of all the alcoholic beverages
                //pull list of all possible mixers
                //insert these into a model to pass to the view
                //
            
            return View();
        }
	}
}