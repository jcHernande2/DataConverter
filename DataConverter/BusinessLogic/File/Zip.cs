using System.IO.Compression;
using System.Text;

namespace DataConverter.BusinessLogic.File
{
    public class Zip
    {
        private byte[] archivoZipBytes;
        
        public Zip()
        {

        }
        public byte[] GetBytesZip()
        {
            return archivoZipBytes;
        }
        public void AddEntries(Dictionary<string, string> textData)
        {
            try
            {
                using (var memoryStreamZip = new MemoryStream())
                {
                    using (var zipArchive = new ZipArchive(memoryStreamZip, ZipArchiveMode.Update, true))
                    {
                        foreach (var data in textData)
                        {
                            // Crear un Stream de ejemplo (MemoryStream con datos de texto)
                            byte[] textBytes = Encoding.UTF8.GetBytes(data.Value);
                            using (MemoryStream memoryStream = new MemoryStream(textBytes))
                            {
                                // Crear la entrada ZIP
                                ZipArchiveEntry entry = zipArchive.CreateEntry($"{data.Key}", CompressionLevel.Optimal);

                                // Obtener el Stream de la entrada ZIP y copiar los datos
                                using (Stream entryStream = entry.Open())
                                {
                                    memoryStream.CopyTo(entryStream);
                                }
                            }
                        }
                        Console.WriteLine("Archivo ZIP creado exitosamente.");
                    }
                    this.archivoZipBytes = memoryStreamZip.ToArray();
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        public void Dispose()
        {
        }
    }
}
