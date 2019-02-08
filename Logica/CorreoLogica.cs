using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class CorreoLogica
    {
        public int Folio { get; set; }
        public string Estado { get; set; }
        public string Asunto { get; set; }
        public string FolioRef { get; set; }
        public string Proceso { get; set; }
        public string Mensaje { get; set; }
        public string Destino { get; set; }
        public string Tipo { get; set; }
        public static int Guardar(CorreoLogica mail)
        {
            string[] parametros = { "@Folio", "@Estado", "@Asunto", "@FolioRef", "@Proceso"};
            return AccesoDatos.Actualizar("sp_mant_correo", parametros, mail.Folio, mail.Estado, mail.Asunto, mail.FolioRef, mail.Proceso);
        }

        public static int GuardarBody(CorreoLogica mail)
        {
            string[] parametros = { "@Folio", "@Mensaje"};
            return AccesoDatos.Actualizar("sp_mant_correo_body", parametros, mail.Folio, mail.Mensaje);

        }

        public static int GuardarDest(CorreoLogica mail)
        {
            string[] parametros = { "@Folio", "@Correo", "@Tipo" };
            return AccesoDatos.Actualizar("sp_mant_correo_dest", parametros, mail.Folio, mail.Destino, mail.Tipo);

        }
        public static DataTable ConsultarPend()
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_correo where estado = 'P' order by folio");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultarFolio(CorreoLogica mail)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_correo where folio = " + mail.Folio + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultarRef(CorreoLogica mail)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_correo where estado = 'P' AND folio_ref = '" + mail.FolioRef + "' AND proceso = '"+mail.Proceso+"'");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultaDest(int aiFolio, string asTipo)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_correo_dest where folio = " + aiFolio+" and tipo = '"+asTipo+"'");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultaDestAux(string asPlanta, string asLinea, string asTurno)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT us.correo,'C' FROM t_suplinea sp inner join t_usuario us on sp.supervisor = us.usuario WHERE sp.planta = '"+asPlanta+"' AND sp.linea = '"+asLinea+"' and sp.turno = "+asTurno+" UNION select correo, 'T' from t_usuario where area = 'AUX' AND planta = '"+asPlanta+"' and turno = "+asTurno+" AND correo is not null and correo > ''");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultaBody(int aiFolio)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_correo_body where folio = " + aiFolio);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }
    }
}
