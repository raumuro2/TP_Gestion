using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Gestion.Acceso_a_datos
{
    public static class SQLQueries
    {
        public static readonly string GetRegistro = @"
        SELECT p.Id AS Id, 
               p.Nombre AS Nombre, 
               p.Apellidos AS Apellidos, 
               p.Casa AS Casa, 
               p.Fecha_registro AS FechaRegistro, 
               p.Fecha_baja AS FechaBaja, 
               p.Fecha_ultima_alta AS FechaUltimaAlta, 
               p.En_alta AS EnAlta,
               t.Id AS TipoId, 
               t.Tipo AS Tipo
        FROM Persona p
        INNER JOIN Tipo_Persona t ON p.Id_tipo_vecino = t.Id
        WHERE (@TipoId IS NULL OR t.Id = @TipoId)
        AND (@EnAlta IS NULL OR p.En_alta = @EnAlta)
        AND (@FiltroNombre IS NULL OR 
        CONCAT(p.Nombre, ' ', p.Apellidos) LIKE '%' + @FiltroNombre + '%')";

        public static readonly string GetTiposPersona = @"SELECT Id as TipoId,Tipo  FROM Tipo_Persona";

        public static readonly string UpdateEnAlta = $@"UPDATE Persona
                                         SET En_alta = @EnAlta
                                         WHERE Id = @Id";

        public static readonly string GetCuotas = $@"
        SELECT 
               c.Anio AS Anio,
               c.Fecha AS Fecha,
               c.ID AS ID,
               p.Id AS IdPersona, 
               p.Nombre AS Nombre, 
               p.Apellidos AS Apellidos, 
               p.Casa AS Casa, 
               p.Fecha_registro AS FechaRegistro, 
               p.Fecha_baja AS FechaBaja, 
               p.Fecha_ultima_alta AS FechaUltimaAlta, 
            CASE 
                WHEN EXISTS (
                    SELECT 1 
                    FROM Cuota c2 
                    WHERE c2.Id_vecino = p.ID AND c2.Anio = @AnioConsulta
                ) THEN 1
                ELSE 0
            END AS Pagado
        FROM 
            Persona p inner join Cuota c on p.ID = c.Id_vecino 
        WHERE 
            p.En_alta = 1
            AND (@FiltroNombre IS NULL OR 
            CONCAT(p.Nombre, ' ', p.Apellidos) LIKE '%' + @FiltroNombre + '%')
            AND (@FiltroPagado IS NULL OR 
            CASE 
                WHEN EXISTS (
                    SELECT 1 
                    FROM Cuota c2 
                    WHERE c2.Id_vecino = p.ID AND c2.Anio = @AnioConsulta
                ) THEN 1
                ELSE 0
            END = @FiltroPagado);";

        public static readonly string GetProductos = $@"
            SELECT 
	            p.ID as Id,
	            Nombre,
	            Precio,
	            Precio_Iva as PrecioIva,
	            Seg_stock as SegStock,
	            Fecha_creacion as FechaCreacion,
	            Fecha_modificacion as FechaModificacion,
	            s.ID as IdStock,
	            s.Stock as StockProducto
            FROM Producto p
            INNER JOIN Stock s on p.ID = s.Id_producto
            ;";

    }
}
