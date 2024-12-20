using System.ComponentModel.DataAnnotations;

namespace SqlGenerator.Models
{
    public class FileData
    {
        public string Campo { get; set; }

        [Required]
        public IFormFile XlsFile { get; set; }

        public IFormFile JsonFile { get; set; }
    }
}
