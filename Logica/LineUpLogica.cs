using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class LineUpLogica
    {
        public long Folio { get; set; }
        public DateTime Fecha { get; set; }
        public string Planta { get; set; }
        public string Linea { get; set; }
        public string Modelo { get; set; }
        public string Core { get; set; }
        public string RPO { get; set; }
        public string Controla { get; set; }
        public string Autoriza { get; set; }
        public string ModeloCtrl { get; set; }
        public string Turno { get; set; }
        public string Usuario { get; set; }
        public string Duplicado { get; set; }
        public long FolioDup { get; set; }
        public string UsuarioDup { get; set; }
        public string ModeloDup { get; set; }
        public string RpoDup { get; set; }
        public string RevStd { get; set; }
        public string ModRev { get; set; }
        public string BloqPPH { get; set; }
        public static int Guardar(LineUpLogica lin)
        {
            string[] parametros = { "@Folio", "@Fecha", "@Planta", "@Linea", "@Modelo", "@RPO", "@Core", "@Control", "@Autorizo", "@ModeloCtrl", "@Turno", "@IndDuplic", "@FolioDup", "@UserDup", "@ModeloDup", "@RpoDup", "@RevStd", "@ModeloRev", "@BloqPPH", "@Usuario" };
            return AccesoDatos.Actualizar("sp_mant_lineup", parametros, lin.Folio, lin.Fecha, lin.Planta, lin.Linea, lin.Modelo, lin.RPO, lin.Core, lin.Controla, lin.Autoriza, lin.ModeloCtrl, lin.Turno, lin.Duplicado, lin.FolioDup, lin.UsuarioDup, lin.ModeloDup, lin.RpoDup, lin.RevStd, lin.ModRev, lin.BloqPPH, lin.Usuario);
        }

        public static DataTable Listar()
        {
            DataTable datos = new DataTable();
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_lineup";
                datos = AccesoDatos.Consultar(sQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable Consultar(LineUpLogica linea)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_lineup WHERE folio = " + linea.Folio + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static bool Verificar(LineUpLogica linea)
        {
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_lineup WHERE folio = " + linea.Folio + "";
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
        public static bool VerificaModelo(LineUpLogica linea)
        {
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_lineup WHERE CAST(fecha AS date) = '" + linea.Fecha + "' AND linea = '"+linea.Linea+"' AND modelo = '"+ linea.Modelo+"' and turno = '"+linea.Turno+"'";
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
        public static bool VerificaRPO(LineUpLogica linea)
        {
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_lineup WHERE CAST(fecha AS date) = CAST('" + linea.Fecha + "' AS date) AND linea = '"+linea.Linea+"' AND rpo = '" + linea.RPO + "' and turno = '"+linea.Turno+"'";
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

        public static DataTable ConsultarRPOxFecha(LineUpLogica linea)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_lineup WHERE CAST(fecha as date) = CAST('"+linea.Fecha+"' AS date) AND rpo = '"+linea.RPO+"' AND turno = '"+linea.Turno+"' ");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultaRPO(LineUpLogica linea)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_lineup WHERE rpo = '" + linea.RPO + "' and turno = '" + linea.Turno + "' and cast(fecha as date) <= cast(getdate() as date) ORDER BY folio DESC");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }


        public static bool ModificaLinea(LineUpLogica line)
        {
            try
            {
                DateTime dt = DateTime.Now;
                string sQuery = "UPDATE t_lineup SET planta = '" + line.Planta + "',linea = '"+line.Linea+"', u_id = '" + line.Usuario + "', f_id = '" + dt.ToString() + "' WHERE folio = " + line.Folio + "";
                if (AccesoDatos.Borrar(sQuery) != 0)
                {
                    sQuery = "UPDATE t_lineupdet SET linea = '" + line.Linea + "', u_id = '" + line.Usuario + "', f_id = '" + dt.ToString() + "' WHERE folio = " + line.Folio + "";
                    if (AccesoDatos.Borrar(sQuery) != 0)
                        return true;
                    else
                        return false;
                }
                    
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public static bool Borrar(LineUpLogica linea)
        {
            bool bDelete = false;
            try
            {
                string sQuery = "DELETE FROM t_lineupdet WHERE folio = " + linea.Folio + " ";
                if (AccesoDatos.Borrar(sQuery) != 0)
                {
                    sQuery = "DELETE FROM t_lineup WHERE folio = " + linea.Folio + " ";
                    if (AccesoDatos.Borrar(sQuery) != 0)
                        bDelete = true;
                }
            }
            catch { throw; }

            return bDelete;
        }
    }
}
