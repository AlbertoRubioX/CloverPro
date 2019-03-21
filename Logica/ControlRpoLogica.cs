using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class ControlRpoLogica
    {
        public DateTime Fecha { get; set; }
        public long Folio { get; set; }
        public int Consec { get; set; }
        public string Surte { get; set; }
        public string Etiqueta { get; set; }
        public string Locacion { get; set; }
        public string EtiquetaInterna { get; set; }
        public string LocacionInterna { get; set; }
        public string Almacen { get; set; }
        public string IndPrio { get; set; }
        public string Prioridad { get; set; }
        public string Entrega { get; set; }
        public string NombreOper { get; set; }
        public string IndPlanta { get; set; }
        public string Planta { get; set; }
        public string IndLinea { get; set; }
        public string Linea { get; set; }
        public string IndTurno { get; set; }
        public string Turno { get; set; }
        public string IndRPO { get; set; }
        public string RPO { get; set; }
        public string Modelo { get; set; }
        public int Cantidad { get; set; }
        public string IndEstatus { get; set; }
        public string Estatus { get; set; }
        public string IndArea { get; set; }
        public string Area { get; set; }
        public string WP { get; set; }
        public string EtiNota { get; set; }
        public string EtIntNota { get; set; }
        public string AlmNota { get; set; }
        public string IndFiltro { get; set; }
        public string Filtro { get; set; }
        public string ValorFiltro { get; set; }
        public string CargaInd { get; set; }
        public DateTime FechaGlob { get; set; }
        public string HoraComp { get; set; }
        public string Destino { get; set; }
        public string HoraEntrega { get; set; }
        public int CantGlobal { get; set; }
        public string Supervisor { get; set; }
        public string Nota { get; set; }
        public string ParteDetenido { get; set; }
        public double CantDetenido { get; set; }
        public string Usuario { get; set; }
        public string CargaParcial { get; set; }

        public static int Guardar(ControlRpoLogica rpo)
        {
            string[] parametros = { "@Folio", "@Consec", "@Planta", "@Linea", "@RPO", "@Modelo", "@Cantidad", "@Surte", "@Etiqueta", "@Almacen", "@Prioridad", "@Entrega", "@Turno", "@WP", "@CargaInd", "@FechaGlob", "@HrComp", "@Destino", "@HrEntrega", "@Supervisor", "@CantGlob", "@Nota", "@Usuario" };
            return AccesoDatos.Actualizar("sp_mant_rpo_actdet", parametros, rpo.Folio, rpo.Consec, rpo.Planta, rpo.Linea, rpo.RPO, rpo.Modelo, rpo.Cantidad, rpo.Surte, rpo.Etiqueta, rpo.Almacen, rpo.Prioridad, rpo.Entrega, rpo.Turno, rpo.WP, rpo.CargaInd, rpo.FechaGlob, rpo.HoraComp, rpo.Destino, rpo.HoraEntrega, rpo.Supervisor, rpo.CantGlobal, rpo.Nota, rpo.Usuario);
        }

        public static DataTable ListarSP(ControlRpoLogica rpo)
        {
            DataTable datos = new DataTable();
            try
            {
                string[] parametros = { "@Folio", "@Fecha", "@IndPlanta", "@Planta", "@IndLinea", "@Linea", "@IndTurno", "@Turno", "@IndRPO", "@RPO", "@IndEst", "@Estatus", "@IndFiltro", "@Filtro", "@ValorFiltro", "@Area" }; 
                datos = AccesoDatos.ConsultaSP("sp_mon_rpoact", parametros,rpo.Folio, rpo.Fecha, rpo.IndPlanta, rpo.Planta, rpo.IndLinea, rpo.Linea, rpo.IndTurno, rpo.Turno, rpo.IndRPO, rpo.RPO, rpo.IndEstatus, rpo.Estatus, rpo.IndFiltro, rpo.Filtro, rpo.ValorFiltro, rpo.Area);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable Consultar(ControlRpoLogica rpo)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_rpo_actdet WHERE folio = " + rpo.Folio + " and consec = " + rpo.Consec + " ");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultaRPO(ControlRpoLogica rpo)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_rpo_actdet WHERE folio = " + rpo.Folio + " and rpo = '" + rpo.RPO + "' ");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultarFolio(ControlRpoLogica rpo)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT max(folio) FROM t_rpo_actdet WHERE planta = '"+rpo.Planta+"' ");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }
        public static long ConsultarUltFolio(ControlRpoLogica rpo)
        {
            long lFolio = 0;
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT max(folio) FROM t_rpo_actdet WHERE planta = '" + rpo.Planta + "' ");
                if (datos.Rows.Count > 0)
                {
                    if (!long.TryParse(datos.Rows[0][0].ToString(), out lFolio))
                        return lFolio;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lFolio;
        }
        public static long ConsultarUltFolioRPO(ControlRpoLogica rpo)
        {
            long lFolio = 0;
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT max(folio) FROM t_rpo_actdet WHERE rpo = '" + rpo.RPO + "' ");
                if (datos.Rows.Count > 0)
                {
                    if (!long.TryParse(datos.Rows[0][0].ToString(), out lFolio))
                        return lFolio;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lFolio;
        }

        public static DataTable ConsultarFolioAnt(ControlRpoLogica rpo)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT max(ac.folio) FROM t_rpo_actdet dt INNER JOIN t_rpo_act ac ON dt.folio = ac.folio WHERE dt.planta = '" + rpo.Planta + "' and CAST(ac.fecha AS DATE) = CAST('"+rpo.Fecha+"' AS DATE) ");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }
        public static DataTable ConsultarUltRPO(ControlRpoLogica rpo)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT max(folio) FROM t_rpo_actdet WHERE rpo = '" + rpo.RPO+"'");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }
        public static DataTable CargarRPO(ControlRpoLogica rpo)
        {
            DataTable datos = new DataTable();
            try
            {
                string[] parametros = { "@Fecha", "@Planta", "@Usuario", "@Parcial" };
                datos = AccesoDatos.ConsultaSP("sp_act_rpoact", parametros, rpo.Fecha, rpo.Planta, rpo.Usuario, rpo.CargaParcial);
            }
            catch (Exception ex)
            {
                string srr = ex.ToString();
                throw ex;
            }

            return datos;
        }

        public static bool VerificaPlan(ControlRpoLogica rpo)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_rpo_act WHERE cast(fecha as date) = cast('"+rpo.Fecha+"' as date) and planta = '" + rpo.Planta + "'");
                if (datos.Rows.Count > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static DataTable ConsultarDisp(ControlRpoLogica rpo)
        {
            DataTable datos = new DataTable();
            try
            {
                string sSql = "SELECT COUNT(ac.consec),ub.config FROM t_rpo_actdet ac inner join t_rpo_ubidet ub on ac.planta = ub.planta and SUBSTRING(ac.locacion,1,2) = ub.ubicacion and SUBSTRING(ac.locacion,4,1) = ub.celda where ac.folio = "+rpo.Folio+" and ac.locacion = '"+rpo.Locacion+"' group by ub.config having COUNT(ac.consec) >= ub.config";
                datos = AccesoDatos.Consultar(sSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }
        public static bool ActualizaSurte(ControlRpoLogica rpo)
        {
            try
            {
                DateTime dt = DateTime.Now;
                string sQuery = "UPDATE t_rpo_actdet SET surte = '" + rpo.Surte + "' ,u_id = '" + rpo.Usuario + "', f_id = '" + dt.ToString() + "' WHERE folio = " + rpo.Folio + " and consec = " + rpo.Consec + "";
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
        public static bool ActualizaWP(ControlRpoLogica rpo)
        {
            try
            {
                DateTime dt = DateTime.Now;
                string sQuery = "UPDATE t_rpo_actdet SET pw = '" + rpo.WP + "', u_id = '" + rpo.Usuario + "', f_id = '" + dt.ToString() + "' WHERE folio = " + rpo.Folio + " and consec = " + rpo.Consec + "";
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

        public static bool ActualizaEti(ControlRpoLogica rpo)
        {
            try
            {
                DateTime dt = DateTime.Now;
                string sQuery = string.Empty;
                if (rpo.Etiqueta == "D")
                    sQuery = "UPDATE t_rpo_actdet SET etiqueta = '" + rpo.Etiqueta + "',eti_nota = '"+rpo.EtiNota+"',u_id = '" + rpo.Usuario + "', f_id = '" + dt.ToString() + "' WHERE folio = " + rpo.Folio + " and consec = " + rpo.Consec + "";
                else
                    sQuery = "UPDATE t_rpo_actdet SET etiqueta = '" + rpo.Etiqueta + "',entrega = '"+rpo.Entrega+"',nombre_oper = '"+rpo.NombreOper+ "', u_id = '" + rpo.Usuario + "', f_id = '" + dt.ToString() + "' WHERE folio = " + rpo.Folio + " and consec = " + rpo.Consec + "";

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
        public static bool ActualizaEtInt(ControlRpoLogica rpo)
        {
            try
            {
                DateTime dt = DateTime.Now;
                string sQuery = string.Empty;
                if (rpo.EtiquetaInterna == "D")
                    sQuery = "UPDATE t_rpo_actdet SET etiqueta_interna = '" + rpo.EtiquetaInterna + "',etint_nota = '" + rpo.EtIntNota + "',u_id = '" + rpo.Usuario + "', f_id = '" + dt.ToString() + "' WHERE folio = " + rpo.Folio + " and consec = " + rpo.Consec + "";
                else
                    sQuery = "UPDATE t_rpo_actdet SET etiqueta_interna = '" + rpo.EtiquetaInterna + "',entrega = '" + rpo.Entrega + "',nombre_oper = '" + rpo.NombreOper + "', u_id = '" + rpo.Usuario + "', f_id = '" + dt.ToString() + "' WHERE folio = " + rpo.Folio + " and consec = " + rpo.Consec + "";

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
        public static bool ActualizaLocal(ControlRpoLogica rpo)
        {
            try
            {
                DateTime dt = DateTime.Now;
                string sQuery = "UPDATE t_rpo_actdet SET locacion = '" + rpo.Locacion + "' ,u_id = '" + rpo.Usuario + "', f_id = '" + dt.ToString() + "' WHERE folio = " + rpo.Folio + " and consec = " + rpo.Consec + "";
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
        public static bool ActualizaLocalInt(ControlRpoLogica rpo)
        {
            try
            {
                DateTime dt = DateTime.Now;
                string sQuery = "UPDATE t_rpo_actdet SET locacion_etint = '" + rpo.LocacionInterna + "' ,u_id = '" + rpo.Usuario + "', f_id = '" + dt.ToString() + "' WHERE folio = " + rpo.Folio + " and consec = " + rpo.Consec + "";
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
        public static bool ActualizaAlma(ControlRpoLogica rpo)
        {
            try
            {
                DateTime dt = DateTime.Now;
                string sQuery = string.Empty;
                if (rpo.Almacen == "D")
                    sQuery = "UPDATE t_rpo_actdet SET almacen = '" + rpo.Almacen + "',alm_nota = '"+rpo.AlmNota+"',dete_nota = '"+rpo.ParteDetenido+"',dete_cant = "+rpo.CantDetenido+", u_dete = '" + rpo.Usuario + "', f_dete = '" + dt.ToString() + "' WHERE folio = " + rpo.Folio + " and consec = " + rpo.Consec + "";
                else
                    sQuery = "UPDATE t_rpo_actdet SET almacen = '" + rpo.Almacen + "', u_id = '" + rpo.Usuario + "', f_id = '" + dt.ToString() + "' WHERE folio = " + rpo.Folio + " and consec = " + rpo.Consec + "";

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

        public static bool ActualizaPrio(ControlRpoLogica rpo)
        {
            try
            {
                DateTime dt = DateTime.Now;
                string sQuery = "UPDATE t_rpo_actdet SET prioridad = '" + rpo.Prioridad + "' ,u_id = '" + rpo.Usuario + "', f_id = '" + dt.ToString() + "' WHERE folio = " + rpo.Folio + " and consec = " + rpo.Consec + "";
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
        public static bool ActualizaEntrega(ControlRpoLogica rpo)
        {
            try
            {
                DateTime dt = DateTime.Now;
                string sQuery = "UPDATE t_rpo_actdet SET entrega = '" + rpo.Entrega + "' ,u_id = '" + rpo.Usuario + "', f_id = '" + dt.ToString() + "' WHERE folio = " + rpo.Folio + " and consec = " + rpo.Consec + "";
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

        public static DataTable ConsultaEspera(ControlRpoLogica rpo)
        {
            DataTable datos = new DataTable();
            try
            {
                string sSql = "select count(*) from t_rpo_actdet where folio = "+rpo.Folio+" and estatus = 'E' and turno = '"+rpo.Turno+"'";
                datos = AccesoDatos.Consultar(sSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultaTurno(ControlRpoLogica rpo)
        {
            DataTable datos = new DataTable();
            try
            {
                string sSql = "select count(*) from t_rpo_actdet where folio = " + rpo.Folio + " and turno = '" + rpo.Turno + "'";
                datos = AccesoDatos.Consultar(sSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultaVista(ControlRpoLogica rpo)
        {
            DataTable datos = new DataTable();
            try
            {
                string sSql = "select sum(proceso) as PROCESO,sum(listo) as LISTO,sum(detenido) as DETENIDO,sum(espera) as ESPERA "+
                "from vw_rpo_dashalm where folio = "+rpo.Folio+" and cast(fecha as date) = cast('"+rpo.Fecha+"' as date) and turno = '"+rpo.Turno+"'";
                datos = AccesoDatos.Consultar(sSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }
        public static DataTable ConsultaVistaEti(ControlRpoLogica rpo)
        {
            DataTable datos = new DataTable();
            try
            {
                string sSql = "select sum(proceso) as PROCESO,sum(listo) as LISTO,sum(detenido) as DETENIDO,sum(entregado) as ENTREGADO " +
                "from vw_rpo_dasheti where folio = " + rpo.Folio + " and cast(fecha as date) = cast('" + rpo.Fecha + "' as date) and turno = '" + rpo.Turno + "'";
                datos = AccesoDatos.Consultar(sSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static string RastreaLocacion(ControlRpoLogica rpo)
        {
            string sVal = string.Empty;
            try
            {
                string sSql = "select locacion from t_rpo_actdet where rpo='"+rpo.RPO+"' and locacion is not null order by folio desc ";
                DataTable datos = AccesoDatos.Consultar(sSql);
                if (datos.Rows.Count > 0)
                    sVal = datos.Rows[0][0].ToString();
                        
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return sVal;
        }

        public static DataTable ValidaPendienteSP(ControlRpoLogica rpo)
        {
            DataTable datos = new DataTable();
            try
            {
                string[] parametros = { "@Folio", "@Consec", "@Linea", "@Estatus" };
                datos = AccesoDatos.ConsultaSP("sp_mon_rpoact_validapend", parametros, rpo.Folio, rpo.Consec, rpo.Linea, rpo.Estatus );
            
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static bool ConsultaPendTurno(ControlRpoLogica rpo)
        {
            try
            {
                int iCont = 0;
                DateTime dt = DateTime.Now;
                string sQuery = "SELECT COUNT(*) FROM t_rpo_actdet WHERE folio = " + rpo.Folio + " AND consec < "+rpo.Consec+" AND turno = '" + rpo.Turno + "' AND linea = '"+rpo.Linea+"' AND almacen is null";
                DataTable datos = AccesoDatos.Consultar(sQuery);
                if (datos.Rows.Count > 0)
                {
                    iCont = int.Parse(datos.Rows[0][0].ToString());
                    if(iCont > 0)
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

        public static bool ConsultaPendientes(ControlRpoLogica rpo)
        {
            try
            {
                int iCont = 0;
                DateTime dt = DateTime.Now;
                string sQuery = "SELECT COUNT(*) FROM t_rpo_actdet WHERE folio = " + rpo.Folio + " AND turno = '" + rpo.Turno + "' AND linea = '"+rpo.Linea+"' AND almacen is null ";
                DataTable datos = AccesoDatos.Consultar(sQuery);
                if (datos.Rows.Count > 0)
                {
                    iCont = int.Parse(datos.Rows[0][0].ToString());
                    if (iCont > 0)
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
    }
}
