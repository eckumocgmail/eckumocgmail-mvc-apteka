using Microsoft.AspNetCore.Mvc;

using System;

namespace Mvc_Apteka.Controllers
{
    public class ProductsDatabaseController: Controller
    {

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
