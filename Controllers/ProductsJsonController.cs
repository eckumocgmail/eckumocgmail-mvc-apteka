using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc_Apteka.Controllers
{
    public class ProductsJsonController: FilesController
    {
 
        public IActionResult DownloadJson([FromServices] AppDbContext appDbContext)
        {
            string json = JsonConvert.SerializeObject(appDbContext.ProductInfos.ToList());
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            return Download("products.json", "application/json", bytes);
        }


        public async Task UploadJson([FromServices] AppDbContext appDbContext)
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

                        if (ContentType != "application/json")
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
            catch(Exception ex)
            {
                await Task.FromException(ex);
            }
        }
    }
}
