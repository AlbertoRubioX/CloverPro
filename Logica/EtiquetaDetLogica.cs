using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class EtiquetaDetLogica
    {
        public string Planta { get; set; }
        public long Folio { get; set; }
        public int Consec { get; set; }
        public string RPO { get; set; }
        public string Barcode { get; set; }
        public string HoraEntra { get; set; }
        public string HoraSale { get; set; }
        public string SinUtil { get; set; }
        public DateTime FechaSale { get; set; }
        public string Defecto { get; set; }
        public string Codigo { get; set; }
        public string Linea { get; set; }
        public string Turno { get; set; }
        public string Usuario { get; set; }
        public static int Guardar(EtiquetaDetLogica eti)
        {
            string[] parametros = { "@Folio", "@Planta", "@Linea", "@Turno", "@Etiqueta", "@RPO", "@FechaS", "@IndDefecto", "@Codigo", "@Usuario" };
            return AccesoDatos.Actualizar("sp_mant_etidet", parametros, eti.Folio, eti.Planta, eti.Linea, eti.Turno, eti.Barcode, eti.RPO, eti.FechaSale, eti.Defecto, eti.Codigo, eti.Usuario);
        }

        public static DataTable Consultar(EtiquetaDetLogica eti)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_etidet WHERE planta = '"+eti.Planta+"' and linea = '"+eti.Linea+"' and turno = '"+eti.Turno+"' and rpo = '"+eti.RPO+"' ");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static bool Verificar(EtiquetaDetLogica eti)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_etidet WHERE planta = '" + eti.Planta + "' and linea = '" + eti.Linea + "' and turno = '" + eti.Turno + "' and etiqueta = '" + eti.Barcode + "' ");
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

        public static DataTable Listar(EtiquetaDetLogica eti)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT planta,folio,consec,rpo,barcode as Etiqueta,linea as Linea,hora_entra as [Hora Inicia],hora_sale as [Hora Termina],sin_util as [Sin Utilizar],sin_defecto as Defecto,codigo1 as [Código 1],codigo2 as [Código 2],codigo3 as [Código 3] FROM t_etidet WHERE planta = '" + eti.Planta+"' AND rpo = '" + eti.RPO + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }
    }
}
