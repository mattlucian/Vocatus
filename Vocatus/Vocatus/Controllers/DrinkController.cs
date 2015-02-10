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
                udm.allCurrentIngredients = results.OrderBy(r=>r.name).ToList();

                List<SelectListItem> remainingLiquors = new List<SelectListItem>();
                List<SelectListItem> remainingCordials = new List<SelectListItem>();
                List<SelectListItem> remainingMixers = new List<SelectListItem>();
                List<SelectListItem> remainingLiqueurs = new List<SelectListItem>();
                List<SelectListItem> remainingWines = new List<SelectListItem>();
                List<SelectListItem> remainingMisc = new List<SelectListItem>();


                foreach (Ingredient ie in udm.allPossibleIngredients)
                {
                    if (ie.type == "Liquor")
                    {
                        remainingLiquors.Add(new SelectListItem { Text = ie.name, Value = "" + ie.ingredients_id });
                    }
                    else if (ie.type == "Cordial")
                    {
                        remainingCordials.Add(new SelectListItem { Text = ie.name, Value = "" + ie.ingredients_id });
                    }
                    else if (ie.type == "Wine")
                    {
                        remainingWines.Add(new SelectListItem { Text = ie.name, Value = "" + ie.ingredients_id });
                    }
                    else if (ie.type == "Liqueur")
                    {
                        remainingLiqueurs.Add(new SelectListItem { Text = ie.name, Value = "" + ie.ingredients_id });
                    }
                    else if (ie.type == "Mixer")
                    {
                        remainingMixers.Add(new SelectListItem { Text = ie.name, Value = "" + ie.ingredients_id });
                    }
                    else if(ie.type == "Misc")
                    {
                        remainingMisc.Add(new SelectListItem { Text = ie.name, Value = "" + ie.ingredients_id });
                    }
                    
                }
                ViewBag.RemainingLiquors = remainingLiquors;
                ViewBag.RemainingCordials = remainingCordials;
                ViewBag.RemainingMisc = remainingMisc;
                ViewBag.RemainingWines = remainingWines;
                ViewBag.RemainingLiqueurs = remainingLiqueurs;
                ViewBag.RemainingMixers = remainingMixers;

                var allCocktails = db.Cocktails.ToList();
                udm.allPossibleCocktails = allCocktails.ToList();

                foreach (Cocktail c in allCocktails)
                {
                    List<Combination> combinations = db.Combinations.Where(j => j.cocktail_id == c.cocktail_id).ToList();
                    List<String> combz = new List<String>();
                    foreach (Combination id in combinations)
                    {
                        combz.Add(db.Ingredients.Where(i => i.ingredients_id == id.ingredients_id).Select(i => i.name).FirstOrDefault());
                    }


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

                    int checkCount = 0;
                    foreach(bool check in validChecks){
                        if(check == true){
                            checkCount++;
                        }
                    }
                    if((validChecks.Count() -1 ) == checkCount ){
                        


                        // just missing 1 ingredient
                        udm.allPartialCocktailCombinations.Add(new CocktailModel
                        {
                            cocktailImagePath = c.cocktail_image_path,
                            cocktailName = c.cocktail_name,
                            ingredients = combz,
                            partialIngredients = validChecks.ToList()
                        });

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

                foreach (Cocktail c in udm.allPossibleCocktails)
                {
                    List<int> comboIds = db.Combinations.Where(co => co.cocktail_id == c.cocktail_id).Select(co=>co.ingredients_id).ToList();
                    List<String> combos = new List<String>();
                    foreach (int id in comboIds)
                    {
                        combos.Add(db.Ingredients.Where(i=>i.ingredients_id == id).Select(i => i.name).FirstOrDefault());
                    }

                    udm.allCocktailCombinations.Add(new CocktailModel
                    {
                        ingredients = combos,
                        cocktailName = c.cocktail_name,
                        cocktailImagePath = c.cocktail_image_path
                    });
                }

                udm.allCocktailCombinations = udm.allCocktailCombinations.OrderBy(co => co.cocktailName).ToList();
                return View(udm);

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public void InsertIngredientForUser(int ingredientID)
        {
            if (Request.IsAuthenticated)
            {

                VocatusEntities db = new VocatusEntities();
                db.IngredientsOnHands.Add(new IngredientsOnHand
                {
                    user_name = User.Identity.Name,
                    ingredient_id = ingredientID
                });

                db.SaveChanges();
            }
            else
            {
                RedirectToAction("Login", "Account");
            }
        }

        public void RemoveIngredientForUser(int ingredientID)
        {
            if (Request.IsAuthenticated)
            {

                VocatusEntities db = new VocatusEntities();
                
                IngredientsOnHand ioh = new IngredientsOnHand
                {
                    user_name = User.Identity.Name,
                    ingredient_id = ingredientID
                };

                db.IngredientsOnHands.Attach(ioh);

                db.IngredientsOnHands.Remove(ioh);

                db.SaveChanges();
            }
            else
            {
                RedirectToAction("Login", "Account");
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