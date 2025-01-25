using System.ComponentModel.DataAnnotations;

namespace DataConverter.Models.Request
{
    public class FileData
    {

        [Required]
        public IFormFile XlsFile { get; set; }

        [Required]
        public IFormFile JsonFile { get; set; }
    }
}
