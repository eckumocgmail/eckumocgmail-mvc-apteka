using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System.Threading.Tasks;
 
namespace Mvc_Apteka.Controllers
{

    /// <summary>
    /// Расширяет контроллер методами работы с файлами
    /// </summary>
    public class FilesController: Controller
    {
        
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
        /// </summary>        
        public async Task<IFormFileCollection> Upload()
        {
            IFormCollection formCollection = await Request.ReadFormAsync();
            return formCollection.Files;
        }        
    }
}
