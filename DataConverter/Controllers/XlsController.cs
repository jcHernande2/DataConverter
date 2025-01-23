using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Http.Features;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using DataConverter.Models;
using System.IO;

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
                var bytes = new byte[0];
                using (var ms = new MemoryStream())
                {
                    file.XlsFile.CopyTo(ms);
                    bytes = ms.ToArray();
                }

               HttpResponseMessage msg = new HttpResponseMessage(HttpStatusCode.Accepted);
                return this.Ok("");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return this.BadRequest();
            }
        }

    }
}
