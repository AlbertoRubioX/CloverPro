using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class ModeloLogica
    {
        public string Modelo { get; set; }
        public string Descrip { get; set; }
        public string Estatus { get; set; }
        public string Tipo { get; set; }
        public string Planta { get; set; }
        public double Tasktime { get; set; }
        public double Duracion { get; set; }
        public double Coating { get; set; }
        public double CantGuia { get; set; }
        public double CantRetra { get; set; }
        public double CantMylar { get; set; }
        public double CantMulti { get; set; }
        public string Robot { get; set; }
        public double CantOper { get; set; }
        public double TotalOper { get; set; }
        public string RevStd { get; set; }
        public string UltRev { get; set; }
        public string Conversion { get; set; }
        public string CRC { get; set; }
        public string FormatoStd { get; set; }
        public string FormatoPath { get; set; }
        public string VisualAidFolder { get; set; }
        public string Usuario { get; set; }
        
        public static int Guardar(ModeloLogica mode)
        {
            string[] parametros = { "@Modelo", "@Descrip", "@Estatus", "@Tipo", "@Planta", "@Takttime", "@Duracion", "@Opcoating", "@CantGuia", "@CantRetra", "@CantMylar", "@CantMulti", "@Robot", "@CantOper", "@TotalOper", "@RevStd", "UltRev", "@Conversion", "CrcConv", "@FormatoStd", "@FormatoPath", "@VisualAidFolder", "@Usuario"};
            return AccesoDatos.Actualizar("sp_mant_modelo", parametros, mode.Modelo, mode.Descrip, mode.Estatus, mode.Tipo, mode.Planta, mode.Tasktime, mode.Duracion, mode.Coating, mode.CantGuia, mode.CantRetra, mode.CantMylar, mode.CantMulti, mode.Robot, mode.CantOper, mode.TotalOper, mode.RevStd, mode.UltRev, mode.Conversion, mode.CRC, mode.FormatoStd, mode.FormatoPath, mode.VisualAidFolder, mode.Usuario);
        }

        public static DataTable Listar()
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_modelo");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable Consultar(ModeloLogica mode)
        {
            DataTable datos = new DataTable();
            try
            {
                datos = AccesoDatos.Consultar("SELECT * FROM t_modelo WHERE modelo = '" + mode.Modelo + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static bool Verificar(ModeloLogica mode)
        {
            try
            {
                string sQuery;
                sQuery = "SELECT * FROM t_modelo WHERE modelo = '" + mode.Modelo + "'";
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

        public static bool AntesDeBorrar(ModeloLogica mode)
        {
            try
            {
                string sQuery;

                DataTable datos = new DataTable();

                sQuery = "SELECT * FROM t_lineup WHERE modelo_rev = '" + mode.Modelo + "'";
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

        public static bool Borrar(ModeloLogica mode)
        {
            try
            {
                string sQuery = "DELETE FROM t_modelo WHERE modelo = '" + mode.Modelo + "'"; 
                if (AccesoDatos.Borrar(sQuery) != 0)
                {
                    sQuery = "DELETE FROM t_moderela WHERE modelo = '" + mode.Modelo + "'";
                    AccesoDatos.Borrar(sQuery);

                    sQuery = "DELETE FROM t_modesta WHERE modelo = '" + mode.Modelo + "'";
                    if (AccesoDatos.Borrar(sQuery) != 0)
                        return true;
                    else
                        return false;
                }
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
