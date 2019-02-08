using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.Windows.Forms;
using Spire.Barcode.Forms;
using Spire.Barcode;
using System.IO;
using Neodynamic.SDK.Printing;
using System.Management;
using Logica;
using Datos;

namespace CloverPro
{
    public partial class wfCodBarrasEmp : Form
    {
        ThermalLabel _currentThermalLabel = null;
        ImageSettings _imgSetting = new ImageSettings();
        private string _lsProceso = "EMP010";
        private string _lsPrinter;
        public wfCodBarrasEmp()
        {
            InitializeComponent();
        }
        #region regInicio
        private void Inicio()
        {
            cbbOrigen.ResetText();
            txtRPO.Clear();
            txtSKU.Clear();
            txtSKU.Enabled = false;
            txtCant.Text = "1";

            Dictionary<string, string> Orig = new Dictionary<string, string>();
            Orig.Add("M", "Mexico");
            Orig.Add("U", "U.S.A.");
            Orig.Add("C", "China");
            Orig.Add("V", "Vietnam");
            Orig.Add("S", "Serbia");
            cbbOrigen.DataSource = new BindingSource(Orig, null);
            cbbOrigen.DisplayMember = "Value";
            cbbOrigen.ValueMember = "Key";
            cbbOrigen.SelectedIndex = 0;

            
        }
        private void wfCodBarrasEmp_Activated(object sender, EventArgs e)
        {
            txtRPO.Focus();
        }

        private void wfCodBarrasEmp_Load(object sender, EventArgs e)
        {
            if (!AccesoLogica.verificaAdmin(GlobalVar.gsUsuario))
            {
                wfCodigo Codigo = new wfCodigo();
                Codigo.ShowDialog();
                string sCodigo = Codigo._sCodigo;
                if (string.IsNullOrEmpty(sCodigo))
                {
                    Close();
                    return;
                }
            }

            Inicio();
            txtRPO.Focus();
        }

        #endregion

        #region regGuardar 
        private bool Valida()
        {

            if (string.IsNullOrEmpty(txtRPO.Text) || string.IsNullOrWhiteSpace(txtRPO.Text))
                return false;

            if (string.IsNullOrEmpty(txtSKU.Text) || string.IsNullOrWhiteSpace(txtSKU.Text))
                return false;
            else
            {
                if (txtSKU.Text.ToString() == txtRPO.Text.ToString())
                {
                    MessageBox.Show("El RPO debe ser direferente al Modelo", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            return true;
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            if (!Valida())
                return;

            string sRPO = txtRPO.Text.ToString();
            int iCant = Int32.Parse(txtCant.Text);

            if (sRPO.IndexOf("RPO") != -1)
                sRPO = sRPO.Replace("RPO", "");

            if (ImprimeEtiquetas(sRPO, iCant))
            {
                this.Cursor = Cursors.WaitCursor;
                Guardar(sRPO);
                    //EnviarMail();

                this.Cursor = Cursors.Arrow;

                Inicio();

                txtRPO.Focus();
            }
        }
        private bool EnviarMail()
        {

            DataTable dtMail = new DataTable();
            dtMail = CorreoLogica.ConsultarPend();
            if(dtMail.Rows.Count != 0)
            {
                try
                {
                    int iFolio = Convert.ToInt32(dtMail.Rows[0]["folio"].ToString());
                    EnviarCorreo envMail = new EnviarCorreo();
                    envMail.FolioMail = iFolio;
                    envMail.Asunto = dtMail.Rows[0]["asunto"].ToString();

                    string sMsg = EnviarCorreo.EnviaAlerta(envMail);

                    CorreoLogica correo = new CorreoLogica();

                    if (sMsg == "OK")
                    {
                        sMsg = "Se ha Enviado una Notificación por Correo electrónico al Supervisor.";
                        correo.Estado = "E";
                    }
                    else
                        correo.Estado = "R";

                    MessageBox.Show(sMsg, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    correo.Folio = Convert.ToInt32(dtMail.Rows[0]["folio"].ToString());
                    correo.Asunto = dtMail.Rows[0]["asunto"].ToString();
                    correo.Proceso = _lsProceso;
                    CorreoLogica.Guardar(correo);
                }
                catch(Exception ex)
                {
                    throw (ex);
                }
                
            }
            
            return true;
        }
        private bool Guardar(string asRPO)
        {
            try
            {
                bool bReturn;

                EtiquetaLogica eti = new EtiquetaLogica();

                string sLeyenda = "";
                if (cbbOrigen.SelectedValue.ToString() == "M")
                    sLeyenda = "Mexico";
                if (cbbOrigen.SelectedValue.ToString() == "U")
                    sLeyenda = "U.S.A.";
                if (cbbOrigen.SelectedValue.ToString() == "C")
                    sLeyenda = "China";
                if (cbbOrigen.SelectedValue.ToString() == "V")
                    sLeyenda = "Vietnam";
                if (cbbOrigen.SelectedValue.ToString() == "S")
                    sLeyenda = "Serbia";

                eti.RPO = asRPO;
                eti.Modelo = txtSKU.Text.ToString();
                eti.Cantidad = Convert.ToInt32(txtCant.Text);
                eti.Planta = GlobalVar.gsPlanta;
                eti.Estacion = GlobalVar.gsEstacion;
                eti.Usuario = GlobalVar.gsUsuario;
                eti.Tipo = "E";
                eti.Leyenda = "Assembled in "+ sLeyenda;

                if (EtiquetaLogica.Guardar(eti) == 0)
                    bReturn = false;
                else
                    bReturn = true;

                return bReturn;
            }
            catch(Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                MessageBox.Show("EtiquetaLogica.Guardar(eti)" + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }
        private bool ValidaRangoImp(string asRPO, int aiIni)
        {
            try
            {
                EtiquetaLogica eti = new EtiquetaLogica();
                eti.RPO = asRPO;
                eti.Inicio = aiIni;

                if (EtiquetaLogica.VerificarRango(eti))
                    return false;
                else
                    return true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            
        }

        private bool ImprimeEtiquetas(string asRPO, int aiCant)
        {
            int iCont = 0;
            iCont++;

            this.Cursor = Cursors.WaitCursor;

            dtsBarcodes ds = new dtsBarcodes();
            DataTable t = ds.Tables.Add("dtBarcodes");
            t.Columns.Add("rpo", Type.GetType("System.String"));
            t.Columns.Add("modelo", Type.GetType("System.String"));
            t.Columns.Add("consec", Type.GetType("System.String"));
            t.Columns.Add("Barcode", Type.GetType("System.Byte[]"));
            t.Columns.Add("leyenda", Type.GetType("System.String"));

            BarCodeControl cont = new BarCodeControl();

            cont.Type = BarCodeType.Code128;
            cont.Height = 50;
            cont.ShowText = false;
            cont.ShowCheckSumChar = false;
            cont.Code128SetMode = Code128SetMode.Auto;

            DataRow r;
            
            string sCode = asRPO;
            string sLine = txtSKU.Text.ToString();
            string sLeyenda="";

            sLeyenda = cbbOrigen.SelectedValue.ToString();
            if (sLeyenda == "M")
                sLeyenda = "Mexico";
            if (sLeyenda == "U")
                sLeyenda = "U.S.A.";
            if (sLeyenda == "C")
                sLeyenda = "China";
            if (sLeyenda == "V")
                sLeyenda = "Vietnam";
            if (sLeyenda == "S")
                sLeyenda = "Serbia";

            r = t.NewRow();
            r["rpo"] = asRPO;
            r["modelo"] = sLine;
            r["consec"] = iCont.ToString();
            r["leyenda"] = "Assembled in " + sLeyenda;
            cont.Data = sCode;
            BarCodeGenerator bar = new BarCodeGenerator(cont);
            Image im = bar.GenerateImage();

            r["Barcode"] = imageToByteArray(im);

            t.Rows.Add(r);
            

            try
            {
                ReportDocument rptAgenda = new ReportDocument();

                DataTable dtConf = ConfigLogica.Consultar();
                string sPrinter = dtConf.Rows[0]["emp_printer"].ToString();
                string sDir = dtConf.Rows[0]["directorio"].ToString();
                string sArchivo = sDir + @"\Reportes\";

                if (string.IsNullOrEmpty(_lsPrinter))
                {
                    
                    PrintDialog dialog = new PrintDialog();
                    DialogResult result = dialog.ShowDialog();
                    if (result == DialogResult.OK)
                        _lsPrinter = dialog.PrinterSettings.PrinterName;
                    else
                    {
                        this.Cursor = Cursors.Arrow;
                        MessageBox.Show("No se ha especificado la impresora de etiquetas", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                }
                    
                if(_lsPrinter.IndexOf("2844") != -1)
                    sArchivo = sArchivo + "ImpCodBarEmp2.rpt";
                else
                    sArchivo = sArchivo + "ImpCodBarEmp1.rpt";


                rptAgenda.Load(sArchivo);
                rptAgenda.SetDataSource(t);

                rptAgenda.PrintOptions.PrinterName = _lsPrinter;
                rptAgenda.PrintToPrinter(aiCant, false, 0,0);
                this.Cursor = Cursors.Arrow;
                return true;

            }
            catch(Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
        }
        private string FindPrinters()
        {
            string query = string.Format("SELECT * from Win32_Printer");

            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection coll = searcher.Get();

            foreach (ManagementObject printer in coll)
            {
                //foreach (PropertyData property in printer.Properties)
                //{
                //    Console.WriteLine(string.Format("{0}: {1}", property.Name, property.Value));
                //}

                var property = printer.Properties["DriverName"];
                if (property.Value.ToString().ToLowerInvariant().Contains("zebra"))
                {
                    _lsPrinter = property.Value.ToString();
                    //Console.ForegroundColor = ConsoleColor.Red;
                    //Console.Write("ZEBRA: ");
                }
                else
                {
                    //Console.ForegroundColor = ConsoleColor.Gray;
                    //Console.Write("Regular: ");
                }

            }
            return _lsPrinter;
        }
        #endregion

        #region Controles
        private void txtRPO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();

            if (e.KeyCode != Keys.Enter)
                return;

            if (!string.IsNullOrEmpty(txtRPO.Text))
            {
                //FIND SKU IN DATABASE
                string sModelo = BuscarRPO(txtRPO.Text.ToString());

                if (!string.IsNullOrEmpty(sModelo))
                {
                    txtSKU.Text = sModelo;
                    btnGenerar_Click(sender, e);
                }
                else
                {
                    txtSKU.Clear();
                    txtSKU.Enabled = true;
                    txtCant.Text = "1";

                    txtSKU.Focus();
                }

            }
        }

        private string BuscarRPO(string asRPO)
        {
            string sModelo = "";
            DataTable dtEx = new DataTable();

            try
            {
                int iPos = asRPO.IndexOf("-");

                if (iPos == -1)
                {
                    MessageBox.Show("La Etiqueta escaneada no cuenta con el Formato Esperado.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return sModelo;
                }

                asRPO = asRPO.Substring(0, iPos);
                if (asRPO.IndexOf("RPO") == -1)
                    asRPO = "RPO" + asRPO;

                RpoLogica rpo = new RpoLogica();
                rpo.RPO = asRPO;
                if (!RpoLogica.Verificar(rpo))
                    MessageBox.Show("No se encontró información del RPO, favor de capturar ó escanear Modelo", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                {
                    DataTable data = RpoLogica.Consultar(rpo);
                    sModelo = data.Rows[0]["modelo"].ToString();

                    txtCant.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error... " + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return sModelo;
        }

        private void txtSKU_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            if (!string.IsNullOrEmpty(txtSKU.Text))
                btnGenerar.Focus();
        }


        private void txtInicio_Enter(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCant.Text))
            {
                if (string.IsNullOrEmpty(txtSKU.Text))
                {
                    if (txtSKU.Enabled)
                        txtSKU.Focus();
                    else
                        txtRPO.Focus();
                }
                else
                    txtCant.Focus();
            }
        }
        private void txtCant_Enter(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSKU.Text))
            {
                if (txtSKU.Enabled)
                    txtSKU.Focus();
                else
                    txtRPO.Focus();
            }
        }

        private void txtCant_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            if (!string.IsNullOrEmpty(txtCant.Text) && txtCant.Text != "0")
                txtCant.Focus();
            else
                btnGenerar.Focus();
        }

        private void txtCant_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void txtCant_TextChanged(object sender, EventArgs e)
        {
           
        }
        #endregion

        #region ThermalLabel
        private ThermalLabel GenerateBasicThermalLabel(string _asCode,string _asModel, string _asLeyend)
        {
            //Define a ThermalLabel object and set unit to inch and label size
            ThermalLabel tLabel = new ThermalLabel(UnitType.Inch, 2.5, 1);
            tLabel.GapLength = .2;
            
            TextItem txtItem = new TextItem(0.3, 0.3, 2, 0.5, _asCode);
            txtItem.Font.Name = "Consolas";
            txtItem.Font.Bold = true;
            txtItem.Font.Size = 12;
            txtItem.Font.Unit = FontUnit.Point;

            TextItem txtItem2 = new TextItem(0.3, 0.45, 2, 0.5, _asModel);
            txtItem2.Font.Name = "Calibri";
            txtItem2.Font.Bold = true;
            txtItem2.Font.Size = 8;
            txtItem2.Font.Unit = FontUnit.Point;

            TextItem txtItem3 = new TextItem(0.3, 0.58, 2, 0.5, _asLeyend);
            txtItem3.Font.Name = "Times New Roman";
            txtItem3.Font.Size = 8;
            txtItem3.Font.Unit = FontUnit.Point;
            //Define a BarcodeItem object
            BarcodeItem bcItem = new BarcodeItem(0.2, 0.05, 2, .25, BarcodeSymbology.Code128, _asCode);

            bcItem.Font.Name = "Arial";
            bcItem.Font.Size = 10;
            bcItem.BarHeight = 0.50;
            bcItem.BarWidth = 0.0095;
            bcItem.DisplayCode = false;
            bcItem.BarcodeAlignment = BarcodeAlignment.TopLeft;

            tLabel.Items.Add(bcItem);
            tLabel.Items.Add(txtItem);
            tLabel.Items.Add(txtItem2);
            tLabel.Items.Add(txtItem3);

            return tLabel;
        }
        private bool ImprimeEtiqueta(string asRPO, int aiCant)
        {
            string sCode = asRPO;
            string sLine = txtSKU.Text.ToString();
            string sLeyenda = cbbOrigen.Text.ToString();
            sLeyenda = "Assembled in " + sLeyenda;

            string sTipoImp = "ZPL";
            
            Cursor = Cursors.WaitCursor;

            _currentThermalLabel = GenerateBasicThermalLabel(sCode,sLine,sLeyenda);

            using (PrintJob pj = new PrintJob())
            {
                pj.ThermalLabel = _currentThermalLabel;
                pj.Copies = 1;
                System.IO.MemoryStream ms = new System.IO.MemoryStream();

                ImageSettings imgSett = new ImageSettings();
                imgSett.ImageFormat = Neodynamic.SDK.Printing.ImageFormat.Tiff;
                imgSett.PixelFormat = PixelFormat.BGRA32;
                imgSett.AntiAlias = true;
                imgSett.TransparentBackground = false;

                pj.ExportToImage(ms, imgSett, 96);
            }

            //create a PrintJob object
            try
            {
                DataTable dtConf = ConfigLogica.Consultar();
                string sPrinter = dtConf.Rows[0]["emp_printer"].ToString();
                string sSerial = dtConf.Rows[0]["emp_serial"].ToString();

                PrinterSettings _printerSettings = new PrinterSettings();

                if (string.IsNullOrEmpty(sPrinter) && !string.IsNullOrEmpty(sSerial))
                {
                    _printerSettings.Communication.CommunicationType = CommunicationType.Serial;
                    _printerSettings.Communication.SerialPortName = sSerial;
                    _printerSettings.Communication.SerialPortBaudRate = 9600;
                    _printerSettings.Communication.SerialPortDataBits = 8;
                    _printerSettings.Communication.SerialPortFlowControl = (System.IO.Ports.Handshake)Enum.Parse(typeof(System.IO.Ports.Handshake), "None");
                    _printerSettings.Communication.SerialPortParity = (System.IO.Ports.Parity)Enum.Parse(typeof(System.IO.Ports.Parity), "None");
                    _printerSettings.Communication.SerialPortStopBits = (System.IO.Ports.StopBits)Enum.Parse(typeof(System.IO.Ports.StopBits), "One");
                    _printerSettings.Dpi = 203;
                    _printerSettings.ProgrammingLanguage = (ProgrammingLanguage)Enum.Parse(typeof(ProgrammingLanguage), sTipoImp);
                }
                else
                {
                    _printerSettings.Communication.CommunicationType = CommunicationType.USB;
                    _printerSettings.PrinterName = sPrinter;
                }
                
                using (PrintJob pj = new PrintJob(_printerSettings))
                {
                    pj.Copies = 1; // set copies
                    pj.PrintOrientation = PrintOrientation.Portrait;
                    pj.ThermalLabel = _currentThermalLabel; // set the ThermalLabel object
                    
                    pj.Print(); // print the ThermalLabel object                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(),Text,MessageBoxButtons.OK,MessageBoxIcon.Error);
                Cursor = Cursors.Default;
                return false;
            }

            Cursor = Cursors.Default;
            return true;
        }
        #endregion

        #region regBotones
        private void btnNew_Click(object sender, EventArgs e)
        {
            Inicio();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void btnHelp_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = ConfigLogica.Consultar();
                string sDirec = dt.Rows[0]["directorio"].ToString();
                if (!string.IsNullOrEmpty(sDirec))
                {
                    sDirec += @"\CloverPRO_AyudaVisual_e.pdf";
                    System.Diagnostics.Process.Start(sDirec);
                }
                else
                    MessageBox.Show("No se ha especificado la ubicacion del Manual", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error... " + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        #endregion

        private void cbbOrigen_SelectionChangeCommitted(object sender, EventArgs e)
        {
            txtRPO.Focus();
        }
    }
}
