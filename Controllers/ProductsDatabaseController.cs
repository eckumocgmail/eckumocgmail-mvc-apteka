using Microsoft.AspNetCore.Mvc;

using System;

namespace Mvc_Apteka.Controllers
{

    /// <summary>
    /// Управление структурой БД
    /// </summary>
    public class ProductsDatabaseController: Controller
    {

        [HttpGet]
        public IActionResult Config() => View("Config", Startup.ConnectionString);

        [HttpPost]
        public IActionResult Config(string ConnectionString)
        {
            using (var appDb = new AppDbContext() {
                ConnectionString = ConnectionString 
            }) {


                if (appDb.Database.CanConnect())
                {                                         
                    return Redirect("/Home/Index");
                }
                else
                {
                    
                }
            }
            return View("Config",ConnectionString);
            
        }


        /// <summary>
        /// Обновление структуры данных
        /// </summary>
        public IActionResult CreateDatabase([FromServices] AppDbContext context)
        {
            bool result = false;
            try
            {
                result = context.Database.EnsureCreated();
                return View("CreateDatabase", result);
            }
            catch (Exception)
            {
                return View("CreateDatabase", false);
            }
        }

        /// <summary>
        /// Уничтожение структуры данных
        /// </summary>
        public IActionResult DeleteDatabase([FromServices] AppDbContext context)
        {
            bool result = false;
            try
            {
                result = context.Database.EnsureDeleted();
                return View("DeleteDatabase", result);
            }
            catch (Exception)
            {
                return View("DeleteDatabase", false);
            }
        }
    }
}
