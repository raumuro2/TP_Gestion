using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Gestion.Models
{
    public class LineaVenta
    {
        public LineaVenta()
        {
        }

        public LineaVenta(long idVenta, long idProducto, int cantidad, decimal precioUd, 
            decimal precioTotal, decimal precioTotalIva)
        {
            IdVenta = idVenta;
            IdProducto = idProducto;
            Cantidad = cantidad;
            PrecioUd = precioUd;
            PrecioTotal = precioTotal;
            PrecioTotalIva = precioTotalIva;
        }

        public long Id { get; set; }
        public long IdVenta{ get; set; }
        public long IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUd { get; set; }
        public decimal PrecioTotal { get; set; }
        public decimal PrecioTotalIva { get; set; }

    }
}
