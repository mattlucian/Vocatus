using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vocatus.Models
{
    public class CocktailModel 
    {
        public String cocktailName;
        public int cocktailId;
        public List<String> ingredients = new List<String>();
        public List<bool> partialIngredients = new List<bool>();
        public String cocktailImagePath;
        public int rating;
	}

    public class RatingKeyValuePair
    {
        public int id;
        public int rating;
    }
}