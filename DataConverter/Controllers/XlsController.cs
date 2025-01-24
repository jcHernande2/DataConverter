using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Http.Features;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using DataConverter.Models;
using System.IO;
using ExcelDataReader;

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
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                var bytes = new byte[0];
                using (var ms = new MemoryStream())
                {
                    file.XlsFile.CopyTo(ms);
                    bytes = ms.ToArray();
                    using (MemoryStream streamWrite = new MemoryStream())
                    {
                        
                            using (var reader = ExcelReaderFactory.CreateReader(ms))
                            {
                                //conexion.Open();
                                string update = string.Empty;
                                string insert = string.Empty;
                                StreamWriter sw = new StreamWriter(streamWrite);
                                do
                                {
                                    int row = 0;
                                    string FieldIn = string.Empty;
                                    while (reader.Read()) //Each ROW
                                    {
                                        if (reader.Name.Trim() == file.Sheet)
                                        {
                                            sw.WriteLine($"INSERT INTO");
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                } while (reader.NextResult());
                            }
                        
                    }
                }
                HttpResponseMessage msg = new HttpResponseMessage(HttpStatusCode.Accepted);
                return this.Ok(new { });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return this.BadRequest();
            }
        }

    }
}
