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
        
        public static string cadenaConexion = ConfigurationManager.ConnectionStrings["Cloverprod_Connection"].ToString();
        
        private static void Cadena()
        {
            if (string.IsNullOrEmpty(cadenaConexion))
                cadenaConexion = "mxni3-app-08\mxnilocalapps;Initial Catalog=cloverprod;Persist Security Info=True;User ID=Sa;Password=Admin.10";
        }
        public static string CadenaConexion()
        {
            Cadena();
            return cadenaConexion;
        }
    }
}
