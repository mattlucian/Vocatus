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
        public List<String> ingredients = new List<String>();
        public String cocktailImagePath;
	}
}