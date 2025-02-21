namespace DataConverter.BusinessLogic.Models
{
    public class FieldConfig
    {
        public string Sheet { get; set; }

        public int StartRow { get; set; }

        public string NameTable {get; set;}

        public Dictionary<string, int> FieldNames { get; set; }
    }
}
