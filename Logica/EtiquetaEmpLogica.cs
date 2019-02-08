using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class EtiquetaEmpLogica
    {
        public int Folio { get; set; }
        public string Planta { get; set; }
        public string Linea { get; set; }
        public string RPO { get; set; }
        public string Modelo { get; set; }
        public string VendorCode { get; set; }
        public int Lote { get; set; }
        public int Saldo { get; set; }
        public int CantError { get; set; }
        public string Estacion { get; set; }
        public string Estatus { get; set; }
        public string Turno { get; set; }
        public string Usuario { get; set; }

        public static int Guardar(EtiquetaEmpLogica eti)
        {
            string[] parametros = { "@Folio", "@Planta", "@Linea", "@Estacion", "@VendorCode", "@Saldo", "@CantError", "@Estatus", "@Usuario" };
            return AccesoDatos.Actualizar("sp_mant_etiemp", parametros, eti.Folio, eti.Planta, eti.Linea, eti.Estacion, eti.VendorCode, eti.Saldo, eti.CantError, eti.Estatus, eti.Usuario);
        }

        public static DataTable Consultar(EtiquetaEmpLogica eti)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_etiemp where folio = "+eti.Folio);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }
        public static DataTable Listar()
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_etiemp");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }
    }
}
