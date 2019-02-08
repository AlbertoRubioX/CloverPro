using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class UsuarioLogica
    {
        public string Usuario { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Activo { get; set; }
        public string Area { get; set; }
        public string Planta { get; set; }
        public string SupGral { get; set; }
        public string Modulo { get; set; }
        public string Turno { get; set; }
        public string Proceso { get; set; }
        public string Operacion { get; set; }
        public string Permiso { get; set; }
        public string UserId { get; set; }
        public static int Guardar(UsuarioLogica user)
        {
            string[] parametros = { "@Usuario", "@Nombre", "@Correo", "@Planta", "@Area", "@Activo", "@Modulo", "@Turno", "@SupGral", "@UserId" };
            return AccesoDatos.Actualizar("sp_mant_usuario", parametros, user.Usuario, user.Nombre,user.Correo,user.Planta,user.Area,user.Activo, user.Modulo, user.Turno, user.SupGral, user.UserId);
        }

        public static DataTable Consultar(UsuarioLogica user)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT us.* FROM t_usuario us INNER JOIN t_plantas pl on us.planta = pl.planta WHERE us.usuario = '" + user.Usuario + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

       

        public static bool Verificar(UsuarioLogica user)
        {
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_usuario WHERE usuario = '" + user.Usuario + "'";
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

        public static DataTable Listar()
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_usuario where usuario <> 'ADMINP'");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ListarSuper()
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_usuario where area = 'SUP' and usuario <> 'ADMINP' ORDER BY nombre");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ListarSupGral()
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_usuario where area = 'SPG' ORDER BY nombre");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ListarSuperParam(UsuarioLogica user)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_usuario where planta = '"+user.Planta+"' and turno = '"+user.Turno+"' and area = '"+user.Area+"' and usuario <> 'ADMINP' and activo = '1' ORDER BY nombre");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ListarPlanners(UsuarioLogica user)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_usuario where turno = '" + user.Turno + "' and area = 'PLA' and usuario <> 'ADMINP' and activo = '1' ORDER BY nombre");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ListarArea(UsuarioLogica user)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_usuario where area = '"+user.Area+"' and usuario <> 'ADMINP'");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ListarArea()
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_area");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static bool AntesDeEliminar(UsuarioLogica user)
        {
            try
            {
                string sQuery;

                DataTable datos = new DataTable();

                sQuery = "SELECT * FROM t_etiqueta WHERE usuario = '" + user.Usuario + "'";
                datos = AccesoDatos.Consultar(sQuery);
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

        public static bool Eliminar(UsuarioLogica user)
        {
            try
            {
                string sQuery = "DELETE FROM t_usuaper WHERE usuario = '" + user.Usuario + "'";
                if (AccesoDatos.Borrar(sQuery) == 0)
                    return false;
                
                sQuery = "DELETE FROM t_usuario WHERE usuario = '" + user.Usuario + "'";
                if (AccesoDatos.Borrar(sQuery) != 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public static int GuardarPermiso(UsuarioLogica usuario)
        {
            string[] parametros = { "@Usuario", "@Proceso", "@Operacion", "@Permiso" };
            return AccesoDatos.Actualizar("sp_mant_permisos", parametros, usuario.Usuario, usuario.Proceso, usuario.Operacion, usuario.Permiso);
        }
        public static bool VerificarPermiso(string asUsuario, string asOperacion)
        {
            if (asUsuario == "ADMINP")
                return true;

            try
            {
                string sQuery = "select * from t_usuaper where usuario = '" + asUsuario + "' and operacion = '" + asOperacion + "' and permiso = '1'";
                DataTable datos = AccesoDatos.Consultar(sQuery);
                if (datos.Rows.Count != 0)
                {
                    //AccesoLogica.RegistraUser(asUsuario, asOperacion.Substring(0, 4), asOperacion);
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public static DataTable ListarPermisos(string as_proceso)
        {
            DataTable datos = new DataTable();
            try
            {
                string sQuery = "";
                sQuery = "select * from t_mod03 where proceso = '" + as_proceso + "' order by operacion asc";
                datos = AccesoDatos.Consultar(sQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ListarAccesos(int ai_nivel)
        {
            DataTable datos = new DataTable();
            try
            {
                string sQuery = "";
                if (ai_nivel == 1)
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

        public static DataTable ListarProcesos(string as_modulo)
        {
            DataTable datos = new DataTable();
            try
            {
                string sQuery = "";
                sQuery = "select * from t_mod02 where modulo = '" + as_modulo + "' order by proceso asc";
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
