using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Gestion.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public decimal PrecioIva { get; set; }
        public bool SegStock { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion{ get; set; }
    }
}
