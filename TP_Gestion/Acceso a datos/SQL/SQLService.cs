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

        public bool CrearVenta(Venta venta, out long id)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("Id_persona", venta.IdPersona);
            data.Add("Total", venta.Total);
            data.Add("Total_iva", venta.TotalIva);
            data.Add("Fecha", venta.FechaVenta);
            data.Add("Observaciones", venta.Observaciones);
            if (_dbConnection.Insert("Venta", data, out id))
            {
                foreach (var linea in venta.LineaVentaList)
                {
                    CrearLineaVenta(id, linea);
                }
                CrearMovContable(new MovContable(id, null, venta.TotalIva, false, venta.FechaVenta));
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

        //DELETES
        public bool EliminarVecino(int id)
        {
            return _dbConnection.Delete("Persona", "ID", id, out string errorMessage);
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

        public IEnumerable<Cuota> SelectCuotasAnual(int anioConsulta)
        {
            var parameters = new { AnioConsulta = anioConsulta };

            return _dbConnection.ExecuteQuery<Cuota>(SQLQueries.GetCuotas,parameters);
        }
    }
}
