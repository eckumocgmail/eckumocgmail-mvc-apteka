using Microsoft.AspNetCore.Mvc;

using Mvc_Apteka.Entities;

using System;
using System.Linq;

namespace Mvc_Apteka.Controllers
{
    /// <summary>
    /// Страница поиска с карточным отображением и фильтрами по цене и объёму продукции
    /// </summary>
    public class ProductsSearchController: Controller
    {
        public virtual IActionResult Index()=>Redirect("/ProductsSearch/Search");

        /// <summary>
        /// Переход к странице выбора 
        /// </summary>
        [HttpGet]
        public virtual IActionResult Search(
           [FromServices] AppDbContext context,
           [FromQuery] string searchInput = "",
           [FromQuery] float minPrice = 0,
           [FromQuery] float maxPrice = 1000000,
           [FromQuery] int minCount = 0,
           [FromQuery] int maxCount = 1000000,

           [FromQuery] int PageNumber = 1,
           [FromQuery] int PageSize = 10)
        {
            var model = OnSearch(context, searchInput, minPrice, maxPrice, minCount, maxCount, PageNumber, PageSize);
            return View( model );
        }


        /// <summary>
        /// Выбор данных
        /// </summary>
        public virtual object OnSearch([FromServices] AppDbContext context,
            [FromQuery] string searchInput = "",
            [FromQuery] float minPrice = 0,
            [FromQuery] float maxPrice = 1000000,
            [FromQuery] int minCount = 0,
            [FromQuery] int maxCount = 1000000,

            [FromQuery] int PageNumber = 1,
            [FromQuery] int PageSize = 10)
        {
            IQueryable<ProductInfo> infos = null;
            if ((String.IsNullOrWhiteSpace(searchInput)))
            {
                infos = context.ProductInfos.AsQueryable();
            }
            else
            {
                infos = context.ProductInfos.Where(p => p.ProductName.ToUpper().IndexOf(searchInput.ToUpper()) != -1);
            }
            infos = context.ProductsSearch(infos, minCount, maxCount, minPrice, maxPrice);
            return new
            {
                PageNumber = PageNumber,
                PageSize = PageSize,
                SearchQuery = searchInput,
                TotalResults = infos.Count(),
                SearchResults = infos.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList()
            };
        }


        /// <summary>
        /// Запрос терминов для автоподстановки в строке поиска
        /// </summary>       
        [HttpGet]
        public virtual object OnInput([FromServices] AppDbContext context, [FromQuery] string value)
        {
            HttpContext.Response.ContentType = "application/json";

            var products =
                String.IsNullOrWhiteSpace(value) ?
                context.ProductInfos :
                context.ProductInfos.Where(p => p.ProductName.ToUpper().IndexOf(value.ToUpper()) != -1);
            return new
            {
                Options = products.Select(p => p.ProductName).ToList()
            };
        }
    }
}
