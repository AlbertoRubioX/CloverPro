using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using Logica;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;

namespace CloverPro
{
    public partial class wfImpresor : Form
    {
        public string _lsProceso;
        public string _lsIndPlanta;
        public string _lsPlanta;
        public DateTime _ldtInicio;
        public DateTime _ldtFinal;
        public string _lsIndFolio;
        public string _lsFolioIni;
        public string _lsFolioFin;
        public string _lsIndLinea;
        public string _lsLineaIni;
        public string _lsLineaFin;
        public string _lsLineas;
        public string _lsIndModelo;
        public string _lsModelo;
        public string _lsIndRPO;
        public string _lsRPO;
        public string _lsTurno;
        public string _lsIndTurno;
        public string _lsArea;
        public string _lsIndArea;
        public string _lsEmpleado;
        public string _lsIndEmp;
        public string _lsTipoEmp;
        public string _lsIndEst;
        public string _lsEstatus;
        public string _lsIndNivel;
        public string _lsNivel;
        public string _lsBloqImp;
        public string _lsImpOperBloq;
        private string _lsDirec;
        public string _lsIndSolo;
        public string _lsIndHorario;
        public string _lsIndAct;
        public string _lsIndRegid;
        public string _lsSupervisor;

        public wfImpresor()
        {
            InitializeComponent();
        }
        private void wfImpresor_Load(object sender, EventArgs e)
        {

            DataTable data = ConfigLogica.Consultar();
            _lsDirec = data.Rows[0]["directorio"].ToString();

            if (_lsProceso == "REP010")
                ImprimeEtiquetas();

            if (_lsProceso == "REP020")
                InspeccionEtiquetas();

            if (_lsProceso == "REP030" || _lsProceso == "PRO060")
                CapturaLineUp();

            if (_lsProceso == "REP040")
                ReporteUsuarios();

            if (_lsProceso == "REP050")
                ReporteSuperLine();

            if (_lsProceso == "REP060")
                ReporteTransferencias();

            if (_lsProceso == "REP070")
                CapturaLineUpResumen();

            if (_lsProceso == "REP080")
                ReporteEmpaqueComp();

            if (_lsProceso == "REP090")
                ReporteHistorialPPH();

            if ((_lsProceso == "REP100"))
                DiarioSetup();

            if ((_lsProceso == "REP110"))
                ReporteArmadoRPO();

            if ((_lsProceso == "REP120"))
                ReporteGlobalRPO();

            if (_lsProceso == "PRO090")
                DuracionSetUp();
        }
        private void InspeccionEtiquetas()
        {
            try
            {
                ReportDocument rptDoc = new ReportDocument();
                rptDoc.Load(_lsDirec + @"\Reportes\rptEtiquetasDet.rpt");

                ReportesLogica rep = new ReportesLogica();
                rep.FechaIni = _ldtInicio;
                rep.FechaFin = _ldtFinal;
                rep.Turno = _lsTurno;
                rep.RPO = _lsRPO;

                DataTable dtSource = ReportesLogica.InspeccionEtiquetas(rep);
                rptDoc.SetDataSource(dtSource);
                
                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Close();
            }
        }

        private void ImprimeEtiquetas()
        {
            try
            {
                ReportDocument rptDoc = new ReportDocument();
                rptDoc.Load(_lsDirec + @"\Reportes\rptEtiquetas.rpt");

                ReportesLogica rep = new ReportesLogica();
                rep.Planta = _lsPlanta;
                rep.FechaIni = _ldtInicio;
                rep.FechaFin = _ldtFinal;
                
                DataTable dtSource = ReportesLogica.Etiquetas(rep);
                rptDoc.SetDataSource(dtSource);
                ////PARAMETROS
                ParameterFields paramFields = new ParameterFields();
                ParameterField paramField = new ParameterField();
                ParameterDiscreteValue discreteVal = new ParameterDiscreteValue();
                paramField.Name = "desde";
                discreteVal.Value = _ldtInicio.ToString().Substring(0,10);
                paramField.CurrentValues.Add(discreteVal);
                paramFields.Add(paramField);

                paramField = new ParameterField();
                discreteVal = new ParameterDiscreteValue();
                paramField.Name = "hasta";
                discreteVal.Value = _ldtFinal.ToString().Substring(0, 10);
                paramField.CurrentValues.Add(discreteVal);
                paramFields.Add(paramField);

                crystalReportViewer1.ParameterFieldInfo = paramFields;
                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Close();
            }
        }

        private void CapturaLineUp()
        {
            try
            {
                ReportDocument rptDoc = new ReportDocument();
                rptDoc.Load(_lsDirec + @"\Reportes\rptLineUpFormato.rpt");

                ReportesLogica rep = new ReportesLogica();
               
                rep.FechaIni = _ldtInicio;
                rep.FechaFin = _ldtFinal;
                rep.IndPlanta = _lsIndPlanta;
                rep.Planta = _lsPlanta;
                rep.IndFolio = _lsIndFolio;
                if(_lsIndFolio == "U" || _lsIndFolio == "A")
                    rep.FolioIni = long.Parse(_lsFolioIni);
                if (_lsIndFolio == "A")
                    rep.FolioFin = long.Parse(_lsFolioFin);

                rep.IndLinea = _lsIndLinea;
                if (_lsIndLinea == "U" || _lsIndLinea == "A")
                    rep.LineaIni = _lsLineaIni;
                if (_lsIndLinea == "A")
                    rep.LineaFin = _lsLineas;
                rep.IndModelo = _lsIndModelo;
                rep.Modelo = _lsModelo;
                rep.IndRPO = _lsIndRPO;
                rep.RPO = _lsRPO;
                rep.Turno = _lsTurno;
                rep.BloqPPH = _lsBloqImp;

                DataTable dtSource = new DataTable();
                if (_lsImpOperBloq == "S")
                    dtSource = ReportesLogica.CapturaLineUpBloq(rep);
                else
                    dtSource = ReportesLogica.CapturaLineUp(rep);

                rptDoc.SetDataSource(dtSource);
                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Close();
            }
        }

        private void ListarRelacionadosFusor(string _asListado)
        {
            string sConver = string.Empty;
            string sLinea = string.Empty;
            string sLineas = string.Empty;
            int iPos = 0;


            while (!string.IsNullOrEmpty(_asListado))
            {
                sLinea = string.Empty;
                iPos = _asListado.IndexOf(","); //',C03,C05,C06,'

                if (iPos != -1 && _asListado.Length > 1)
                {
                    sLinea = _asListado.Substring(0, iPos).TrimEnd();

                }
               
            }
        }

        private void CapturaLineUpResumen()
        {
            try
            {
                ReportDocument rptDoc = new ReportDocument();
                rptDoc.Load(_lsDirec + @"\Reportes\rptLineUpResumen.rpt");

                ReportesLogica rep = new ReportesLogica();

                if(_lsIndSolo == "1")
                {
                    rep.FechaIni = DateTime.Today.AddDays(1);
                    rep.FechaFin = DateTime.Today.AddDays(1);
                }
                else
                {
                    rep.FechaIni = _ldtInicio;
                    rep.FechaFin = _ldtFinal;
                }
                rep.IndPlanta = _lsIndPlanta;
                rep.Planta = _lsPlanta;
               
                rep.IndLinea = _lsIndLinea;
                if (_lsIndLinea == "U" || _lsIndLinea == "A")
                    rep.LineaIni = _lsLineaIni;
                if (_lsIndLinea == "A")
                    rep.LineaFin = _lsLineas;
                rep.IndModelo = _lsIndModelo;
                rep.Modelo = _lsModelo;
                rep.IndRPO = _lsIndRPO;
                rep.RPO = _lsRPO;
                rep.Turno = _lsTurno;
               
                DataTable data = ReportesLogica.CapturaLineUpRes(rep);

                //BUSCAR RPO CARGADOS EN ORBIS
                if (rep.Planta == "TON1")
                    rep.Planta = "1";
                if (rep.Planta == "NIC2" || rep.Planta == "NIC3")
                    rep.Planta = "2";
                if (rep.Planta == "COL")
                    rep.Planta = "4";
                if (rep.Planta == "FUS")
                    rep.Planta = "10";

                if (_lsIndLinea == "U" || _lsIndLinea == "A")
                {
                    if(!string.IsNullOrEmpty(_lsLineaIni))
                    {
                        if (_lsPlanta == "COL")
                            _lsLineaIni = _lsLineaIni.Replace("C", "CTNR");
                        if (_lsPlanta == "NIC2" || _lsPlanta == "NIC3" || _lsPlanta == "TON1")
                        {
                            int iLinea = 0;
                            if(int.TryParse(_lsLineaIni,out iLinea))
                                _lsLineaIni = "TNR" + _lsLineaIni;
                        }
                            
                        if (_lsPlanta == "NIC2")
                        {
                            if (_lsLineaIni == "TNR1")
                                _lsLineaIni = "TNR01A";
                            if (_lsLineaIni == "TNR2")
                                _lsLineaIni = "TNR02A";
                            if (_lsLineaIni == "TNR5")
                                _lsLineaIni = "TNR5A";
                            if (_lsLineaIni == "TNR6")
                                _lsLineaIni = "TNR6A";
                            if (_lsLineaIni == "TNR7")
                                _lsLineaIni = "TNR7A";
                            if (_lsLineaIni == "TNR8")
                                _lsLineaIni = "TNR8A";
                            if (_lsLineaIni == "TNR9")
                                _lsLineaIni = "TNR9A";
                            if (_lsLineaIni == "TNR16")
                                _lsLineaIni = "TNR16A";
                            if (_lsLineaIni == "TNR17")
                                _lsLineaIni = "TNR17A";
                            if (_lsLineaIni == "TNR18")
                                _lsLineaIni = "TNR18A";
                            if (_lsLineaIni == "TNR20")
                                _lsLineaIni = "TNR20A";
                            if (_lsLineaIni == "TNR21")
                                _lsLineaIni = "TNR21A";
                            if (_lsLineaIni == "TNR22")
                                _lsLineaIni = "TNR22A";
                            if (_lsLineaIni == "TNR23")
                                _lsLineaIni = "TNR23A";

                        }
                    }
                
                    rep.LineaIni = _lsLineaIni;
                }
                if (_lsIndLinea == "A")
                {
                    if(!string.IsNullOrEmpty(_lsLineas))
                    {
                        if (_lsPlanta == "COL")
                            _lsLineas = _lsLineas.Replace("C", "CTNR");
                        if (_lsPlanta == "NIC2" || _lsPlanta == "NIC3")
                        {
                            _lsLineas = _lsLineas.Substring(0, _lsLineas.Length - 1);
                            _lsLineas = _lsLineas.Replace(",", ",TNR");
                        }
                    }

                    rep.LineaFin = _lsLineas;
                }

                if (_lsIndSolo == "1")
                {
                    rep.FechaIni = _ldtInicio;
                    rep.FechaFin = _ldtFinal;
                }
                DataTable dtOrbis = ReportesLogica.CapturaLineUpResOrbis(rep);
                foreach (DataRow row in dtOrbis.Rows)
                {
                    long lOrbisFolio = long.Parse(row[0].ToString());
                    string sOrbisPta = row[1].ToString();
                    string sOrbisLin = row[2].ToString();
                    DateTime dtOrbisF = Convert.ToDateTime(row[3].ToString());
                    string sOrbisRPO = row[4].ToString().ToUpper();
                    if (sOrbisRPO.IndexOf("RPO") == -1)
                        sOrbisRPO = "RPO" + sOrbisRPO;
                    string sOrbisProd = row[5].ToString().ToUpper();
                    string sOrbisCore = row[6].ToString();
                    string sOrbisTurno = row[7].ToString();
                    string sOrbisUser = row[8].ToString().ToUpper();
                    string sOrigen = "O"; // Orbis

                    if (sOrbisPta == "1")
                        sOrbisPta = "TON1";
                    if (sOrbisPta == "2")
                        sOrbisPta = "TON2";
                    if (sOrbisPta == "4")
                        sOrbisPta = "COL";
                    if (sOrbisPta == "10")
                        sOrbisPta = "FUS";

                    string sOrbisLinea = string.Empty;
                    if (sOrbisLin.IndexOf("TNR") != -1)
                    {
                        sOrbisLinea = sOrbisLin.Replace("TNR", "");
                        sOrbisLinea = sOrbisLin.Replace("A", "");
                        if (sOrbisLinea.Substring(0, 1) == "0")
                            sOrbisLinea = sOrbisLinea.Substring(1); //quita el 0 a lineas de toner
                    }
                    
                    bool bFind = false;

                    if (_lsIndSolo == "1")
                    {
                        if (sOrbisPta == "COL")
                            sOrbisLinea = sOrbisLinea.Replace("CTNR", "C");

                        LineUpLogica lineup = new LineUpLogica();
                        lineup.Fecha = dtOrbisF;
                        lineup.RPO = sOrbisRPO;
                        lineup.Linea = sOrbisLinea;
                        lineup.Turno = sOrbisTurno;
                        if (LineUpLogica.VerificaRPO(lineup))
                            bFind = true;
                    }
                    else
                    {
                        for (int iCt = 0; iCt < data.Rows.Count; iCt++)
                        {
                            string sPlanta = data.Rows[iCt]["planta"].ToString();
                            DateTime dFecha = Convert.ToDateTime(data.Rows[iCt]["fecha"].ToString());
                            string sLinea = data.Rows[iCt]["linea_nom"].ToString();
                            string sRPO = data.Rows[iCt]["rpo"].ToString();
                            string sTurno = data.Rows[iCt]["turno"].ToString();

                            if (sPlanta == sOrbisPta && sLinea == sOrbisLin && sRPO == sOrbisRPO && sOrbisTurno == sTurno)
                            {
                                bFind = true;
                                break;
                            }
                        }
                    }
                    
                    if(bFind == false)
                    {
                        if (sOrbisPta == "TON2")
                        {
                            LineaLogica lin = new LineaLogica();
                            lin.Linea = sOrbisLinea;
                            DataTable dtL = LineaLogica.Consultar(lin);
                            if (dtL.Rows.Count > 0)
                                sOrbisPta = dtL.Rows[0]["planta"].ToString();
                        }
                        PlantaLogica pla = new PlantaLogica();
                        pla.Planta = sOrbisPta;
                        DataTable dtPta = PlantaLogica.Consultar(pla);
                        if(dtPta.Rows.Count > 0)
                        {
                            if (sOrbisPta == "COL")
                                sOrbisLinea = sOrbisLinea.Replace("CTNR", "C");
                            string sNombrePta = dtPta.Rows[0]["nombre"].ToString();
                            data.Rows.Add(sOrigen, dtOrbisF, lOrbisFolio, sOrbisPta, sNombrePta, sOrbisLinea, sOrbisLin, sOrbisRPO, sOrbisProd, sOrbisTurno, dtOrbisF, sOrbisUser, '0', null, '0', null, null, null, sOrbisCore, '0', null);
                        }
                    }
                }
                //data.DefaultView.Sort = "planta asc,linea asc,ind_tipo asc,rpo asc";
                rptDoc.SetDataSource(data);

                ////PARAMETROS
                ParameterFields paramFields = new ParameterFields();
                ParameterField paramField = new ParameterField();
                ParameterDiscreteValue discreteVal = new ParameterDiscreteValue();
                paramField.Name = "pfDesde";
                discreteVal.Value = _ldtInicio.ToString("dd/MM/yyyy");
                paramField.CurrentValues.Add(discreteVal);
                paramFields.Add(paramField);

                paramField = new ParameterField();
                discreteVal = new ParameterDiscreteValue();
                paramField.Name = "pfHasta";
                discreteVal.Value = _ldtFinal.ToString("dd/MM/yyyy");
                paramField.CurrentValues.Add(discreteVal);
                paramFields.Add(paramField);

                crystalReportViewer1.ParameterFieldInfo = paramFields;
                crystalReportViewer1.ReportSource = rptDoc;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Close();
            }
        }

        private void DiarioSetup()
        {
            try
            {
                ReportDocument rptDoc = new ReportDocument();
                if(_lsIndHorario == "1")
                    rptDoc.Load(_lsDirec + @"\Reportes\rptSetUpRegId.rpt");
                else
                {
                    if(_lsIndAct == "1")
                        rptDoc.Load(_lsDirec + @"\Reportes\rptSetUpDiario.rpt");
                    else
                    {
                        if (_lsIndRegid == "1")
                            rptDoc.Load(_lsDirec + @"\Reportes\rptSetUpDuraRegid.rpt");
                        else
                            rptDoc.Load(_lsDirec + @"\Reportes\rptSetUpDura1.rpt");
                    }
                        
                }
                    

                ReportesLogica rep = new ReportesLogica();
                rep.FechaIni = _ldtInicio;
                rep.FechaFin = _ldtFinal;
            
                rep.IndPlanta = _lsIndPlanta;
                rep.Planta = _lsPlanta;

                rep.IndLinea = _lsIndLinea;
                if (_lsIndLinea == "1")
                    rep.LineaIni = _lsLineaIni;
                rep.IndEst = _lsIndEst;
                rep.Estatus = _lsEstatus;
                rep.IndTurno = _lsIndTurno;
                rep.Turno = _lsTurno;

                DataTable data = new DataTable();

                
                data = ReportesLogica.DiarioSetUp(rep);
           

                rptDoc.SetDataSource(data);

                    ////PARAMETROS
                ParameterFields paramFields = new ParameterFields();
                ParameterField paramField = new ParameterField();
                ParameterDiscreteValue discreteVal = new ParameterDiscreteValue();
                paramField.Name = "pfDesde";
                discreteVal.Value = _ldtInicio.ToString("dd/MM/yyyy");
                paramField.CurrentValues.Add(discreteVal);
                paramFields.Add(paramField);

                paramField = new ParameterField();
                discreteVal = new ParameterDiscreteValue();
                paramField.Name = "pfHasta";
                discreteVal.Value = _ldtFinal.ToString("dd/MM/yyyy");
                paramField.CurrentValues.Add(discreteVal);
                paramFields.Add(paramField);

                crystalReportViewer1.ParameterFieldInfo = paramFields;
                crystalReportViewer1.ReportSource = rptDoc;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Close();
            }

        }
       

        private void ReporteGlobalRPO()
        {
            try
            {
                ReportDocument rptDoc = new ReportDocument();
                rptDoc.Load(_lsDirec + @"\Reportes\rptOrdenesUrgentes.rpt");

                ReportesLogica rep = new ReportesLogica();

                rep.FechaIni = _ldtInicio;
                rep.FechaFin = _ldtFinal;
                rep.IndRPO = _lsIndRPO;
                rep.RPO = _lsRPO;
                rep.IndModelo = _lsIndModelo;
                rep.Modelo = _lsModelo;
                rep.IndTurno = _lsIndTurno; // indTO
                rep.Turno = _lsTurno; // TO
                rep.Estatus = _lsIndEst; // Cancelados

                DataTable dtSource = ReportesLogica.RepGlobalRPO(rep);
                rptDoc.SetDataSource(dtSource);

                ////PARAMETROS
                ParameterFields paramFields = new ParameterFields();
                ParameterField paramField = new ParameterField();
                ParameterDiscreteValue discreteVal = new ParameterDiscreteValue();
                paramField.Name = "pfDesde";
                discreteVal.Value = _ldtInicio.ToString("dd/MM/yyyy");
                paramField.CurrentValues.Add(discreteVal);
                paramFields.Add(paramField);

                paramField = new ParameterField();
                discreteVal = new ParameterDiscreteValue();
                paramField.Name = "pfHasta";
                discreteVal.Value = _ldtFinal.ToString("dd/MM/yyyy");
                paramField.CurrentValues.Add(discreteVal);
                paramFields.Add(paramField);

                crystalReportViewer1.ParameterFieldInfo = paramFields;
                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Close();
            }
        }
        private void ReporteArmadoRPO()
        {
            try
            {
                ReportDocument rptDoc = new ReportDocument();

                if(_lsTipoEmp == "A")
                    rptDoc.Load(_lsDirec + @"\Reportes\rptEmpaqueRPOalm.rpt");
                else
                    rptDoc.Load(_lsDirec + @"\Reportes\rptEmpaqueRPOprio.rpt");

                ReportesLogica rep = new ReportesLogica();
                
                rep.FechaIni = _ldtInicio;
                rep.FechaFin = _ldtFinal;
                rep.IndPlanta = _lsIndPlanta;
                rep.Planta = _lsPlanta;
                rep.IndLinea = _lsIndLinea;
                rep.LineaIni = _lsLineaIni;
                rep.IndRPO = _lsIndRPO;
                rep.RPO = _lsRPO;
                rep.Turno = _lsTurno;
                rep.IndEst = _lsIndEst;
                rep.Estatus = _lsEstatus;

                DataTable dtSource = ReportesLogica.RepEmpaqueRPOalm(rep);
                rptDoc.SetDataSource(dtSource);

                ////PARAMETROS
                ParameterFields paramFields = new ParameterFields();
                ParameterField paramField = new ParameterField();
                ParameterDiscreteValue discreteVal = new ParameterDiscreteValue();
                paramField.Name = "pfDesde";
                discreteVal.Value = _ldtInicio.ToString("dd/MM/yyyy");
                paramField.CurrentValues.Add(discreteVal);
                paramFields.Add(paramField);

                paramField = new ParameterField();
                discreteVal = new ParameterDiscreteValue();
                paramField.Name = "pfHasta";
                discreteVal.Value = _ldtFinal.ToString("dd/MM/yyyy");
                paramField.CurrentValues.Add(discreteVal);
                paramFields.Add(paramField);

                crystalReportViewer1.ParameterFieldInfo = paramFields;
                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Close();
            }
        }
        private void DuracionSetUp()
        {
            try
            {
                ReportDocument rptDoc = new ReportDocument();
                rptDoc.Load(_lsDirec + @"\Reportes\rptSetUpDuraMP.rpt");

                ReportesLogica rep = new ReportesLogica();

                rep.FechaIni = _ldtInicio;
                rep.FechaFin = _ldtFinal;
                rep.IndPlanta = _lsIndPlanta;
                rep.Planta = _lsPlanta;
                rep.IndLinea = _lsIndLinea;
                rep.LineaIni = _lsLineaIni;

                DataTable dtSource = ReportesLogica.DuracionSetUpMP(rep);
                rptDoc.SetDataSource(dtSource);

                ////PARAMETROS
                ParameterFields paramFields = new ParameterFields();
                ParameterField paramField = new ParameterField();
                ParameterDiscreteValue discreteVal = new ParameterDiscreteValue();
                paramField.Name = "pfDesde";
                discreteVal.Value = _ldtInicio.ToString("dd/MM/yyyy");
                paramField.CurrentValues.Add(discreteVal);
                paramFields.Add(paramField);

                paramField = new ParameterField();
                discreteVal = new ParameterDiscreteValue();
                paramField.Name = "pfHasta";
                discreteVal.Value = _ldtFinal.ToString("dd/MM/yyyy");
                paramField.CurrentValues.Add(discreteVal);
                paramFields.Add(paramField);

                paramField = new ParameterField();
                discreteVal = new ParameterDiscreteValue();
                paramField.Name = "pfPlanta";
                discreteVal.Value = _lsPlanta;
                paramField.CurrentValues.Add(discreteVal);
                paramFields.Add(paramField);
                crystalReportViewer1.ParameterFieldInfo = paramFields;

                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Close();
            }
        }
        private void ReporteEmpaqueComp()
        {
            try
            {
                ReportDocument rptDoc = new ReportDocument();
                rptDoc.Load(_lsDirec + @"\Reportes\rptEtiquetaComp.rpt");

                ReportesLogica rep = new ReportesLogica();

                rep.FechaIni = _ldtInicio;
                rep.FechaFin = _ldtFinal;
                rep.IndPlanta = _lsIndPlanta;
                rep.Planta = _lsPlanta;
                rep.IndLinea = _lsIndLinea;
                rep.LineaIni = _lsLineaIni;
                rep.IndRPO = _lsIndRPO;
                rep.RPO = _lsRPO;
                rep.Turno = _lsTurno;

                DataTable dtSource = ReportesLogica.RepEtiEmpaque(rep);
                rptDoc.SetDataSource(dtSource);
                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Close();
            }
        }
        private void ReporteTransferencias()
        {
            try
            {
                ReportDocument rptDoc = new ReportDocument();
                rptDoc.Load(_lsDirec + @"\Reportes\rptTransferLine.rpt");

                ReportesLogica rep = new ReportesLogica();

                rep.FechaIni = _ldtInicio;
                rep.FechaFin = _ldtFinal;
                rep.Planta = _lsPlanta;
                rep.LineaIni = _lsLineaIni;
                rep.IndEmp = _lsIndEmp;
                rep.Empleado = _lsEmpleado;
                rep.Turno = _lsTurno;

                DataTable dtSource = ReportesLogica.RepTransferLineas(rep);
                rptDoc.SetDataSource(dtSource);
                rptDoc.SetParameterValue("prmSupervisor", _lsSupervisor);
                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Close();
            }
        }
        private void ReporteUsuarios()
        {
            try
            {
                ReportDocument rptDoc = new ReportDocument();
                rptDoc.Load(_lsDirec + @"\Reportes\rptUsuarios.rpt");

                ReportesLogica rep = new ReportesLogica();

                rep.IndPlanta = _lsIndPlanta;
                rep.Planta = _lsPlanta;
                rep.IndTurno = _lsIndTurno;
                rep.Turno = _lsTurno;
                rep.IndArea = _lsIndArea;
                rep.Area = _lsArea;
                rep.IndEmp = _lsIndEmp;
                rep.TipoEmp = _lsTipoEmp;
                rep.Empleado = _lsEmpleado;

                DataTable dtSource = ReportesLogica.ListadoUsuarios(rep);
                rptDoc.SetDataSource(dtSource);
                rptDoc.SetParameterValue("prmIndArea", rep.IndArea);
                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Close();
            }
        }

        private void ReporteSuperLine()
        {
            try
            {
                ReportDocument rptDoc = new ReportDocument();
                rptDoc.Load(_lsDirec + @"\Reportes\rptSuperline.rpt");

                ReportesLogica rep = new ReportesLogica();

                rep.IndPlanta = _lsIndPlanta;
                rep.Planta = _lsPlanta;
                rep.Turno = _lsTurno;
                rep.IndLinea = _lsIndLinea;
                rep.LineaIni = _lsLineaIni;
                rep.LineaFin = _lsLineas;
                rep.IndEmp = _lsIndEmp;
                rep.Empleado = _lsEmpleado;

                DataTable dtSource = ReportesLogica.ListadoSuperLine(rep);
                rptDoc.SetDataSource(dtSource);
                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Close();
            }
        }

        private void ReporteHistorialPPH()
        {
            try
            {
                ReportDocument rptDoc = new ReportDocument();
                rptDoc.Load(_lsDirec + @"\Reportes\rptHistoricoPPH.rpt");

                ReportesLogica rep = new ReportesLogica();

                rep.FechaIni = _ldtInicio;
                rep.FechaFin = _ldtFinal;

                rep.IndEmp = _lsIndEmp;
                rep.Empleado = _lsEmpleado;
                rep.IndNivel = _lsIndNivel;
                rep.Nivel = _lsNivel;

                rep.IndPlanta = _lsIndPlanta;
                rep.Planta = _lsPlanta;
                
                rep.IndLinea = _lsIndLinea;
                if (_lsIndLinea == "U" || _lsIndLinea == "A")
                    rep.LineaIni = _lsLineaIni;
                if (_lsIndLinea == "A")
                    rep.LineaFin = _lsLineas;

                DataTable dtSource = ReportesLogica.HistorialPPH(rep);
                rptDoc.SetDataSource(dtSource);

                ////PARAMETROS
                ParameterFields paramFields = new ParameterFields();
                ParameterField paramField = new ParameterField();
                ParameterDiscreteValue discreteVal = new ParameterDiscreteValue();
                paramField.Name = "pf_desde";
                discreteVal.Value = _ldtInicio.ToString().Substring(0, 10);
                paramField.CurrentValues.Add(discreteVal);
                paramFields.Add(paramField);

                paramField = new ParameterField();
                discreteVal = new ParameterDiscreteValue();
                paramField.Name = "pf_hasta";
                discreteVal.Value = _ldtFinal.ToString().Substring(0, 10);
                paramField.CurrentValues.Add(discreteVal);
                paramFields.Add(paramField);

                crystalReportViewer1.ParameterFieldInfo = paramFields;
                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Close();
            }
        }
    }
}
