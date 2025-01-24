using System.ComponentModel.DataAnnotations;

namespace DataConverter.Models
{
    public class FileData
    {
        public string Sheet { get; set; }

        [Required]
        public IFormFile XlsFile { get; set; }

        public IFormFile JsonFile { get; set; }
    }
}
