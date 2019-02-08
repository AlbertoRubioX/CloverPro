using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class EtiquetaEmpdetLogica
    {
        public int Folio { get; set; }
        public int Consec { get; set; }
        public string VendorCode { get; set; }
        public int ConsError { get; set; }
        public string IndAlerta { get; set; }
        public string MotivoBloq { get; set; }
        public string Usuario { get; set; }

        public static int Guardar(EtiquetaEmpdetLogica eti)
        {
            string[] parametros = { "@Folio", "@Consec", "@VendorCode", "@ConsError", "@IndAlerta", "MotivoBloq", "@Usuario" };
            return AccesoDatos.Actualizar("sp_mant_etiempdet", parametros, eti.Folio, eti.Consec, eti.VendorCode, eti.ConsError, eti.IndAlerta, eti.MotivoBloq, eti.Usuario);
        }

        public static DataTable Listar()
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_etiempdet");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable Consultar(EtiquetaEmpdetLogica eti)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_etiempdet where folio = " + eti.Folio + " and consec = " + eti.Consec + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }
        public static int ConsultaCons(EtiquetaEmpdetLogica eti)
        {
            DataTable datos = new DataTable();
            int iConsec = 0;
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_etiempdet where folio = " + eti.Folio + " and cons_err = " + eti.ConsError + "");
                if (datos.Rows.Count != 0)
                    iConsec = int.Parse(datos.Rows[0][1].ToString());
                else
                    iConsec = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return iConsec;
        }
    }
}
