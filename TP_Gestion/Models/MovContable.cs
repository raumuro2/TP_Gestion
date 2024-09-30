﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Gestion.Models
{
    public class MovContable
    {
        public MovContable(long? idVenta, long? idCompra, decimal total, bool borron, DateTime fecha)
        {
            IdVenta = idVenta;
            IdCompra = idCompra;
            Total = total;
            Borron = borron;
            Fecha = fecha;
        }

        public int Id { get; set; }
        public long? IdVenta { get; set; }
        public long? IdCompra { get; set; }
        public decimal Total { get; set; }
        public bool Borron { get; set; }
        public DateTime Fecha { get; set; }
    }
}
