using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class LineCapaDetLogica
    {
        public long Folio { get; set; }
        public int Partida { get; set; }
        public int Consec { get; set; }
        public string Planta { get; set; }
        public string Linea { get; set; }
        public string Modelo { get; set; }
        public string Turno1 { get; set; }
        public string Turno2 { get; set; }
        public double Std1 { get; set; }
        public double Std2 { get; set; }
        public double Hc1 { get; set; }
        public double Hc2 { get; set; }
        public double HcTress1 { get; set; }
        public double HcTress2 { get; set; }
        public string TipoLinea { get; set; }
        public string Usuario { get; set; }

        public static int Guardar(LineCapaDetLogica line)
        {
            string[] parametros = { "@Folio", "@Partida", "@Consec", "@Planta", "@Linea", "@Modelo", "@Turno1", "@Std1", "@Hc1", "@Hc_t1", "@Turno2", "@Std2", "@Hc2", "@Hc_t2", "@TipoLinea", "@Usuario"};
            return AccesoDatos.Actualizar("sp_mant_linecapdet", parametros, line.Folio, line.Partida, line.Consec, line.Planta, line.Linea, line.Modelo, line.Turno1, line.Std1, line.Hc1, line.HcTress1, line.Turno2, line.Std2, line.Hc2, line.HcTress2, line.TipoLinea, line.Usuario );
        }

        public static DataTable Consultar(LineCapaDetLogica line)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_linecapdet WHERE folio = " + line.Folio + " and partida = "+line.Partida+" order by linea");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        
        public static DataTable ListarLinea(LineCapaDetLogica line)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT distinct linea_nav as Linea,'' as Modelo,0 as [STD 1 Turno],0 as [STD 2 Turno],0 as [STD HC],0 as factor,'1' as Activa FROM t_linea WHERE planta = '" + line.Planta + "' and linea_nav is not null and linea_nav > ''");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }
    }
}
