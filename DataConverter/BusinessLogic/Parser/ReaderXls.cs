using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using DataConverter.BusinessLogic.Models;
using DataConverter.Models.Request;
using ExcelDataReader;

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
        public string GetSql(string operation)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
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
                                Dictionary<string, string> fieldValues = new Dictionary<string, string>();
                                foreach (var field in json.FieldNames)
                                {
                                    var value = reader?.GetValue(field.Value)?.ToString();

                                    if (value != null)
                                    {
                                        value = Regex.Replace(value.ToString(), @"\r\n?|\n", "").Trim();
                                        value = value.ToString().Replace('"', '“').Trim();
                                        value = value.ToString().Replace("'", "“").Trim();
                                    }
                                    value = !string.IsNullOrEmpty(value) ? $"'{value}'" : "NULL";
                                    fieldValues.Add(field.Key, value);
                                }
                                if (row > json.StartRow && fieldValues.Count()== json.FieldNames.Count())
                                {
                                    var values=string.Empty;
                                    var fields = string.Empty;
                                    if (json?.FieldsToUpdate?.Count > 0)
                                    {
                                        foreach (var field in json.FieldsToUpdate)
                                        {

                                            fields += !string.IsNullOrEmpty(fields) ? $",{field}={fieldValues[field]}" : $"{field}={fieldValues[field]}";

                                        }
                                        contenido = contenido + $"UPDATE [dbo].[{json.NameTable}] SET {fields} WHERE {json.FieldWhere}={fieldValues[json.FieldWhere]};\n";
                                    }                                    
                                    else
                                    {
                                        foreach (var value in fieldValues)
                                        {
                                            values += !string.IsNullOrEmpty(values) ? $",{value.Value}" : $"{value.Value}";
                                            fields += !string.IsNullOrEmpty(fields) ? $",[{value.Key}]" : $"[{value.Key}]";
                                        }
                                        contenido = contenido + $"INSERT INTO [dbo].[{json.NameTable}]({fields}) values({values});\n";
                                    }
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
