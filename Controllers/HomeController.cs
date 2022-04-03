using Microsoft.AspNetCore.Mvc;

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
    }
}
