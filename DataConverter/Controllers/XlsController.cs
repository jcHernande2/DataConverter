using System.Text;
using Microsoft.AspNetCore.Mvc;
using DataConverter.BusinessLogic.Parser;
using DataConverter.Models.Request;

namespace DataConverter.Controllers
{

    public class XlsController : Controller
    {
        // GET: XlsController
        public ActionResult Index()
        {
            return View();
        }

        // GET: XlsController/sql
        [HttpGet]
        [Route("XlsController/Base64")]
        [RequestSizeLimit(20485760)]
        public ActionResult GetSql([FromForm] FileData file)
        {
            try
            {
               var reader =new ReaderXls(file);
                reader.ExtractData();
                var fileBytes = reader.GetStreamText();
                var metadata = new
                {
                    NombreArchivo = $"{DateTime.Now:yyyyMMdd}_script.sql",
                    Tipo = "Texto plano",
                    FechaGeneracion = DateTime.Now
                };

                var response = new
                {
                    Archivo = Convert.ToBase64String(fileBytes),
                    Metadata = metadata
                };

                return Ok(response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return this.BadRequest();
            }
        }
        // GET: XlsController/text
        [HttpGet]
        [Route("XlsController/text")]
        [RequestSizeLimit(20485760)]
        public ActionResult GetText([FromForm] FileData file)
        {
            try
            {
                var reader = new ReaderXls(file);
                reader.ExtractData();
                var fileBytes = reader.GetStreamText();
                return File(fileBytes, "text/plain", $"{DateTime.Now:yyyyMMdd}_script.sql");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return this.BadRequest();
            }
        }
        // GET: XlsController/text
        [HttpGet]
        [Route("XlsController/zip")]
        [RequestSizeLimit(50485760)]
        public ActionResult GetZip([FromForm] FileData file)
        {
            try
            {
                var reader = new ReaderXls(file);
                reader.ExtractData();
                var bytesZip = reader.GetStreamZip();
                return File(bytesZip, "application/zip", "scripts.zip");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return this.BadRequest();
            }
        }

    }
}
