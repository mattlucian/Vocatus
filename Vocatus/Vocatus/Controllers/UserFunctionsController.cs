using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Vocatus.Models;

namespace Vocatus.Controllers
{
    public class UserFunctionsController : Controller
    {
        //
        // GET: /UserFunctions/
        public string RateCocktail(string cocktailId, string rating)
        {
            Int32 cid = Convert.ToInt32(cocktailId);
            Int32 r = Convert.ToInt32(rating);

            VocatusEntities vdb = new VocatusEntities();

            if(Request.IsAuthenticated){
                
                var user = User.Identity.Name;
                var userid = vdb.AspNetUsers.Where(x => x.UserName == user).Select(x => x.Id).FirstOrDefault();

                var check = vdb.CocktailRatings.FirstOrDefault(x => x.cocktail_id == cid && x.user_id == userid);

                if (check != null)
                {
                    // update rating
                    check.rating = r;
                    vdb.SaveChanges();
                    return "updated";
                }
                else
                {
                    // insert
                    CocktailRating cr = new CocktailRating()
                    {
                        user_id = userid,
                        rating = r,
                        cocktail_id = cid
                    };
                    vdb.CocktailRatings.Add(cr);
                    vdb.SaveChanges();
                    return "added";
                }
            }
            else
            {
                return "disregarded, as there was an error";
            }            
        }

        public string GetPotentialIngredientsToAddWithPartial(string partial)
        {
            List<String> ingredients = new List<String>();
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                var command = con.CreateCommand();
                command.Parameters.Add("@user", User.Identity.Name);
                command.Parameters.Add("@partial", partial);

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ingredients.Add(Convert.ToString(reader["name"]));
                }

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Serialize(ingredients);
            }
        }

	}
}