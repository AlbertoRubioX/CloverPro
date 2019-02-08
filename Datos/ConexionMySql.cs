using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace Datos
{
    public class ConexionMySql
    {

        public static string cadenaConexion = string.Empty;//ConfigurationManager.ConnectionStrings["Cloverprod_Connection"].ToString();
        
        private static void Cadena()
        {

            cadenaConexion = "Server=10.129.240.27;Port=3306;Database='cpro';Uid='root';Pwd='root'";
            
        }
        public static string CadenaConexion()
        {
            Cadena();
            return cadenaConexion;
        }
    }
}
