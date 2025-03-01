﻿namespace DataConverter.BusinessLogic.Models
{
    public class FieldConfig
    {
        public string Sheet { get; set; }

        public int StartRow { get; set; }

        public int RowMax { get; set; }

        public string NameTable {get; set;}

        public Dictionary<string, int> FieldNames { get; set; }

        public string FieldWhere { get; set; }

        public List<string> FieldsToUpdate { get; set; }
    }
}
