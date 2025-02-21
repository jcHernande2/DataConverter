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
        [Route("XlsController/Base64/{operation}")]
        public ActionResult GetSql([FromForm] FileData file,string operation)
        {
            try
            {
               var reader =new ReaderXls(file);
                byte[] fileBytes = Encoding.UTF8.GetBytes(reader.GetSql(operation));

                var metadata = new
                {
                    NombreArchivo = "script.sql",
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
        [Route("XlsController/text/{operation}")]
        public ActionResult GetText([FromForm] FileData file, string operation)
        {
            try
            {
                var reader = new ReaderXls(file);
                byte[] fileBytes = Encoding.UTF8.GetBytes(reader.GetSql(operation));
                return File(fileBytes, "text/plain", "script.sql");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return this.BadRequest();
            }
        }

    }
}
