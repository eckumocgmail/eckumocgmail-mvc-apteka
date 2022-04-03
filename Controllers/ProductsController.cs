using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Mvc_Apteka.Controllers.Xml;
using Mvc_Apteka.Entities;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Mvc_Apteka.Controllers
{
    public class ProductsController : ProductsSearchController
    {
        public override IActionResult Index() => Redirect("/Products/Search");

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

        [HttpGet]
        public IActionResult Create([FromServices] AppDbContext context)
            => View(new ProductInfo());

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


        [HttpGet]
        public IActionResult Edit([FromServices] AppDbContext context, int ID)
            => View(context.ProductInfos.Find(ID));


        
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


        [HttpGet]
        public IActionResult Delete([FromServices] AppDbContext context, int ID)
            => View(context.ProductInfos.Find(ID));

        [HttpPost]
        public IActionResult Delete([FromServices] AppDbContext context, int ID, ProductInfo model)
        {
            var product = context.ProductInfos.Find(ID);
            context.ProductInfos.Remove(product);
            context.SaveChanges();
            return RedirectToAction("Index");
        }


        public IActionResult Info([FromServices] AppDbContext context, int ID)
            => View(context.ProductInfos.Find(ID));
        
    }
}

