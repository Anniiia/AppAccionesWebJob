using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoNetCore.Models
{
    [Table("ACCION")]
    public class Accion
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }
        [Column("NOMBRE")]
        public string Nombre { get; set; }
        [Column("ULTIMO")]
        public string Ultimo { get; set; }
        [Column("MAXIMO")]
        public string Maximo { get; set; }
        [Column("MINIMO")]
        public string Minimo { get; set; }
        [Column("CAMBIO")]
        public string Cambio { get; set; }
        [Column("CAMBIOPORC")]
        public string CambioPorcentaje { get; set; }
        //[Column("ID")]
        // public int Id { get; set; }
        [Column("FECHA")]
        public DateTime Fecha { get; set; }
    }
}