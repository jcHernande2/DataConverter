using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using DataConverter.BusinessLogic.Models;
using DataConverter.Models.Request;
using ExcelDataReader;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataConverter.BusinessLogic.Parser
{
    public class ReaderXls
    {
        private readonly FileData file;
        public ReaderXls(FileData file) 
        {
            this.file = file;
        }
        public ReaderXls(string name) { }   
        public string GetSql()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var bytes = new byte[0];
            string contenido = string.Empty;
            using (var ms = new MemoryStream())
            {
                var msjson = new MemoryStream();
                file.JsonFile.CopyTo(msjson);
                file.XlsFile.CopyTo(ms);
                var json = JsonSerializer.Deserialize<FieldConfig>(this.MemoryStreamToText(msjson));
                

                using (var reader = ExcelReaderFactory.CreateReader(ms))
                {
                    do
                    {
                        int row = 0;
                        string FieldIn = string.Empty;
                        while (reader.Read()) //Each ROW
                        {
                            if (reader.Name.Trim() == json.Sheet)
                            {
                                var field0 = reader.GetValue(0) ?? "";
                                var field1 = reader.GetValue(1) ?? "";
                                var field2 = reader.GetValue(2) ?? "";
                                var field3 = reader.GetValue(3) ?? "";
                                var field4 = reader.GetValue(4) ?? "";
                               
                                if (row > 4 && !string.IsNullOrEmpty(field0?.ToString()) && !string.IsNullOrEmpty(field1?.ToString()))
                                {
                                    field0 = Regex.Replace(field0.ToString(), @"\r\n?|\n", "").Trim();
                                    field0 = field0.ToString().Replace('"', '“').Trim();
                                    field0 = field0.ToString().Replace("'", "“").Trim();
                                    field1 = Regex.Replace(field1.ToString(), @"\r\n?|\n", "").Trim();
                                    field1 = field1.ToString().Replace('"', '“').Trim();
                                    field1 = field1.ToString().Replace("'", "“").Trim();
                                    field2 = Regex.Replace(field2.ToString(), @"\r\n?|\n", "").Trim();
                                    field2 = field2.ToString().Replace('"', '“').Trim();
                                    field2 = field2.ToString().Replace("'", "\"").Trim();
                                    field3 = Regex.Replace(field3.ToString(), @"\r\n?|\n", "").Trim();
                                    field3 = field3.ToString().Replace('"', '“').Trim();
                                    field3 = field3.ToString().Replace("'", "“").Trim();
                                    field4 = Regex.Replace(field4.ToString(), @"\r\n?|\n", "").Trim();
                                    field4 = field4.ToString().Replace('"', '“').Trim();
                                    field4 = field4.ToString().Replace("'", "“").Trim();
                                   

                                    //field5 = !string.IsNullOrEmpty(field5.ToString()) ? $"'{field5}'" : "NULL";
                                    contenido = contenido + $"INSERT INTO [dbo].[c_NumPedimentoAduana]([c_Aduana],[Patente],[Ejercicio],[Cantidad],[FechaInicio],[FechaFin]) values('{field0}','{field1}','{field2}','{field3}','{field4}');\n";
                                }
                                row++;
                                
                            }
                            else
                            {
                                break;
                            }
                        }
                    } while (reader.NextResult());
                }
            }
            return contenido;
        }
        private string MemoryStreamToText(MemoryStream memoryStream)
        {
            memoryStream.Seek(0, SeekOrigin.Begin);

            using (StreamReader reader = new StreamReader(memoryStream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
