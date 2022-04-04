using Microsoft.AspNetCore.Mvc;
using Mvc_Apteka.Entities;

namespace Mvc_Apteka.Controllers
{

    /// <summary>
    /// Просмотр карточки продукции, изменение сведений, поиск, удаление
    /// </summary>
    public class ProductsController : ProductsSearchController
    {
        public IActionResult Clear([FromServices] AppDbContext context)
        {
            context.Clear();
            return Redirect("/Products/Search");
        }
        public override IActionResult Index() => Redirect("/Products/Search");


        /// <summary>
        /// Просмотр сведений о продукции поиском по списку
        /// </summary>
        public override IActionResult Search([FromServices] AppDbContext context,
           string searchInput = "",
           [FromQuery] float minPrice = 0,
           [FromQuery] float maxPrice = 1000000,
           [FromQuery] int minCount = 0,
           [FromQuery] int maxCount = 1000000,
           [FromQuery] int PageNumber = 1,
           [FromQuery] int PageSize = 10)
        {
                   
            return base.Search(context, searchInput, minPrice, maxPrice, minCount, maxCount, PageNumber, PageSize);
        }

        /// <summary>
        /// Перход к странице просмотра сведений о продукции
        /// </summary>
        public IActionResult Info([FromServices] AppDbContext context, int ID)
            => View(context.ProductInfos.Find(ID));


        /// <summary>
        /// Переход к странице регистрации сведений о продукции
        /// </summary>
        [HttpGet]
        public IActionResult Create([FromServices] AppDbContext context)
            => View(new ProductInfo());

        /// <summary>
        /// Фиксация сведений о новой продукции
        /// </summary>
        [HttpPost]
        public IActionResult Create([FromServices] AppDbContext context,  ProductInfo model)
        {
            model.ProductName = model.ProductName.Trim();
            if (ModelState.IsValid)
            {
                bool success = true;
                if (context.HasProductWithName(model.ProductName))
                {
                    success = false;
                    ModelState.AddModelError("ProductName", "Наименование уже существует");
                }
                if (model.ProductPrice <= 0)
                {
                    success = false;
                    ModelState.AddModelError("ProductPrice", "Цена - положительное число");
                }
                if (model.ProductCount <= 0)
                {
                    success = false;
                    ModelState.AddModelError("ProductCount", "Кол-во - положительное число");
                }
                if (success)
                {
                    context.ProductInfos.Add(model);
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }


        /// <summary>
        /// Переход к странице редактирования сведений о продукции
        /// </summary>
        [HttpGet]
        public IActionResult Edit([FromServices] AppDbContext context, int ID)
            => View(context.ProductInfos.Find(ID));


        /// <summary>
        /// Фиксация измененных сведений о продукции
        /// </summary>
        [HttpPost]
        public IActionResult Edit([FromServices] AppDbContext context, int ID, ProductInfo model)
        {
            model.ProductName = model.ProductName.Trim();
            if (ModelState.IsValid)
            {
                bool success = true;
                
                if (model.ProductPrice <= 0)
                {
                    success = false;
                    ModelState.AddModelError("ProductPrice", "Цена - положительное число");
                }
                if (model.ProductCount <= 0)
                {
                    success = false;
                    ModelState.AddModelError("ProductCount", "Кол-во - положительное число");
                }
                if (success)
                {
                    var product = context.ProductInfos.Find(ID);
                    if (product.ProductName != model.ProductName)
                    {
                        if (context.HasProductWithName(model.ProductName))
                        {
                             
                            ModelState.AddModelError("ProductName", "Наименование уже существует");
                            return View(model);
                        }
                    }
                    product.ProductCount = model.ProductCount;
                    product.ProductPrice = model.ProductPrice;
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
                   
        }


        /// <summary>
        /// Переход к странице удаления сведений о продукции
        /// </summary>
        [HttpGet]
        public IActionResult Delete([FromServices] AppDbContext context, int ID)
            => View(context.ProductInfos.Find(ID));


        /// <summary>
        /// Вып. опер. удаления
        /// </summary>
        [HttpPost]
        public IActionResult Delete([FromServices] AppDbContext context, int ID, ProductInfo model)
        {
            var product = context.ProductInfos.Find(ID);
            context.ProductInfos.Remove(product);
            context.SaveChanges();
            return RedirectToAction("Index");
        }



        
    }
}

