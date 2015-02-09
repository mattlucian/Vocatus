using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vocatus.Models
{
    public class UserDrinkModel 
    {
        public List<Ingredient> allPossibleIngredients = new List<Ingredient>();
        public List<Ingredient> allCurrentIngredients = new List<Ingredient>();
        public List<Cocktail> allPossibleCocktails = new List<Cocktail>();
        public List<CocktailModel> allCocktailCombinations = new List<CocktailModel>();


	}
}