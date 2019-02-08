using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class LineCapacityLogica
    {
        public long Folio { get; set; }
        public int Partida { get; set; }
        public DateTime Fecha { get; set; }
        public string Planta { get; set; }
        public string Nota { get; set; }
        public double Rampeo { get; set; }
        public string Usuario { get; set; }

        public static int Guardar(LineCapacityLogica line)
        {
            string[] parametros = { "@Folio", "@Partida", "@Fecha", "@Planta", "@Rampeo", "@Usuario" };
            return AccesoDatos.Actualizar("sp_mant_linecap", parametros, line.Folio, line.Partida, line.Fecha, line.Planta, line.Rampeo, line.Usuario);
        }

        public static DataTable Consultar(LineCapacityLogica line)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_linecap WHERE folio = " + line.Folio + " order by modelo");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultarPartida(LineCapacityLogica line)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_linecap WHERE folio = " + line.Folio + " and partida = "+line.Partida+"");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultarParts(LineCapacityLogica line)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT partida FROM t_linecap WHERE folio = " + line.Folio + " ");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultarLinea(LineCapacityLogica line)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_modelohc WHERE planta = '" + line.Planta + "'  order by modelo");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

       

        public static DataTable ListarModelos(LineCapacityLogica line)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT modelo as Modelo,std1er as [STD 1 Turno],std2do as [STD 2 Turno],headcount as [STD HC],factor as Factor FROM t_modelohc WHERE folio = " + line.Folio + " and partida = "+line.Partida+" order by modelo");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }
       
        public static bool Verificar(LineCapacityLogica line)
        {
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_linecap WHERE folio = '" + line.Folio + "'";
                DataTable datos = AccesoDatos.Consultar(sQuery);
                if (datos.Rows.Count != 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public static long VerificarPta(LineCapacityLogica line)
        {
            try
            {
                long lFolio = 0;
                string sQuery;
                sQuery = "SELECT max(folio) FROM t_linecap WHERE planta = '" + line.Planta + "' and cast(fecha as date) = cast('"+line.Fecha+"' as date)";
                DataTable datos = AccesoDatos.Consultar(sQuery);
                if (datos.Rows.Count != 0)
                {
                    if(!long.TryParse(datos.Rows[0][0].ToString(),out lFolio))
                        lFolio = 0;
                }
                else
                    lFolio = 0;

                return lFolio;
            }
            catch
            {
                return 0;
            }
        }
        public static bool VerificarFolio(LineCapacityLogica line)
        {
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_linecap WHERE folio = '" + line.Folio + "'";
                DataTable datos = AccesoDatos.Consultar(sQuery);
                if (datos.Rows.Count != 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public static int VerificarPartida(LineCapacityLogica line)
        {
            try
            {
                int iPart = 0;
                string sQuery;
                sQuery = "SELECT max(partida) FROM t_linecap WHERE folio = " + line.Folio + " ";
                DataTable datos = AccesoDatos.Consultar(sQuery);
                if (datos.Rows.Count != 0)
                {
                    if (!int.TryParse(datos.Rows[0][0].ToString(), out iPart))
                        iPart = 0;
                    return iPart;
                }
                else
                    return 0;
            }
            catch
            {
                return 0;
            }
        }

        public static DataTable Listar()
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_linecap");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }
    }
}
