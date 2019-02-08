﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class LineSetupDetLogica
    {
        public DateTime Fecha { get; set; }
        public long Folio { get; set; }
        public int Consec { get; set; }
        public string Planta { get; set; }
        public string Linea { get; set; }
        public string RPO { get; set; }
        public string Modelo { get; set; }
        public double Cantidad { get; set; }
        public string RPOSig { get; set; }
        public string ModeloSig { get; set; }
        public string IniProg { get; set; }
        public string FinProg { get; set; }
        public string IniReal { get; set; }
        public string FinReal { get; set; }
        public double Duracion { get; set; }
        public double Retraso { get; set; }
        public string Comentario { get; set; }
        public string CancelComent { get; set; }
        public string Urgente { get; set; }
        public string Estatus { get; set; }
        public string Turno { get; set; }
        public string Planner { get; set; }
        public string IniciaMP { get; set; }
        public string FinalMP { get; set; }
        public double DuraMP { get; set; }
        public string RpoMP { get; set; }
        public string Usuario { get; set; }
        public static int Guardar(LineSetupDetLogica det)
        {
            string[] parametros = { "@Folio", "@Consec", "@Planta", "@Linea", "@Turno", "@RPO", "@Modelo", "@Cantidad", "@RPO_sig", "@Modelo_sig", "@Inicio_prog", "@Final_prog", "@Inicio_real", "@Final_real", "@Duracion", "@RetMin", "@Comentario", "@CancelComent", "@Urgente", "@Estatus", "@Planner", "@Usuario" };
            return AccesoDatos.Actualizar("sp_mant_linesedet", parametros, det.Folio,det.Consec, det.Planta, det.Linea, det.Turno, det.RPO,det.Modelo,det.Cantidad,det.RPOSig,det.ModeloSig,det.IniProg,det.FinProg,det.IniReal,det.FinReal, det.Duracion, det.Retraso, det.Comentario, det.CancelComent, det.Urgente, det.Estatus, det.Planner, det.Usuario);
        }

        public static DataTable Consultar(LineSetupDetLogica det)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_linesedet WHERE folio = "+det.Folio+" and consec = "+det.Consec+" ");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static bool Verificar(LineSetupDetLogica det)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT sd.* FROM t_linesedet sd INNER JOIN t_lineset ls ON sd.folio = ls.folio WHERE cast(ls.fecha as date) = cast('"+det.Fecha+"' as date) AND sd.planta = '"+det.Planta+ "' and sd.linea = '" + det.Linea+ "' and sd.rpo = '" + det.RPO+ "' and sd.modelo = '" + det.Modelo+ "' and sd.rpo_sig = '" + det.RPOSig+ "' and sd.modelo_sig = '" + det.ModeloSig+ "' and sd.estatus <> 'C'");
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

        public static DataTable Listar(LineSetupDetLogica det)
        {
            DataTable datos = new DataTable();
            try
            {
                string sSql = "SELECT * FROM vw_linesedet_vista where folio=" + det.Folio+" ORDER BY 1,2";
                datos = AccesoDatos.Consultar(sSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }
        public static bool Eliminar(LineSetupDetLogica det)
        {
            try
            {
                string sQuery = "DELETE FROM t_linesedet WHERE folio = " + det.Folio + " and consec = "+det.Consec+"";
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
        public static bool ActualizaHoraIni(LineSetupDetLogica det)
        {
            try
            {
                DateTime dt = DateTime.Now;
                string sQuery = "UPDATE t_linesedet SET inicio_prog='"+det.IniProg+"',u_hora = '"+det.Usuario+"',f_hora = '"+dt.ToString()+"' WHERE folio = " + det.Folio + " and consec = " + det.Consec + "";
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

        public static bool ActualizaDura(LineSetupDetLogica det)
        {
            try
            {
                DateTime dt = DateTime.Now;
                string sQuery = "UPDATE t_linesedet SET duracion=" + det.Duracion + ",u_id = '" + det.Usuario + "',f_id = '" + dt.ToString() + "' WHERE folio = " + det.Folio + " and consec = " + det.Consec + "";
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
        public static bool ActualizaEst(LineSetupDetLogica det)
        {
            try
            {
                DateTime dt = DateTime.Now;
                string sQuery = "UPDATE t_linesedet SET estatus='" + det.Estatus+ "',u_id = '" + det.Usuario + "',f_id = '" + dt.ToString() + "' WHERE folio = " + det.Folio + " and consec = " + det.Consec + "";
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
        public static bool ActualizaComent(LineSetupDetLogica det)
        {
            try
            {
                DateTime dt = DateTime.Now;
                string sQuery = "UPDATE t_linesedet SET comentario='" + det.Comentario + "',u_id = '" + det.Usuario + "',f_id = '" + dt.ToString() + "' WHERE folio = " + det.Folio + " and consec = " + det.Consec + "";
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

        public static bool Cancelar(LineSetupDetLogica det)
        {
            try
            {
                string sQuery = "UPDATE t_linesedet SET estatus='C',motivo_cancel = '"+det.CancelComent+"' WHERE folio = " + det.Folio + " and consec = " + det.Consec + "";
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

        public static bool ActualizaDuraMP(LineSetupDetLogica det)
        {
            try
            {
                DateTime dt = DateTime.Now;
                string sQuery = "UPDATE t_linesedet SET inicio_mp = '"+det.IniciaMP+"', final_mp = '"+det.FinalMP+"', dura_mp=" + det.DuraMP + ", rpo_mp = '"+det.RpoMP+"',  u_id = '" + det.Usuario + "',f_id = '" + dt.ToString() + "' WHERE folio = " + det.Folio + " and consec = " + det.Consec + "";
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
