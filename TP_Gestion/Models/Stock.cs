using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Gestion.Models
{
    public class Stock
    {
        public int IdStock { get; set; }
        public long IdProducto { get; set; }
        public decimal StockProducto { get; set; }
    }
}
