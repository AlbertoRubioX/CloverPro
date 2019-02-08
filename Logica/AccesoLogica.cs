using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Datos;

namespace Logica
{
    public class AccesoLogica
    {
        public static bool verificaAdmin(string as_usuario)
        {
            if (as_usuario == "ADMINP")
                return true;
            else
                return false;
        }

        public static bool verificaUsuario(string as_usuario)
        {
            try
            {
                DataTable datos = AccesoDatos.VerificarUsuario(as_usuario);
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

       
        public static DataTable traeDatosUsuario(string as_usuario)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.TraerUsuario(as_usuario);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static bool verificaAcceso(string as_Usuario)
        {
            if (as_Usuario == "ADMINP")
                return true;

            string sQuery;
            sQuery = "select * from t_usuario where usuario = '" + as_Usuario + "'";
            DataTable datos = new DataTable();
            datos = AccesoDatos.Consultar(sQuery);
            if (datos.Rows.Count != 0)
                return true;
            else
                return false;
        }

        public static DataTable listarAccesos(string as_Usuario)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.TraerPermisos(as_Usuario);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ListarMenu(string as_tipo)
        {
            DataTable datos = new DataTable();
            try
            {
                string sQuery = "";
                if (as_tipo == "M")
                    sQuery = "select * from t_mod01 order by modulo asc";
                else
                    sQuery = "select * from t_mod02 order by proceso asc";
                datos = AccesoDatos.Consultar(sQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }
    }
}
