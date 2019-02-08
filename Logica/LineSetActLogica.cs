using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class LineSetActLogica
    {
        public long Folio { get; set; }
        public int Consec { get; set; }
        public string Actividad { get; set; }
        public string Estatus { get; set; }
        public string Comentario { get; set; }
        public string Usuario { get; set; }
        public string EntUsuario { get; set; }
        public string TerUsuario { get; set; }
        public string CorreoDest { get; set; }
        public string Asignado { get; set; }
        public static int Guardar(LineSetActLogica det)
        {
            string[] parametros = { "@Folio", "@Consec", "@Actividad", "@Estatus", "@Comentario", "@Usuario" };
            return AccesoDatos.Actualizar("sp_mant_lineseact", parametros, det.Folio,det.Consec, det.Actividad, det.Estatus, det.Comentario, det.Usuario );
        }

        public static DataTable Consultar(LineSetActLogica det)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_lineseact WHERE folio = "+det.Folio+" and consec = "+det.Consec+" ");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultarArea(LineSetActLogica det)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_lineseact WHERE folio = " + det.Folio + " and consec = " + det.Consec + " and actividad = '"+det.Actividad+"'");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultaFolio(LineSetActLogica det)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_lineseact WHERE folio = " + det.Folio + " ");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultaAsigando(LineSetActLogica det)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_lineseact WHERE folio = " + det.Folio + " and consec = "+det.Consec+" and actividad = '"+det.Actividad+"' and ind_asignado = '1' and correo_dest is not null ");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }


        public static bool VerificarAsigando(LineSetActLogica det)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_lineseact WHERE folio = "+det.Folio+" and consec = "+det.Consec+" and actividad = '"+det.Actividad+"'  ");
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

        public static DataTable Listar(LineSetActLogica det)
        {
            DataTable datos = new DataTable();
            try
            {
                string sSql = "SELECT act.folio,act.consec,act.actividad as CLAVE,are.descrip as Actividad,act.estatus,CASE act.estatus WHEN 'E' THEN 'EN ESPERA' WHEN 'R' THEN 'ENTERADO' WHEN 'T' THEN 'TERMINADA' WHEN 'N' THEN 'N/A' ELSE '' END as [Estatus Act],act.comentario as Comentario,act.u_id " +
                "FROM t_lineseact act INNER JOIN t_areas are on act.actividad = are.area " + 
                "WHERE act.folio ="+det.Folio+" and act.consec ="+det.Consec+" ";
                datos = AccesoDatos.Consultar(sSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static bool ActualizaEstatus(LineSetActLogica act)
        {
            try
            {
                DateTime dt = DateTime.Now;

                string sQuery = string.Empty;
                if (act.Estatus == "N")
                    sQuery = "UPDATE t_lineseact SET estatus = '"+act.Estatus+"',comentario='" + act.Comentario + "',u_id = '"+act.Usuario+"',f_id = '"+dt.ToString()+"' WHERE folio = " + act.Folio + " and consec = " + act.Consec + " and actividad = '"+act.Actividad+"'";
                if (act.Estatus == "L")
                    sQuery = "UPDATE t_lineseact SET estatus = '" + act.Estatus + "',comentario='" + act.Comentario + "',u_ent = '" + act.EntUsuario + "',f_ent = '" + dt.ToString() + "' WHERE folio = " + act.Folio + " and consec = " + act.Consec + " and actividad = '" + act.Actividad + "'";
                if (act.Estatus == "I")
                    sQuery = "UPDATE t_lineseact SET estatus = '" + act.Estatus + "',comentario='" + act.Comentario + "',u_ter = '" + act.TerUsuario + "',f_ter = '" + dt.ToString() + "' WHERE folio = " + act.Folio + " and consec = " + act.Consec + " and actividad = '" + act.Actividad + "'";
                if (act.Estatus == "E")
                    sQuery = "UPDATE t_lineseact SET estatus = '" + act.Estatus + "',u_id = '"+act.Usuario+"', f_id = '"+dt.ToString()+"' WHERE folio = " + act.Folio + " and consec = " + act.Consec + " and actividad = '" + act.Actividad + "'";

                if (AccesoDatos.Borrar(sQuery) != 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public static bool AsignarActividad(LineSetActLogica act)
        {
            try
            {
                DateTime dt = DateTime.Now;
                string sQuery = string.Empty;
                sQuery = "UPDATE t_lineseact SET ind_asignado = '"+act.Asignado+"',correo_dest = '" + act.CorreoDest + "', u_asig = '"+act.Usuario+"',f_asig = '"+dt.ToString()+"' WHERE folio = " + act.Folio + " and consec = " + act.Consec + " and actividad = '" + act.Actividad + "'";

                if (AccesoDatos.Borrar(sQuery) != 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
