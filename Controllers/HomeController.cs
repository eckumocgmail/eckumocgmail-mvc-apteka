using Microsoft.AspNetCore.Mvc;

namespace Mvc_Apteka.Controllers
{
    public class HomeController : Controller
    {      
        public IActionResult Index()=> View();
        public IActionResult Privacy() => View();
    }
}
