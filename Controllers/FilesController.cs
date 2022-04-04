using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

using System;
using System.Threading.Tasks;
 
namespace Mvc_Apteka.Controllers
{

    /// <summary>
    /// Расширяет контроллер методами работы с файлами
    /// </summary>    
    public class FilesController: Controller
    {

        public class FileMessageModel
        {
            public string FileName { get; set; }
            public string ContentType { get; set; }
            public byte[] FileData { get; set; }
        }


       
        
        /// <summary>
        /// Передача файла на скачивание
        /// </summary>
        /// <param name="FileName">имя файла</param>
        /// <param name="ContentType">тип контента</param>
        /// <param name="FileData">бинарные данные</param>        
        public IActionResult Download(string FileName, string ContentType, byte[] FileData)
        {
            // "Content-Disposition: attachment; filename="filename.jpg""
            var contentDisposition = new ContentDispositionHeaderValue("attachment");
            contentDisposition.SetHttpFileName(FileName);
            Response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();

            var logger=LoggerFactory.Create(options => options.AddConsole()).CreateLogger(GetType().Name);
            logger.LogInformation(contentDisposition.ToString());
            logger.LogInformation("Size: " + FileData.Length);
            return File(FileData, ContentType);
        }



        /// <summary>
        /// Возвраащет имя файла загрузки
        /// request
        /// => Content-Type=multipart/form-data
        /// => Resource-Name=index.json
        /// => Mime-Type=applciation/json
        /// </summary>        
        public async Task<FileMessageModel> Upload(string contentType, long maxSize)
        {
            foreach (var header in this.HttpContext.Request.Headers)
                Console.WriteLine($"{header.Key}={header.Value}");
            if (HttpContext.Request.ContentType == "multipart/form-data")
            {

                string resourceName = Request.Headers["Resource-Name"];
                string mimeType = Request.Headers["Mime-Type"];
                if (mimeType != contentType ) 
                {
                    throw new Exception("Неверный формат файла: " + contentType);
                }

                long? length = this.HttpContext.Request.ContentLength;
                if (length != null)
                {
                    if (maxSize < length)
                    {
                        throw new Exception("Размер файла превышает максимально допустимый размер: "+maxSize);
                    }
                    byte[] buffer = new byte[(long)length];
                    await this.HttpContext.Request.Body.ReadAsync(buffer, 0, (int)length);

                    return new FileMessageModel() { 
                        FileName = resourceName,
                        ContentType = mimeType,
                        FileData = buffer
                    };
                }
            }
            throw new Exception("Не удалось прочитать файл");
        }        
    }
}
