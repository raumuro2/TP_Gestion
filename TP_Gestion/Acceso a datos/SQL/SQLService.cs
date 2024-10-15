using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP_Gestion.Models;

namespace TP_Gestion.Acceso_a_datos
{
    public class SQLService
    {
        private readonly DBConnection _dbConnection;
        public SQLService(DBConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        //CREATES
        public bool CrearVecino(Persona persona)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("Nombre", persona.Nombre);
            data.Add("Apellidos", persona.Apellidos);
            data.Add("Casa", persona.Casa);
            data.Add("Fecha_registro", persona.FechaRegistro);
            data.Add("Id_tipo_vecino", persona.Tipo_Persona.TipoId);
            data.Add("Fecha_baja", persona.FechaBaja);
            data.Add("fecha_ultima_alta", persona.FechaUltimaAlta);
            data.Add("En_alta", persona.EnAlta);

            return _dbConnection.Insert("Persona", data, out long id);
        }
        public bool CrearProducto(Producto producto)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();

            if (producto.Imagen != null)
            {
                if (CrearImagen(producto.Imagen, out long idImagen)) data.Add("ImagenID", idImagen);
            }

            data.Add("Nombre", producto.Nombre);
            data.Add("Precio", producto.Precio);
            data.Add("PrecioIva", producto.PrecioIva);
            data.Add("Fecha_creacion", producto.FechaCreacion);
            data.Add("Fecha_modificacion", DateTime.Now);
            data.Add("Baja", producto.Baja);
            data.Add("Seg_stock", producto.SegStock);

            bool ok = _dbConnection.Insert("Producto", data, out long id);
            if (ok)
            {
                producto.Stock.IdProducto = id;
                CrearStock(producto.Stock);
            }
            return ok;
        }
        public bool CrearStock(Stock stock)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("Id_producto", stock.IdProducto);
            data.Add("Stock", stock.StockProducto);
            return _dbConnection.Insert("Stock", data, out long id);
        }
        public bool CrearImagen(Imagen imagen, out long idImagen)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("Imagen", imagen.Image);
            return _dbConnection.Insert("Imagen", data, out idImagen);
        }
        public bool CrearVenta(Venta venta, out long id)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("Id_persona", venta.IdPersona);
            data.Add("Total", venta.Total);
            data.Add("Total_iva", venta.TotalIva);
            data.Add("Fecha", venta.FechaVenta);
            data.Add("Observaciones", venta.Observaciones);
            data.Add("Rectificativa", false);
            if (_dbConnection.Insert("Venta", data, out id))
            {
                foreach (var linea in venta.LineaVentaList)
                {
                    CrearLineaVenta(id, linea);
                }
                CrearMovContable(new MovContable(id, null, null,venta.TotalIva, false, venta.FechaVenta));
                return true;
            }
            else return false;
        }

        public bool CrearVentaRectificativa(Venta venta, out long id)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("Id_persona", venta.IdPersona);
            data.Add("Total", venta.Total);
            data.Add("Total_iva", venta.TotalIva);
            data.Add("Fecha", venta.FechaVenta);
            data.Add("Observaciones", venta.Observaciones); 
            data.Add("Rectificativa", true);
            data.Add("IdRectificada", venta.IdRectificada);

            if (_dbConnection.Insert("Venta", data, out id))
            {
                CrearMovContable(new MovContable(null, null, id, venta.TotalIva, false, venta.FechaVenta));
                return true;
            }
            else return false;
        }
        public bool CrearLineaVenta(long idVenta, LineaVenta linea)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("Id_venta", idVenta);
            data.Add("Cantidad", linea.Cantidad);
            data.Add("Precio_ud", linea.PrecioUd);
            data.Add("Precio_total", linea.PrecioTotal);
            data.Add("Precio_total_iva", linea.PrecioTotalIva);

            return _dbConnection.Insert("Linea_Venta", data, out long id);
        }
        public bool CrearMovContable(MovContable mov)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("Id_Venta", mov.IdVenta);
            data.Add("Id_Compra", mov.IdCompra);
            data.Add("Borron", mov.Borron);
            data.Add("Total", mov.Total);
            data.Add("Fecha", mov.Fecha);

            return _dbConnection.Insert("Mov_Contable", data, out long id);
        }
        public bool CrearCouta(Cuota cuota)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("Anio", cuota.Anio);
            data.Add("Fecha", cuota.Fecha);
            data.Add("Id_vecino", cuota.Id);
            data.Add("Id_venta", cuota.IdVenta);

            return _dbConnection.Insert("Cuota", data, out long id);
        }

        //UPDATES
        public bool ActualizarVecino(Persona persona)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("Nombre", persona.Nombre);
            data.Add("Apellidos", persona.Apellidos);
            data.Add("Casa", persona.Casa);
            data.Add("Fecha_registro", persona.FechaRegistro);
            data.Add("Id_tipo_vecino", persona.Tipo_Persona.TipoId);
            data.Add("Fecha_baja", persona.FechaBaja);
            data.Add("fecha_ultima_alta", persona.FechaUltimaAlta);
            data.Add("En_alta", persona.EnAlta);
            string whereClause = "Id = " + persona.Id;

            return _dbConnection.Update("Persona", data, whereClause, out string errorMessage);
        }

        public bool ActualizarEnAltaVecinos(List<Persona> personas)
        {
            return _dbConnection.UpdateMultiplePersonas(personas, out string errorMessage);
        }
        public bool ActualizarProducto(Producto producto)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("Nombre", producto.Nombre);
            data.Add("Precio", producto.Precio);
            data.Add("PrecioIva", producto.PrecioIva);
            data.Add("Fecha_creacion", producto.FechaCreacion);
            data.Add("Fecha_modificacion", DateTime.Now);
            data.Add("Baja", producto.Baja);
            data.Add("Seg_stock", producto.SegStock);
            string whereClause = "Id = " + producto.Id;
            bool ok = _dbConnection.Update("Persona", data, whereClause, out string errorMessage);
            if (ok)
            {
                if (producto.Stock != null) ActualizarStockProducto(producto.Stock);
                if (producto.Imagen != null) ActualizarImagenProducto(producto.Imagen);
            }
            return ok;
        }
        public bool ActualizarStockProducto(Stock stock)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("Stock", stock.StockProducto);
            string whereClause = "Id = " + stock.IdStock;

            return _dbConnection.Update("Stock", data, whereClause, out string errorMessage);
        }
        public bool ActualizarImagenProducto(Imagen imagen)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("Imagen", imagen.Image);
            string whereClause = "Id = " + imagen.Id;

            return _dbConnection.Update("Imagen", data, whereClause, out string errorMessage);
        }

        //DELETES
        public bool EliminarVecino(int id)
        {
            return _dbConnection.Delete("Persona", "ID", id, out string errorMessage);
        }
        public bool EliminarCuota(int id)
        {
            return _dbConnection.Delete("Cuota", "ID", id, out string errorMessage);
        }
        
        //SELECTS
        public IEnumerable<Tipo_Persona> SelectTiposPersona(object parameters = null)
        {
            
            return _dbConnection.ExecuteQuery<Tipo_Persona>(SQLQueries.GetTiposPersona);
        }

        public IEnumerable<Persona> SelectRegistroVecinos(int? tipoId, string filtroNombre, bool? enAlta)
        {
            var parameters = new
            {
                TipoId = tipoId, 
                FiltroNombre = filtroNombre, 
                EnAlta = enAlta 
            };
            return _dbConnection.ExecuteQuery<Persona, Tipo_Persona>(
                SQLQueries.GetRegistro,
                (persona, tipo) =>
                {
                    persona.Tipo_Persona = tipo; // Asigna el objeto Tipo_Persona a la persona
                    return persona;
                },
                parameters,
                splitOn: "TipoId" // Indica que se divide el mapeo en esta columna
            );
        }
        public IEnumerable<Producto> SelectVerProductos(string filtroNombre, decimal? precioMin, 
            decimal? precioMax, bool? baja)
        {
            var parameters = new
            {
                PrecioMin = precioMin,
                PrecioMax = precioMax,
                FiltroNombre = filtroNombre,
                BajaChecked = baja
            };
            return _dbConnection.ExecuteQuery<Producto, Stock>(
                SQLQueries.GetProductos,
                (producto, stock) =>
                {
                    producto.Stock = stock; 
                    return producto;
                },
                parameters,
                splitOn: "Id_producto" 
            );
        }
        public IEnumerable<Cuota> SelectCuotasAnual(int anioConsulta, string nombre, bool? pagado)
        {
            var parameters = new { 
                AnioConsulta = anioConsulta,
                FiltroNombre = nombre,
                FiltroPagado = pagado
            };

            return _dbConnection.ExecuteQuery<Cuota, Persona>(
                SQLQueries.GetCuotas,
                (cuota, persona) =>
                {
                    cuota.Persona = persona; // Asigna el objeto Tipo_Persona a la persona
                    return cuota;
                },
                parameters,
                splitOn: "IdPersona" // Indica que se divide el mapeo en esta columna
            );
        }
    }
}
