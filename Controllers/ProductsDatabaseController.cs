using Microsoft.AspNetCore.Mvc;

using System;

namespace Mvc_Apteka.Controllers
{

    /// <summary>
    /// Управление структурой БД
    /// </summary>
    public class ProductsDatabaseController: Controller
    {


        /// <summary>
        /// Обновление структуры данных
        /// </summary>
        public bool CreateDatabase([FromServices] AppDbContext context)
        {
            bool result = false;
            try
            {
                result = context.Database.EnsureCreated();
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Уничтожение структуры данных
        /// </summary>
        public bool DeleteDatabase([FromServices] AppDbContext context)
        {
            bool result = false;
            try
            {
                result = context.Database.EnsureDeleted();
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
