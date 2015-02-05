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

                var realList = ingredientsOnHandList.ToList();
                List<Ingredient> results = new List<Ingredient>();
                UserDrinkModel udm = new UserDrinkModel();

                udm.allPossibleIngredients = db.Ingredients.OrderBy(o=>o.name).ToList();
                
                foreach (IngredientsOnHand s in realList)
                {
                    results.Add(db.Ingredients.Where(x => x.ingredients_id == s.ingredient_id).FirstOrDefault());
                    udm.allPossibleIngredients.Remove(db.Ingredients.Where(i => i.ingredients_id == s.ingredient_id).FirstOrDefault());
                }
                udm.allCurrentIngredients = results;

                List<SelectListItem> remainingLiqours = new List<SelectListItem>();
                List<SelectListItem> remainingCordials = new List<SelectListItem>();
                List<SelectListItem> remainingMisc = new List<SelectListItem>();


                foreach (Ingredient ie in udm.allPossibleIngredients)
                {
                    if (ie.type == "Liquor")
                    {
                        remainingLiqours.Add(new SelectListItem { Text = ie.name, Value = "" + ie.ingredients_id });
                    }
                    else if (ie.type == "Cordial")
                    {
                        remainingCordials.Add(new SelectListItem { Text = ie.name, Value = "" + ie.ingredients_id });
                    }
                    else
                    {
                        remainingMisc.Add(new SelectListItem { Text = ie.name, Value = "" + ie.ingredients_id });
                    }
                    
                }
                ViewBag.RemainingLiqours = remainingLiqours;
                ViewBag.RemainingCordials = remainingCordials;
                ViewBag.RemainingMisc = remainingMisc;


                var allCocktails = db.Cocktails.ToList();
                udm.allPossibleCocktails = allCocktails.ToList();

                foreach (Cocktail c in allCocktails)
                {
                    List<Combination> combinations = db.Combinations.Where(j => j.cocktail_id == c.cocktail_id).ToList();
                    bool[] validChecks = new bool[combinations.Count];
                    int count = 0;
                    foreach (Combination g in combinations)
                    {
                        foreach (Ingredient i in udm.allCurrentIngredients)
                        {
                            if (i.ingredients_id == g.ingredients_id)
                            {
                                validChecks[count] = true;
                            }    
                        }
                        count++;
                    }

                    foreach (bool check in validChecks)
                    {
                        if (check == false)
                        {
                            udm.allPossibleCocktails.Remove(c);
                            break;
                        }
                    }
                }
                    

                return View(udm);

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

            return View(drinks);
        }
	}
}