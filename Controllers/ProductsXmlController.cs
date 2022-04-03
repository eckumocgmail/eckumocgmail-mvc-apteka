using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

using Mvc_Apteka.Controllers.Xml;
using Mvc_Apteka.Entities;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Mvc_Apteka.Controllers
{

    /// <summary>
    /// Импорт-экспорт файлов формата XML
    /// </summary>
    public class ProductsXmlController: FilesController
    {

        /// <summary>
        /// Экспорт файла XML
        /// </summary>
        public IActionResult DownloadXml([FromServices] AppDbContext appDbContext)
        {
            string json = JsonConvert.SerializeObject(appDbContext.ProductInfos.ToList());
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            return Download("products.xml", "text/xml", bytes);
        }


        /// <summary>
        /// Импорт файла XML
        /// </summary>        
        public async Task UploadXml()
        {
            try
            {
                foreach (IFormFile file in await this.Upload())
                {
                    using (var stream = file.OpenReadStream())
                    {
                        string ContentType = file.ContentType;
                        byte[] FileData = new byte[stream.Length];
                        await stream.ReadAsync(FileData);
                        string FileName = file.FileName;

                        if (ContentType != "text/xml")
                        {
                            throw new System.Exception("ContentType должен быть application/json");
                        }
                        else
                        {
                            System.IO.File.WriteAllText(FileName, Encoding.UTF8.GetString(FileData));
                        }
                    }
                }
                await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                await Task.FromException(ex);
            }
        }


        /// <summary>
        /// Считывание данных из XML0-файла
        /// </summary>
        /// <param name="filename">путь к файлу</param>    
        public object InitXml([FromServices] AppDbContext context, [FromServices] IWebHostEnvironment env)
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
                    catalog.Products.Add(new DATA()
                    {
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
                    foreach (DATA record in next.Products)
                    {
                        
                        context.ProductInfos.Add(new ProductInfo()
                        {
                            ProductCatalogID = ProductCatalog.ID,
                            ProductName = record.NAME,
                            ProductCount = (int)Math.Floor(float.Parse(record.COUNT.Replace(".", ","))),
                            ProductPrice = float.Parse(record.PRICE.Replace(".", ","))
                        });
                    }
                    context.SaveChanges();
                    //context.AddOrUpdate(ProductCatalog);
                }

            }

            return DrugList;
        }

    }


    /// <summary>
    /// Первичные структуры 
    /// </summary>
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
}
