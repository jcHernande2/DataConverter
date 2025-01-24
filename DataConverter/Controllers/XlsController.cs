using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Http.Features;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using DataConverter.Models;
using System.IO;
using DataConverter.BusinessLogic.Parser;

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
        [Route("XlsController/sql")]
        public ActionResult GetSql([FromForm] FileData file)
        {
            try
            {
               var reader =new ReaderXls();
                byte[] fileBytes = Encoding.UTF8.GetBytes(reader.GetSql(file));

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

    }
}
