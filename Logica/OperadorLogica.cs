using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class OperadorLogica
    {
        public string Operador { get; set; }
        public string Nombre { get; set; }
        public string Activo { get; set; }
        public string Turno { get; set; }
        public string Planta { get; set; }
        public string Linea { get; set; }
        public string Nivel { get; set; }
        public DateTime FechaIngre { get; set; }
        public string Usuario { get; set; }
        public static int Guardar(OperadorLogica Oper)
        {
            string[] parametros = { "@Empleado", "@Nombre", "@Activo", "@Turno", "@Planta", "@Linea", "@Nivel", "FechaIngre", "@Usuario" };
            return AccesoDatos.Actualizar("sp_mant_operador", parametros, Oper.Operador, Oper.Nombre, Oper.Activo, Oper.Turno, Oper.Planta, Oper.Linea, Oper.Nivel, Oper.FechaIngre, Oper.Usuario);
        }

        public static DataTable Listar()
        {
            DataTable datos = new DataTable();
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_operador";
                datos = AccesoDatos.Consultar(sQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable Consultar(OperadorLogica Oper)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_operador WHERE empleado = '"+Oper.Operador+"'");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultarOrbis(OperadorLogica oper)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.ConsultarMySql("SELECT * FROM empleado WHERE idEmpleado = "+int.Parse(oper.Operador)+"");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultarLinea(OperadorLogica Oper)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_operador WHERE planta = '"+Oper.Planta+"' AND linea = '" + Oper.Linea + "' AND turno = '"+Oper.Turno+"'");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;

        }

        public static DataTable ConsultarPuesto(OperadorLogica Oper)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_operador WHERE planta = '" + Oper.Planta + "' AND nivel = '"+Oper.Nivel+"' AND turno = '"+Oper.Turno+"' ORDER BY nombre ");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultarPuestoFiltro(OperadorLogica Oper)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_operador WHERE planta = '" + Oper.Planta + "' AND nivel = '" + Oper.Nivel + "' AND turno = '" + Oper.Turno + "' and nombre like'%"+Oper.Nombre+"%' ORDER BY nombre ");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static bool Verificar(OperadorLogica Oper)
        {
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_operador WHERE empleado = '" + Oper.Operador + "'";
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

        public static bool Eliminar(OperadorLogica Oper)
        {
            try
            {
                string sQuery = "DELETE FROM t_operador WHERE empleado = '" + Oper.Operador + "'";
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

        public static bool AntesDeEliminar(OperadorLogica linea)
        {
            try
            {
                string sQuery;

                DataTable datos = new DataTable();

                sQuery = "SELECT * FROM t_lineup WHERE planta = '" + linea.Planta + "' and linea = '" + linea.Linea + "'";
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
    }
}
