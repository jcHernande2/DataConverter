using DataConverter.Models;
using ExcelDataReader;

namespace DataConverter.BusinessLogic.Parser
{
    public class ReaderXls
    {
        public ReaderXls() { }
        public ReaderXls(string name) { }   
        public string GetSql(FileData file)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var bytes = new byte[0];
            string contenido = string.Empty;
            using (var ms = new MemoryStream())
            {
                file.XlsFile.CopyTo(ms);
                bytes = ms.ToArray();

                using (var reader = ExcelReaderFactory.CreateReader(ms))
                {
                    do
                    {
                        int row = 0;
                        string FieldIn = string.Empty;
                        while (reader.Read()) //Each ROW
                        {
                            if (reader.Name.Trim() == file.Sheet)
                            {
                                contenido = contenido + ($"INSERT INTO\n");
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
    }
}
