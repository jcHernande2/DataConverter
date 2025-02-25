using System.IO.Compression;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using DataConverter.BusinessLogic.Models;
using DataConverter.Models.Request;
using ExcelDataReader;
using DataConverter.BusinessLogic.File;

namespace DataConverter.BusinessLogic.Parser
{
    public class ReaderXls
    {
        private readonly FileData file;
        private Dictionary<string, string> Entries;

        public ReaderXls(FileData file) 
        {
            this.file = file;
          
            Entries= new Dictionary<string, string>();

        }
        public ReaderXls(string name) { }   
        public void ExtractData()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
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
                        int blockRow = 0;
                        string FieldIn = string.Empty;
                        while (reader.Read()) //Each ROW
                        {
                            if (reader.Name.Trim() == json.Sheet)
                            {
                                var fieldValues=this.GetRowValues(reader,json);
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
                                    blockRow++;
                                }
                                if(blockRow >= json.RowMax)
                                {
                                    this.Entries.Add($"{json.NameTable}_{this.Entries.Count}", contenido);
                                    contenido = string.Empty;
                                    blockRow = 0;
                                }
                                
                                row++;
                                
                            }
                            else
                            {
                                break;
                            }
                        }
                        if (!string.IsNullOrEmpty(contenido))
                        {
                            this.Entries.Add($"{json.NameTable}_{this.Entries.Count}", contenido);
                            break;
                        }
                    } while (reader.NextResult());
                }
            }
        }
        private Dictionary<string, string> GetRowValues(IExcelDataReader reader, FieldConfig json)
        {
            Dictionary<string, string> fieldValues = new Dictionary<string, string>();
            object[] values= new object[json.FieldNames.Count];
            int res=reader.GetValues(values);
            if (values?.Where(v => v != null)?.Count() > 0)
            {
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
            }
            return fieldValues;

        }

        public byte[] GetStreamZip()
        {
            var zipFile = new Zip();
            zipFile.AddEntries(this.Entries);
            return zipFile.GetBytesZip();
        }
        public Dictionary<string, string> GetEntries()
        {
            return this.Entries;
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
