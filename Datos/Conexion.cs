using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace Datos
{
    public class Conexion
    {
        
        public static string cadenaConexion = ConfigurationManager.ConnectionStrings["Cloverprod_ConnectionDebug"].ToString();
        
        private static void Cadena()
        {
            if (string.IsNullOrEmpty(cadenaConexion))
                cadenaConexion = "Data Source=MEXI1328;Initial Catalog=cloverprod;Persist Security Info=True;User ID=sa;Password=saadmin";
        }
        public static string CadenaConexion()
        {
            Cadena();
            return cadenaConexion;
        }
    }
}
