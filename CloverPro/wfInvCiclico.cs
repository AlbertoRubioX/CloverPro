using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using Logica;

namespace CloverPro
{
    public partial class wfInvCiclico : Form
    {
        FormWindowState _WindowStateAnt;
        private int _iWidthAnt;
        private int _iHeightAnt;

        private string _lsPath;
        public wfInvCiclico()
        {
            InitializeComponent();

            _iWidthAnt = Width;
            _iHeightAnt = Height;
            _WindowStateAnt = WindowState;
        }

        
        private void wfInvCiclico_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;

            DataTable dt = ConfigLogica.Consultar();
            _lsPath = dt.Rows[0]["bin_directory"].ToString();

            Dictionary<string, string> List0 = new Dictionary<string, string>();
            List0.Add("EMPN", "EMPAQUE");
            List0.Add("MON", "TONER");
            List0.Add("COL", "COLOR");
            List0.Add("INKM", "INKJET MFG");
            List0.Add("INKP", "INKJET PKG");
            List0.Add("FUS", "FUSORES");
            cbbPlanta.DataSource = new BindingSource(List0, null);
            cbbPlanta.DisplayMember = "Value";
            cbbPlanta.ValueMember = "Key";
            cbbPlanta.SelectedIndex = 0;

            Dictionary<string, string> List1 = new Dictionary<string, string>();
            List1.Add("0", "All");
            for(int x = 1; x < 50; x++)
            {
                string sLine = x.ToString().PadLeft(2,'0');
                List1.Add(sLine, sLine);
            }
            cbbLine.DataSource = new BindingSource(List1, null);
            cbbLine.DisplayMember = "Value";
            cbbLine.ValueMember = "Key";
            cbbLine.SelectedIndex = 0;
            //
            
        }

        public void ResizeControl(Control ac_Control, int ai_Hor, ref int ai_WidthAnt, ref int ai_HegihtAnt, int ai_Retorna)
        {
            if (ai_WidthAnt == 0)
                ai_WidthAnt = ac_Control.Width;
            if (ai_WidthAnt == ac_Control.Width)
                return;

            int _dif = ai_WidthAnt - ac_Control.Width;
            int _difh = ai_HegihtAnt - ac_Control.Height;

            if (ai_Hor == 1)
                ac_Control.Height = this.Height - _difh;
            if (ai_Hor == 2)
                ac_Control.Width = this.Width - _dif;
            if (ai_Hor == 3)
            {
                ac_Control.Width = this.Width - _dif;
                ac_Control.Height = this.Height - _difh;
            }
            if (ai_Retorna == 1)
            {
                ai_WidthAnt = this.Width;
                ai_HegihtAnt = this.Height;
            }
        }

        private string getFileTime()
        {
            DateTime dtTime = DateTime.Now;
            int iHora = dtTime.Hour;
            if (iHora > 23)
                iHora -= 23;

            if (iHora >= 0 && iHora < 6)
                dtTime = dtTime.AddDays(-1);
            string sHrFile = string.Empty;
            if (iHora < 12)
                sHrFile = iHora.ToString() + "am";
            else
            {
                if (iHora > 23)
                {
                    if (iHora == 24)
                        iHora = 12;
                    else
                        iHora -= 24;
                    sHrFile = iHora.ToString() + "am";
                }
                else
                {
                    if (iHora > 12)
                        iHora -= 12;
                    sHrFile = iHora.ToString() + "pm";
                }
            }
            return sHrFile;
        }
        private void btnBin_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                string sPta = cbbPlanta.SelectedValue.ToString();
                string sLine = string.Empty;
                switch(sPta)
                {
                    case "EMPN":
                        sLine = "TPACKA";
                        break;
                    case "COL":
                        sLine = "CTNR";
                        break;
                    case "MON":
                        sLine = "TNR";
                        break;
                    case "INKM":
                        sLine = "INKM";
                        break;
                    case "INKP":
                        sLine = "INKP";
                        break;
                    case "FUS":
                        sLine = "FUS";
                        break;
                }

                
                string sHora = getFileTime();
                string sArchivo = "Warehouse Bin Contents "+ sHora;// "Contador1";
                if (sLine != "TPACKA")
                    sArchivo += sLine.ToLower();

                sArchivo = _lsPath + @"\"+sArchivo+".csv";
                if (!File.Exists(sArchivo))
                {
                    Cursor = Cursors.Default;
                    return;
                }

                DataTable dt = LoadFile(sArchivo);

                if (dt.Rows.Count > 0)
                    dgwData.DataSource = null;

                for (int x = 0; x < dt.Rows.Count; x++)
                {
                    //eliminar BIN_CODE <> TPACKA
                    string sBin = dt.Rows[x][0].ToString();
                    if (!sBin.StartsWith(sLine))
                    {
                        dt.Rows[x].Delete();
                        x--;
                        continue;
                    }
                    string sBinLine = sBin.Substring(sBin.Length - 2);
                    int iP = 0;
                    if (!int.TryParse(sBinLine.Substring(0, 1), out iP))
                        sBinLine = sBinLine.Substring(1);

                    if (cbbLine.SelectedIndex == 0)
                    {
                        if (sBinLine == "0")
                        {
                            dt.Rows[x].Delete();
                            x--;
                            continue;
                        }
                    }
                    else
                    {
                        string sLinea = cbbLine.SelectedValue.ToString();
                        if (sLinea.StartsWith("0") && sLine != "FUS" && !sLine.StartsWith("INK"))
                            sLinea = sLinea.Replace("0", "");
                        
                        if(sLinea != sBinLine)
                        {
                            dt.Rows[x].Delete();
                            x--;
                            continue;
                        }

                    }
                    //eliminar sin saldo
                    int iCant = 0;
                    if (!int.TryParse(dt.Rows[x][5].ToString(), out iCant))//QUANTITY --> BEFORE //Available qty to take 18
                        iCant = 0;

                    if (iCant <= 0)
                    {
                        dt.Rows[x].Delete();
                        x--;
                    }
                }

                if (dt.Rows.Count > 0)
                {
                    dt.Columns.Add("cicle");
                    dt.Columns.Add("variance");
                    dgwData.DataSource = dt;

                    foreach(DataGridViewRow row in dgwData.Rows)
                    {
                        row.Cells[6].Value = string.Empty;
                        row.Cells[7].Value = string.Empty;
                    }
                    dgwData.Columns[6].HeaderText = "Cicle Count";
                    dgwData.Columns[7].HeaderText = "Variance";
                    
                }
                    

                tssContador.Text = "Bin Contents :" + dt.Rows.Count.ToString();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Cursor = Cursors.Arrow;
            }
            Cursor = Cursors.Arrow;
            
        }
        public List<string> SplitCSV(string line)
        {
            List<string> result = new List<string>();

            if (string.IsNullOrEmpty(line))
            {
                //throw new ArgumentException();
                return result;
            }


            

            int index = 0;
            int start = 0;
            bool inQuote = false;
            StringBuilder val = new StringBuilder();

            // parse line
            foreach (char c in line)
            {
                switch (c)
                {
                    case '"':
                        inQuote = !inQuote;
                        break;

                    case ',':
                        if (!inQuote)
                        {
                            result.Add(line.Substring(start, index - start)
                                .Replace("\"", ""));

                            start = index + 1;
                        }

                        break;
                }

                index++;
            }

            if (start < index)
            {
                result.Add(line.Substring(start, index - start).Replace("\"", ""));
            }

            return result;
        }
        private DataTable LoadFile(string _asFile)
        {
            int iErr = 0;
            DataTable dt = new DataTable();
            try
            {
                using (StreamReader sr = new StreamReader(_asFile))
                {
                    string[] headers = sr.ReadLine().Split(',');
                    foreach (string header in headers)
                    {
                        dt.Columns.Add(header);
                    }
                    
                    while (!sr.EndOfStream)
                    {
                       
                        List<string> result = SplitCSV(sr.ReadLine());
                        //string[] rows = sr.ReadLine().Split(',');
                        if (result.Count > 0)
                        {
                            DataRow dr = dt.NewRow();
                            for (int i = 0; i < headers.Length; i++)
                            {
                                //dr[i] = rows[i];
                                dr[i] = result[i];
                                iErr = i;
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                }
            }
            catch(Exception e)
            {
                string sErr = iErr.ToString() + " " + e.ToString();
                MessageBox.Show(sErr, Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return dt;
        }
        private DataTable LoadFile2(string _asFile)
        {
            int iErr = 0;
            DataTable dt = new DataTable();
            try
            {
                using (StreamReader sr = new StreamReader(_asFile))
                {
                    string[] headers = sr.ReadLine().Split(',');
                    foreach (string header in headers)
                    {
                        dt.Columns.Add(header);
                    }
                    while (!sr.EndOfStream)
                    {

                        string[] rows = sr.ReadLine().Split(',');
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            dr[i] = rows[i];
                            iErr = i;
                        }
                        dt.Rows.Add(dr);
                    }

                }
            }
            catch (Exception e)
            {
                string sErr = iErr.ToString() + " " + e.ToString();
                MessageBox.Show(sErr, Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return dt;
        }
        private void btnOrders_Click(object sender, EventArgs e)
        {
            string sLinea = cbbLine.SelectedValue.ToString();
            if (sLinea == "All")
                return;

            Cursor = Cursors.WaitCursor;

            string sPta = cbbPlanta.SelectedValue.ToString();
            string sLine = string.Empty;
            string sLineF = string.Empty;
            switch (sPta)
            {
                case "EMPN":
                    sLine = "TPACKA";
                    sLineF = "MX1APAC";
                    break;
                case "COL":
                    sLine = "CTNR";
                    sLineF = "MX1ACTNR";
                    break;
                case "MON":
                    sLine = "TNR";
                    sLineF = "MX1ATNR";
                    break;
                case "INKM":
                    sLine = "INKM";
                    break;
                case "INKP":
                    sLine = "INKP";
                    break;
                case "FUS":
                    sLine = "FUS";
                    sLineF = "MX1AFUSER";
                    break;
            }

            string sHora = getFileTime();
            string sArchivo = "Registered pickline "+sHora;
            if (sLine != "TPACKA")
                sArchivo += sLine.ToLower();

            sArchivo = _lsPath + @"\" + sArchivo + ".csv";
            if (!File.Exists(sArchivo))
            {
                Cursor = Cursors.Default;
                return;
            }

            DataTable dt = LoadFile(sArchivo);

            if (dt.Rows.Count > 0)
                dgwRPO.DataSource = null;

            for(int x = 0; x < dt.Rows.Count; x++)
            {
                string sRouting = dt.Rows[x][2].ToString();//9
                if(string.IsNullOrEmpty(sRouting))
                {
                    dt.Rows[x].Delete();
                    x--;
                    continue;
                }

                if (!sRouting.StartsWith(sLine)) 
                {
                    dt.Rows[x].Delete();
                    x--;
                    continue;
                }

                //sRouting = sRouting.Substring(7);
                int iP = 0;
                string sRouLine = sRouting.Substring(sRouting.Length - 2);
                if (!int.TryParse(sRouLine.Substring(0, 1), out iP))
                    sRouLine = sRouLine.Substring(1);
                sRouLine = sRouLine.PadLeft(2, '0');

                if (sLinea != sRouLine)
                {
                    dt.Rows[x].Delete();
                    x--;
                }
            }

            if (dt.Rows.Count > 0)
            {
                dgwRPO.DataSource = dt;
                button1_Click(sender, e);
                //btnPick_Click(sender, e);
            }
                

            Cursor = Cursors.Arrow;
        }

        private void btnPick_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            string sPta = cbbPlanta.SelectedValue.ToString();
            string sLine = string.Empty;
            switch (sPta)
            {
                case "EMPN":
                    sLine = "TPACKA";
                    break;
                case "COL":
                    sLine = "CTNR";
                    break;
                case "MON":
                    sLine = "TNR";
                    break;
                case "INKM":
                    sLine = "INKM";
                    break;
                case "INKP":
                    sLine = "INKP";
                    break;
                case "FUS":
                    sLine = "FUS";
                    break;
            }

            string sArchivo = "Contador3";
            if (sLine != "TPACKA")
                sArchivo += sLine.ToLower();

            sArchivo = _lsPath + @"\" + sArchivo + ".csv";
            if (!File.Exists(sArchivo))
            {
                Cursor = Cursors.Default;
                return;
            }

            int iRows = dgwRPO.Rows.Count;
            if (iRows > 0)
            {
                string sRpoIni = dgwRPO[0, 0].Value.ToString();
                long lRpoIni = long.Parse(sRpoIni.Substring(3));
                iRows--;
                string sRpoFin = dgwRPO[0, iRows].Value.ToString();
                long lRpoFin = long.Parse(sRpoFin.Substring(3));

                DataTable dt = LoadFile2(sArchivo);
                for (int x = 0; x < dt.Rows.Count; x++)
                {
                    string sRPO = dt.Rows[x][1].ToString();
                    if (string.IsNullOrEmpty(sRPO))
                        continue;

                    string sBin = dt.Rows[x][2].ToString();
                    if (!sBin.StartsWith(sLine))
                    {
                        dt.Rows[x].Delete();
                        x--;
                        continue;
                    }

                    if (cbbLine.SelectedIndex == 0)
                    {
                        if (sBin.Substring(sBin.Length - 1, 1) == "0")
                        {
                            dt.Rows[x].Delete();
                            x--;
                            continue;
                        }
                    }
                    else
                    {
                        string sLinea = cbbLine.SelectedValue.ToString();
                        if (sLinea.StartsWith("0") && sLine != "FUS" && !sLine.StartsWith("INK"))
                            sLinea = sLinea.Replace("0", "");

                        //string sBinLine = sBin.Substring(6);
                        int iP = 0;
                        string sBinLine = sBin.Substring(sBin.Length - 2);
                        if (!int.TryParse(sBinLine.Substring(0, 1), out iP))
                            sBinLine = sBinLine.Substring(1);
                        
                        if (sLinea != sBinLine)
                        {
                            dt.Rows[x].Delete();
                            x--;
                            continue;
                        }
                    }

                    long lRpo = 0;
                    if (!long.TryParse(sRPO.Substring(3), out lRpo))
                        continue;

                    bool bExis = false;
                    foreach(DataGridViewRow row in dgwRPO.Rows)
                    {
                        string sRpor = row.Cells[0].Value.ToString();
                        if (sRpor == sRPO)
                        {
                            bExis = true;
                            break;
                        }
                    }

                    if (!bExis)
                    {
                        dt.Rows[x].Delete();
                        x--;
                        continue;
                    }
                    //if (lRpo < lRpoIni || lRpo > lRpoFin)
                    //{
                    //    dt.Rows[x].Delete();
                    //    x--;
                    //    continue;
                    //}
                }

                if (dt.Rows.Count > 0)
                {
                    dgwRPO.DataSource = null;
                    dgwRPO.DataSource = dt;

                    button1_Click(sender, e);
                }
            }
            Cursor = Cursors.Default;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dgwRPO.RowCount == 0 || dgwData.RowCount == 0)
                return;

            if (cbbLine.SelectedIndex <= 0)
                return;

            int iIdx = 0;
            string asItem = string.Empty;
            string sLinea = cbbLine.SelectedValue.ToString();
            try
            {
                Cursor = Cursors.WaitCursor;

                foreach (DataGridViewRow row in dgwData.Rows)
                {
                    string sBin = row.Cells[0].Value.ToString();
                    int iP = 0;
                    string sBinLine = sBin.Substring(sBin.Length - 2);
                    if (!int.TryParse(sBinLine.Substring(0, 1), out iP))
                        sBinLine = sBinLine.Substring(1);

                    sBinLine = sBinLine.PadLeft(2,'0');
                    
                    if (sLinea != sBinLine)
                        continue;

                    string sItem = row.Cells[1].Value.ToString();
                    double dCant = double.Parse(row.Cells[5].Value.ToString());
                    double dCantT = 0;
                    foreach (DataGridViewRow drow in dgwRPO.Rows)//registered pick
                    {
                        iIdx = drow.Index;
                        string sItemD = drow.Cells[3].Value.ToString();
                        asItem = sItemD;
                        double dCantD = 0;
                        if (!double.TryParse(drow.Cells[4].Value.ToString(), out dCantD)) //Quantity
                            dCantD = 0;

                        if (sItem == sItemD)
                            dCantT += dCantD;
                    }
                    row.Cells[6].Value = dCantT;
                    double dCantDif = dCant - dCantT;
                    row.Cells[7].Value = dCantDif; //diferencia
                    if (dCantDif > 0)
                    {
                        row.Cells[1].Style.BackColor = Color.DarkRed;
                        row.Cells[1].Style.ForeColor = Color.White;
                        row.Cells[7].Style.BackColor = Color.DarkRed;
                        row.Cells[7].Style.ForeColor = Color.White;
                        
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(iIdx.ToString() + Environment.NewLine + asItem + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Cursor = Cursors.Default;
            }
            Cursor = Cursors.Default;
        }

        private void wfInvCiclico_Resize(object sender, EventArgs e)
        {
            if (WindowState != _WindowStateAnt && WindowState != FormWindowState.Minimized)
            {
                _WindowStateAnt = WindowState;
                ResizeControl(panel1, 3, ref _iWidthAnt, ref _iHeightAnt, 0);
                ResizeControl(tabControl1, 3, ref _iWidthAnt, ref _iHeightAnt, 0);
                ResizeControl(dgwData, 3, ref _iWidthAnt, ref _iHeightAnt, 0);
                ResizeControl(dgwRPO, 3, ref _iWidthAnt, ref _iHeightAnt, 1);
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            if (dgwData.RowCount == 0)
                return;

            Cursor = Cursors.WaitCursor;
            int iIdx = 0;
            string asItem = string.Empty;
            try
            {
                for (int x = 0; x < dgwData.Rows.Count; x++)
                {
                    double dCant = 0;
                    if (!double.TryParse(dgwData[7, x].Value.ToString(), out dCant))
                        dCant = 0;
                    if(dCant <= 0)
                    {
                        DataGridViewRow row = dgwData.Rows[x];
                        dgwData.Rows.Remove(row);
                        x--;
                    }
                }
            }
            catch (Exception ex) { throw; }

            Cursor = Cursors.Default;
        }
    }
}
