using Microsoft.AspNetCore.Mvc;

using System.Linq;

namespace Mvc_Apteka.Controllers
{
    public class HomeController : Controller
    {      
        /// <summary>
        /// Главная страница
        /// </summary>        
        public IActionResult Index()=> View();

        /// <summary>
        /// Примечание
        /// </summary>    
        public IActionResult Privacy() => View();


        public IActionResult ClearHistory([FromServices] AppDbContext context)
        {
            foreach(var activity in context.Activities.ToList())
            {
                context.Activities.Remove(activity);
            }
            context.SaveChanges();
            return Redirect("/Home/Index");
        }
    }
}
