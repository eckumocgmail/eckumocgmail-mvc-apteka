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
    namespace Xml
    {
        /// <summary>
        /// Сведения о лекарственном препарате
        /// </summary>
        public class LS
        {
            public System.String MNN { get; set; }
            public System.Int32 LS_Id { get; set; }

            [XmlIgnore]
            public List<DATA> Products { get; set; } = new List<DATA> { };
        }


        /// <summary>
        /// Продажи лекарств
        /// </summary>
        public class DATA
        {
            public System.String NAME { get; set; }
            public System.String PRICE { get; set; }
            public System.String COUNT { get; set; }
            public System.Int32 LS_Id { get; set; }
        }
    }
    public class ProductsController: Controller
    {
        

        public IActionResult Index() => Redirect("/Products/Search");


        [HttpGet]
        public IActionResult Search([FromServices] AppDbContext context, [FromQuery]int PageNubmer=1, [FromQuery] int PageSize=10)
        {
            return View(context.ProductInfos.Skip((PageNubmer-1)*PageSize).Take(PageSize).ToList());
        }


        [HttpGet]
        public object OnInput([FromServices] AppDbContext context, [FromQuery] string value)
        {            
            HttpContext.Response.ContentType = "application/json";
            return new
            {
                Options = context.ProductInfos.Where(p => p.ProductName.ToUpper().IndexOf(value.ToUpper()) != -1).Select(p => p.ProductName).ToList()
            };
        }

        [HttpGet]
        public IEnumerable<ProductCatalog> Catalogs([FromServices] AppDbContext context, [FromQuery] int PageNubmer = 1, [FromQuery] int PageSize = 10)
        {
            return context.ProductCatalogs.Include(p=>p.Products).Skip((PageNubmer - 1) * PageSize).Take(PageSize).ToList();
        }



        public IActionResult ExportJson( [FromServices] AppDbContext appDbContext )
        {
            string json = JsonConvert.SerializeObject(appDbContext.ProductInfos.ToList());
            byte[] bytes = Encoding.UTF32.GetBytes(json);            
            return File(bytes, "text/json");
        }

     


        /// <summary>
        /// Считывание данных из XML0-файла
        /// </summary>
        /// <param name="filename">путь к файлу</param>    
        public object Import([FromServices]AppDbContext context, [FromServices] IWebHostEnvironment env)
        {            
           
            var DrugList = new List<LS>();
            string filepath = Path.Combine(
                env.ContentRootPath,
                "Resources",
                "input.xml"
            ).ToString();
            using (var stream = new StreamReader(System.IO.File.OpenRead(filepath)))
            {
                DataSet dataset = new DataSet();
                dataset.ReadXml(stream);
                foreach (DataRow row in dataset.Tables[0].Rows)
                {                   
                    DrugList.Add(new LS()
                    {
                        MNN = row[0].ToString(),
                        LS_Id = int.Parse(row[1].ToString())
                    });                     
                }
                foreach (DataRow row in dataset.Tables[1].Rows)
                {

                    int catalogId = int.Parse(row[3].ToString());
                    LS catalog = DrugList.Where(x => x.LS_Id == catalogId).FirstOrDefault();
                    catalog.Products.Add(new DATA() {
                        NAME = row[0].ToString(),
                        COUNT = row[2].ToString(),
                        PRICE = row[1].ToString()
                    });                    
                }
                foreach (LS next in DrugList)
                {
                    var ProductCatalog = new Entities.ProductCatalog()
                    {
                        ProductCatalogName = next.MNN,
                        ProductCatalogNumber = next.LS_Id
                    };
                    context.ProductCatalogs.Add(ProductCatalog);
                    context.SaveChanges();
                    foreach(DATA record in next.Products)
                    {
                        context.ProductInfos.Add(new ProductInfo() { 
                            ProductCatalogID = ProductCatalog.ID,
                            ProductName = record.NAME,
                            ProductCount = float.Parse(record.COUNT.Replace(".", ",")),
                            ProductPrice = float.Parse(record.PRICE.Replace(".", ","))
                        });
                    }
                    context.SaveChanges();
                    //context.AddOrUpdate(ProductCatalog);
                }
                
            }

            return DrugList;
        }

        /// <summary>
        /// Geeration-скрипт
        /// </summary>    
        public string GetPrimaryDatabaseSql()
        {
            return @"
            CREATE TABLE [DBO].[LS](
              MNN nvarchar(max)
              LS_Id int primary identity(1,1)
            )
            GO
            CREATE TABLE [DBO].[DATA](
              NAME nvarchar(max),
              PRICE nvarchar(max),
              COUNT nvarchar(max),
              LS_Id int primary identity(1,1)
            )
            GO
        ";
        }

        
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
