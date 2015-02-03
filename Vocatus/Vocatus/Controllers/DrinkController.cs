using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vocatus.Models;

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
            if (Request.IsAuthenticated)
            {

                VocatusEntities db = new VocatusEntities();
                var name = this.User.Identity.Name;
                var ingredientsOnHandList = db.IngredientsOnHands.Where(i => i.user_name == name);
                if (ingredientsOnHandList == null || !ingredientsOnHandList.Any())
                {
                    return View(new List<Ingredient>());
                }
                var realList = ingredientsOnHandList.ToList();
                List<Ingredient> results = new List<Ingredient>();

                foreach (IngredientsOnHand s in realList)
                {
                    results.Add(db.Ingredients.Where(x => x.ingredients_id == s.ingredient_id).FirstOrDefault());
                }
                return View(results);

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        


        public ActionResult Explore()
        {
            //TODO:
            VocatusEntities db = new VocatusEntities();
            
            // gets all the cocktails from the DB
            var cocktails = db.Cocktails.ToList();

            List<CocktailModel> drinks = new List<CocktailModel>();

            foreach(Cocktail c in cocktails){
                // var ingredients = 

                List<Combination> incredientsFound = db.Combinations.Where(z => z.cocktail_id == c.cocktail_id).ToList();
                CocktailModel cm = new CocktailModel()
                {
                    cocktailName = c.cocktail_name,
                    cocktailImagePath = c.cocktail_image_path
                };
                foreach (Combination z in incredientsFound)
                {
                    var ingredient = db.Ingredients.Where(i => i.ingredients_id == z.ingredients_id).FirstOrDefault();
                    var name = ingredient.name;

                    cm.ingredients.Add(name);
                }
                drinks.Add(cm);
            }

            //pull list of all the alcoholic beverages
                //pull list of all possible mixers
                //insert these into a model to pass to the view
                        
            return View(drinks);
        }
	}
}