using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Gestion.Models
{
    public class Venta
    {
        public Venta()
        {
        }

        public Venta(long idPersona, decimal total, decimal totalIva, 
            DateTime fechaVenta, string observaciones, List<LineaVenta> lineaVentaList)
        {
            IdPersona = idPersona;
            Total = total;
            TotalIva = totalIva;
            FechaVenta = fechaVenta;
            Observaciones = observaciones;
            LineaVentaList = lineaVentaList;
        }

        public long Id { get; set; }
        public long IdPersona { get; set; }
        public decimal Total { get; set; }
        public decimal TotalIva { get; set; }
        public DateTime FechaVenta { get; set; }
        public string Observaciones{ get; set; }
        public List<LineaVenta> LineaVentaList { get; set; }

    }
}
