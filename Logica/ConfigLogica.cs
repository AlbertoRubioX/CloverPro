using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class ConfigLogica
    {
        public string Directorio { get; set; }
        public string Servidor { get; set; }
        public int Puerto { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }
        public string SSL { get; set; }
        public string HTML { get; set; }
        public string CorreoSal { get; set; }
        public string CorreoDest { get; set; }
        public string CodigoBloq { get; set; }
        public string IndCorreo { get; set; }
        public string LeyendaRec { get; set; }
        public string EmpPrinter { get; set; }
        public string EmpSerial { get; set; }
        public string NotaLup { get; set; }
        public string FormatoLup { get; set; }
        public string OmiteLup { get; set; }
        public string OmiteEti { get; set; }
        public string ServerCp { get; set; }
        public string TipoCp { get; set; }
        public string BasedCp { get; set; }
        public string UserCp { get; set; }
        public string ClaveCp { get; set; }
        public string ServerOrb { get; set;}
        public string TipoOrb { get; set; }
        public string BasedOrb { get; set; }
        public string UserOrb { get; set; }
        public string ClaveOrb { get; set; }
        public string PuertoOrb { get; set; }
        public string RpoOrbis { get; set; }
        public string BuscaModelo { get; set; }
        public string DupRpoMod { get; set; }
        public string AgregaEst { get; set; }
        public string CodigoEmp { get; set; }
        public string SolicitaOper { get; set; }
        public string CodigoBueno { get; set; }
        public string AlerPreSetup { get; set; }
        public int MinPreSetup { get; set; }
        public string AlerHoraSetup { get; set; }
        public string HoraSetup { get; set; }
        public string AlerDuraSetup { get; set; }
        public int MinDuraSetup { get; set; }
        public string DirecRMA { get; set; }
        public string FileRMA { get; set; }
        public string DirectMP { get; set; }
        public string FileTimeDif { get; set; }
        public string AutoTimeDif { get; set; }
        public string HoraTimeDif { get; set; }
        public string TipoHrTime { get; set; }
        public string Setupxtd { get; set; }
        public string SetupDura { get; set; }
        public int SetupTurno { get; set; }
        public int SetupMin { get; set; }
        public string SetupIni1t { get; set; }
        public string SetupIni2t { get; set; }
        public string SetupSoruce { get; set; }
        public string Kanban { get; set; }
        public string KanbanDiect { get; set; }
        public string KanbanFile { get; set; }
        public byte KanbanMins { get; set; }
        public string KanbanStart { get; set; }
        public string KanbanEnd { get; set; }
        public string KanbanAttach { get; set; }
        public string KanbanAttFile { get; set; }
        public string Globals { get; set; }
        public string GlobStart { get; set; }
        public string GlobEnd { get; set; }
        public byte GlobalsMins { get; set; }
        public byte SwitchKanGloMins { get; set; }
        public string SetupMax { get; set; }
        public byte MinSetupMax { get; set; }
        public string SecuenciaRPO { get; set; }
        public string OmiteSecEst { get; set; }
        public string OmiteSecLinea { get; set; }
        public int AlmKanHrIni { get; set; }
        public int AlmKanHrFin { get; set; }
        public string AlmCorte1 { get; set; }
        public string AlmCorte2 { get; set; }
        public string AlmCorte3 { get; set; }
        public string AlmIndCorte4 { get; set; }
        public string AlmCorte4 { get; set; }
        public string AlmKanban { get; set; }
        public int AlmMeta1 { get; set; }
        public int AlmMeta2 { get; set; }
        public int AlmMeta3 { get; set; }
        public string VisualAidRoot { get; set; }
        public string VALineup { get; set; }
        public static int Guardar(ConfigLogica config)
        {
            string[] parametros = { "@Directorio", "@Servidor", "@Puerto", "@Usuario", "@Password", "@CorreoSal", "@SSL", "@HTML", "@CorreoDest", "@CodBloq", "@IndCorreo", "@LeyendaRec", "@EmpPrinter", "@EmpSerial","@NotaLp","@FormatoLp","@OmiteValLp", "@OmiteValEt", "@Server_cp", "@Tipo_cp", "@Based_cp", "@User_cp", "@Clave_cp", "@Server_orb", "@Tipo_orb", "@Based_orb", "@User_orb", "@Clave_orb", "@Puerto_orb", "@RpoOrbis", "@BuscaModelo", "@DupDifMod", "@AgregaEstacion", "@CodigoComp", "SoliOper", "CodigoB", "@AlerPreSetup", "@MinSetup", "@AlerHoraSetup", "@HoraSetup", "@AlerDuraSetup", "@MinDuraSetup", "@DirecRMA", "@FileNameRMA", "@DirecMP", "@FileTimeDif", "@CargAutTD", "@HoraTimeDif", "@TipoTimeDif", "@Setupxtd","@SetupDura", "@SetupTurno", "@SetupMin", "@SetupDur1t", "@SetupDur2t", "@SetupSource", "@Kanban", "@KanDirec", "@KanFile", "@KanStart", "@KanEnd", "@KanMins", "@Global", "@GlobStart", "@GlobEnd", "@GlobMins", "@AlerSetupMax", "@MinSetupMax", "@SecuenciaRPO", "@OmiteSecEst", "@OmiteSecLinea", "@AlmKanban", "@AlmKbHrIni", "@AlmKbHrFin", "@AlmCorte1", "@AlmCorte2", "@AlmCorte3", "@AlmMeta1", "@AlmMeta2", "@AlmMeta3", "@AlmIndCte4", "@AlmCorte4", "@VisualAidRoot", "@VALineup" };
            return AccesoDatos.Actualizar("sp_mant_config", parametros, config.Directorio, config.Servidor, config.Puerto, config.Usuario, config.Password, config.CorreoSal, config.SSL, config.HTML, config.CorreoDest, config.CodigoBloq, config.IndCorreo, config.LeyendaRec, config.EmpPrinter, config.EmpSerial, config.NotaLup,config.FormatoLup,config.OmiteLup, config.OmiteEti, config.ServerCp, config.TipoCp, config.BasedCp, config.UserCp, config.ClaveCp, config.ServerOrb, config.TipoOrb, config.BasedOrb, config.UserOrb, config.ClaveOrb, config.PuertoOrb, config.RpoOrbis, config.BuscaModelo, config.DupRpoMod, config.AgregaEst, config.CodigoEmp, config.SolicitaOper, config.CodigoBueno,config.AlerPreSetup, config.MinPreSetup, config.AlerHoraSetup, config.HoraSetup, config.AlerDuraSetup, config.MinDuraSetup, config.DirecRMA, config.FileRMA, config.DirectMP,config.FileTimeDif, config.AutoTimeDif, config.HoraTimeDif, config.TipoHrTime, config.Setupxtd,config.SetupDura, config.SetupTurno, config.SetupMin, config.SetupIni1t, config.SetupIni2t, config.SetupSoruce, config.Kanban, config.KanbanDiect, config.KanbanFile, config.KanbanStart, config.KanbanEnd, config.KanbanMins, config.Globals, config.GlobStart, config.GlobEnd, config.GlobalsMins, config.SetupMax, config.MinSetupMax, config.SecuenciaRPO, config.OmiteSecEst,config.OmiteSecLinea, config.AlmKanban, config.AlmKanHrIni, config.AlmKanHrFin, config.AlmCorte1, config.AlmCorte2, config.AlmCorte3, config.AlmMeta1, config.AlmMeta2, config.AlmMeta3, config.AlmIndCorte4, config.AlmCorte4, config.VisualAidRoot, config.VALineup);
        }

        public static DataTable Consultar()
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_config");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }
        public static bool VerificaCodigoBloq(string _asCodigo)
        {
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_config WHERE codigo_bloq = '" + _asCodigo + "'";
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

        public static bool VerificaOmiteValidaEti()
        {
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_config WHERE omiteval_eti = '1'";
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
        public static bool VerificaOmiteValidaLup()
        {
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_config WHERE omiteval_lineup = '1'";
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
        public static bool VerificaRpoOrbis()
        {
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_config WHERE rpo_orbis = '1'";
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
        public static string VerificaBuscaModelo()
        {
            string sTipo = string.Empty;
            try
            {
                string sQuery;
                sQuery = "SELECT busca_modelo FROM t_config WHERE clave = '01'";
                DataTable datos = AccesoDatos.Consultar(sQuery);
                if (datos.Rows.Count != 0)
                    sTipo = datos.Rows[0]["busca_modelo"].ToString();
            }
            catch
            {
                return "";
            }

            return sTipo;
        }
        public static bool VerificaDupModelo()
        {
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_config WHERE dup_rpomod = '1'";
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
        public static bool VerificaAgregaEstacion()
        {
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_config WHERE agesta_lineup = '1'";
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

        public static bool VerificaCapturaDuracion()
        {
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_config WHERE cap_durasetup = '1'";
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
        public static bool VerificaCodigoEmp(string asCodigo)
        {
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_config WHERE cod_desbcomp = '"+asCodigo+"'";
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
        public static bool VerificaSecuenciaRPO()
        {
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_config WHERE rpo_validasecuencia = '1'";
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
    }
}
