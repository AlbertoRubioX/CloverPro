using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Data.OleDb;
using Logica;

namespace CloverPro.Funciones
{
    public class ImportaExcel
    {
        public static DataTable ImportarRPO(string _asRPO)
        {
            DataTable _tabla = new DataTable();
            DataTable dtReturn = new DataTable();

            OleDbConnection conexion = null;
            DataSet dataSet = null;
            OleDbDataAdapter dataAdapter = null;

            string consultaHojaExcel = "";
            string sConexionExcel = "";
            string sDir="";
            try
            {
                DataTable dtConf = ConfigLogica.Consultar();
                sDir = dtConf.Rows[0]["directorio"].ToString();
            }
            catch(Exception ex)
            {
                throw (ex);
            }

            string sArchivo = sDir + @"\RPOListado.xlsx";
            string fileExtension = Path.GetExtension(sArchivo);
            
            sConexionExcel = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + sArchivo + "';Mode=ReadWrite;Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\"";
            try
            {
                conexion = new OleDbConnection(sConexionExcel);//creamos la conexion con la hoja de excel
                conexion.Open(); //abrimos la conexion

                System.Data.DataTable dtSchema = conexion.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if ((null != dtSchema) && (dtSchema.Rows.Count > 0))
                {
                    string firstSheetName = dtSchema.Rows[0]["TABLE_NAME"].ToString();
                    consultaHojaExcel = "Select * from [" + firstSheetName + "]";
                }
                dataAdapter = new OleDbDataAdapter(consultaHojaExcel, conexion);
                dataSet = new DataSet();
                dataAdapter.TableMappings.Add("tbl", "Table");
                dataAdapter.Fill(dataSet);
                _tabla = dataSet.Tables[0];

                if (_tabla.Rows.Count > 0)
                {
                    DataRow[] frow = _tabla.Select("No = '" + _asRPO + "'");
                    dtReturn = _tabla.Clone();

                    foreach (DataRow row in frow)
                    {
                        dtReturn.ImportRow(row);
                    }

                }
            }
            catch { throw; }
            finally { conexion.Close(); }

            return dtReturn;
        }
    }
}
