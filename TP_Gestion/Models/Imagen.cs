using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Gestion.Models
{
    public class Imagen
    { 
        public int Id { get; set; }
        public byte[] Image { get; set; }
        public string Nombre { get; set; }
        public string Ruta { get; set; }
    }
}
