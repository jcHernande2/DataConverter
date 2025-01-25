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
        public ActionResult GetSql([FromForm] FileData file)
        {
            try
            {
               var reader =new ReaderXls(file);
                byte[] fileBytes = Encoding.UTF8.GetBytes(reader.GetSql());

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
        [Route("XlsController/text")]
        public ActionResult GetText([FromForm] FileData file)
        {
            try
            {
                var reader = new ReaderXls(file);
                byte[] fileBytes = Encoding.UTF8.GetBytes(reader.GetSql());
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
