namespace DataConverter.BusinessLogic.Models
{
    public class FieldConfig
    {
        public string Sheet { get; set; }

        public int StartRow { get; set; }

        public string NameTable {get; set;}

        public FieldRelationship FieldRelationship { get; set; }
    }
    public class FieldRelationship
    {
        public int Clave { get; set; }

        public int Descripcion { get; set; }

        public int ImpoExpo { get; set; }

        public int FechaInicioVigencia { get; set; }

        public int FechaFinVigencia { get; set; }
    }
}
