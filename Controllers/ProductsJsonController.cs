using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Mvc_Apteka.Entities;
using Mvc_Apteka.Models;

using Newtonsoft.Json;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc_Apteka.Controllers
{
    /// <summary>
    /// Реализация импорта-экспорта файлов в формате json
    /// </summary>
    public class ProductsJsonController: FilesController
    {
        private readonly AppDbContext appDbContext;

        public ProductsJsonController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }


        /// <summary>
        /// Экспорт файла с данными JSON
        /// </summary>
        public IActionResult DownloadJson([FromServices] AppDbContext appDbContext)
        {
            string json = JsonConvert.SerializeObject(appDbContext.ProductInfos.ToList());
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            return Download("products.json", "application/json", bytes);
        }


        /// <summary>
        /// Импорт файлов JSON
        /// </summary>
        public async Task<object> UploadJson( )
        {                         
            
            if (HttpContext.Request.ContentType == "multipart/form-data")
            {
                try
                {
                    var FileMessageModel = await Upload("applciation/json", 1024 * 1024 * 1024);
                    string text = Encoding.UTF8.GetString(FileMessageModel.FileData);
                    var records = JsonConvert.DeserializeObject<IEnumerable<ProductInfo>>(text);
                    foreach(var product in records)
                    {
                        ProductInfo info = appDbContext.GetProductInfo(product.ProductName);
                        if (info == null)
                        {
                            appDbContext.ProductInfos.Add(new ProductInfo()
                            {
                                ProductName = product.ProductName,
                                ProductCount = product.ProductCount,
                                ProductPrice = product.ProductPrice
                            });
                        }
                        else
                        {
                            ProductInfo p = appDbContext.GetProductInfo(product.ProductName);
                            if (appDbContext.Equals(p, product.ProductName, product.ProductPrice, product.ProductCount) == false)
                            {
                                p.ProductName = product.ProductName;
                                p.ProductCount = product.ProductCount;
                                p.ProductPrice = product.ProductPrice;
                            }
                        }
                    }
                    appDbContext.SaveChanges();

                    return MethodResult.FromResult(true);
                }
                catch (Exception ex)
                {
                    return MethodResult.FromException(ex);
                }
                     
            }
            else
            {
                return View();
            }                                            
        }
    }
}
