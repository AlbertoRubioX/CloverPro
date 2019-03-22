using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Logica;

namespace CloverPro
{
    public partial class wfActividEstatusPop : Form
    {
        public string _lsProceso;
        public long _lFolio;
        public int _iConsec;
        public string _sArea;
        public string _sTipo;
        
        public wfActividEstatusPop()
        {
            InitializeComponent();
            
        }
        private void wfActividEstatusPop_Activated(object sender, EventArgs e)
        {
            txtNota.Focus();
        }
       

        private void wfActividEstatusPop_Load(object sender, EventArgs e)
        {
            try
            {
                if(_lsProceso == "EMP040") //Control de RPO empaque
                {
                    this.Height = 240;
                    btnSave.Visible = false;
                    btnEspera.Visible = false;
                    btnAsignar.Visible = false;
                    btnExit.Visible = false;
                    btnFinish.Visible = false;
                    txtNota.Visible = false;

                    ControlRpoLogica rpo = new ControlRpoLogica();
                    rpo.Folio = _lFolio;
                    rpo.Consec = _iConsec;
                    DataTable dt = ControlRpoLogica.Consultar(rpo);
                    string sEstatus = string.Empty;
                    if (_sArea == "E")
                    {
                        
                            sEstatus = dt.Rows[0]["etiqueta"].ToString();
                            if(string.IsNullOrEmpty(sEstatus))
                            {
                                btnProcess.Visible = true;
                                btnProcess.Enabled = true;
                                btnStoped.Enabled = false;
                                btnDone.Enabled = false;
                            }
                            if (sEstatus == "L")
                            {
                                btnDone.Visible = false;
                                btnProcess.Enabled = false;
                                btnStoped.Enabled = false;
                                if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, "EMP04025") == true)
                                    btnComplete.Visible = true;
                            }
                            else
                            {
                                if (sEstatus == "E")
                                {
                                    btnDone.Enabled = false;
                                    btnProcess.Enabled = false;
                                    btnStoped.Enabled = false;
                                }
                            }
                            if (sEstatus != "P")
                            {
                                if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, "EMP04090") == true)
                                {
                                    btnProcess.Enabled = true;
                                }
                            }
                        
                    }
                    if(_sArea == "A")
                    {
                        sEstatus = dt.Rows[0]["almacen"].ToString(); //almacen
                        if (string.IsNullOrEmpty(sEstatus))
                        {
                            btnProcess.Visible = true;
                            btnProcess.Enabled = true;
                            btnStoped.Enabled = true;
                            btnDone.Enabled = false;
                        }
                        else
                        {
                            if (sEstatus == "L")
                            {
                                btnDone.Enabled = false;
                                btnDone.Enabled = false;
                                btnProcess.Enabled = false;
                                btnStoped.Enabled = false;
                            }
                            if (sEstatus != "P")
                            {
                                if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "95") == true)
                                {
                                    btnProcess.Enabled = true;
                                }
                            }
                        }
                    }

                    if (_sArea == "EI") // ETIQUETAS INTERNAS
                    {
                        sEstatus = dt.Rows[0]["etiqueta_interna"].ToString();
                        if (string.IsNullOrEmpty(sEstatus))
                        {
                            btnProcess.Visible = true;
                            btnProcess.Enabled = true;
                            btnStoped.Enabled = false;
                            btnDone.Enabled = false;
                        }
                        if (sEstatus == "L")
                        {
                            btnDone.Visible = false;
                            btnProcess.Enabled = false;
                            btnStoped.Enabled = false;
                            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, "EMP04034") == true)
                                btnComplete.Visible = true;
                        }
                        else
                        {
                            if (sEstatus == "E")
                            {
                                btnDone.Enabled = false;
                                btnProcess.Enabled = false;
                                btnStoped.Enabled = false;
                            }
                        }
                        if (sEstatus != "P")
                        {
                            if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, "EMP040100") == true)
                            {
                                btnProcess.Enabled = true;
                            }
                        }
                    }//FIN ETIQUETAS INTERNAS



                    if (sEstatus == "C")
                        Close();

                    if(sEstatus == "P")
                    {
                        btnProcess.Enabled = false;
                    }
                }
                else
                {
                    btnDone.Visible = false;
                    btnProcess.Visible = false;
                    btnStoped.Visible = false;

                    LineSetActLogica act = new LineSetActLogica();
                    act.Folio = _lFolio;
                    act.Consec = _iConsec;
                    act.Actividad = _sArea;
                    DataTable dt = LineSetActLogica.ConsultarArea(act);
                    string sEst = dt.Rows[0]["estatus"].ToString();
                    string sNota = dt.Rows[0]["comentario"].ToString();

                    if (sEst == "N")
                        btnExit.Enabled = false;

                    if (sEst == "L")
                    {
                        btnSave.Visible = false;
                        btnEspera.Visible = true;
                    }
                    else
                    {
                        btnSave.Visible = true;
                        btnEspera.Visible = false;
                    }



                    if (sEst == "I")
                        btnFinish.Enabled = false;

                    if (_sArea == "SIS")
                    {
                        btnFinish.Visible = false;
                        btnAsignar.Visible = true;

                    }



                    //if (sEst == "E")
                    //{
                    //    btnFinish.Visible = false;
                    //    btnExit.Visible = true;
                    //}
                    //else
                    //{
                    //    btnExit.Visible = false;
                    //    btnFinish.Visible = true;

                    //    btnSave.Location = new Point(60, 35);
                    //    btnSave.Enabled = false;

                    //    if (sEst == "I")
                    //        btnFinish.Enabled = false;
                    //}

                    if (!string.IsNullOrEmpty(sNota))
                        txtNota.Text = sNota;

                    txtNota.Focus();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador " + Environment.NewLine + ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Close();
            }
        }
        private void txtClave_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }

            if (e.KeyCode != Keys.Enter)
                return;

        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            
            try
            {
                if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "40") == false)
                {
                    MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                LineSetActLogica act = new LineSetActLogica();
                act.Folio = _lFolio;
                act.Consec = _iConsec;
                act.Estatus = "L";
                act.Comentario = txtNota.Text.ToString();
                act.Actividad = _sArea;
                act.EntUsuario = GlobalVar.gsUsuario;

                LineSetActLogica.ActualizaEstatus(act);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), "Error en LineSetActLogica.Guardar()", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            Close();
        }
        private void btnEspera_Click(object sender, EventArgs e)
        {
            try
            {

                if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "35") == false)
                {
                    MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                LineSetActLogica act = new LineSetActLogica();
                act.Folio = _lFolio;
                act.Consec = _iConsec;
                act.Actividad = _sArea;
                act.Estatus = "E";
                act.Comentario = txtNota.Text.ToString();
                act.Usuario = GlobalVar.gsUsuario;

                LineSetActLogica.ActualizaEstatus(act);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), "Error en LineSetActLogica.Guardar()", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            Close();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            try
            {

                if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "30") == false)
                {
                    MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                LineSetActLogica act = new LineSetActLogica();
                act.Folio = _lFolio;
                act.Consec = _iConsec;
                act.Actividad = _sArea;
                act.Estatus = "N";
                act.Comentario = txtNota.Text.ToString();
                act.Usuario = GlobalVar.gsUsuario;

                LineSetActLogica.ActualizaEstatus(act);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), "Error en LineSetActLogica.Guardar()", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            Close();
        }

        private void btnAsignar_Click(object sender, EventArgs e)
        {

            try
            {

                if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "45") == false)
                {
                    MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                LineSetActLogica act = new LineSetActLogica();
                act.Folio = _lFolio;
                act.Consec = _iConsec;
                act.Actividad = _sArea;

                DataTable dt = LineSetActLogica.ConsultaAsigando(act);
                if(dt.Rows.Count > 0)
                {
                    string sAsig = dt.Rows[0]["correo_dest"].ToString();
                    DialogResult Result = MessageBox.Show("La actividad ya se encuentra asignada a " + sAsig.ToUpper() + "." + Environment.NewLine + "Desea asignar la actividad a otra persona?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (Result == DialogResult.No)
                    {
                        txtNota.Focus();
                        return;
                    }
                            
                }

                wfCapturaPopGridSetup List = new wfCapturaPopGridSetup(_lsProceso);
                List._sClave = _sArea;
                List.ShowDialog();
                if(!string.IsNullOrEmpty(List._sCorreo))
                {
                    
                    act.Asignado = "1";
                    act.CorreoDest = List._sCorreo;
                    act.Comentario = txtNota.Text.ToString();
                    act.Usuario = GlobalVar.gsUsuario;

                    LineSetActLogica.AsignarActividad(act);

                    MessageBox.Show("Se ha registrado la actividad al Contacto seleccionado", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), "Error en LineSetActLogica.Guardar()", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            
        }
        private bool EnviarCorreo()
        {
            bool bReturn = false;

            bReturn = true;

            return bReturn;
        }
        private void btnFinish_Click(object sender, EventArgs e)
        {
            try
            {

                if (UsuarioLogica.VerificarPermiso(GlobalVar.gsUsuario, _lsProceso + "50") == false)
                {
                    MessageBox.Show("Se requieren permisos para realizar la acción", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                LineSetActLogica act = new LineSetActLogica();
                act.Folio = _lFolio;
                act.Consec = _iConsec;
                act.Actividad = _sArea;
                act.Estatus = "I";
                act.Comentario = txtNota.Text.ToString();
                act.TerUsuario = GlobalVar.gsUsuario;

                LineSetActLogica.ActualizaEstatus(act);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), "Error en LineSetActLogica.Guardar()", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            Close();
        }

        private void txtNota_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void txtNota_Leave(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtNota, 0);
        }

        private void txtNota_Enter(object sender, EventArgs e)
        {
            GlobalVar.CambiaColor(txtNota, 1);
        }

        private void wfActividEstatusPop_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            try
            {

                ControlRpoLogica rpo = new ControlRpoLogica();
                rpo.Folio = _lFolio;
                rpo.Consec = _iConsec;
                rpo.Usuario = GlobalVar.gsUsuario;
                if (_sArea == "E")
                {
                    rpo.Etiqueta = "L";//LISTO
                    rpo.Usuario = GlobalVar.gsUsuario;
                    ControlRpoLogica.ActualizaEti(rpo);

                }
                if (_sArea == "EI")
                {
                    rpo.EtiquetaInterna = "L";//LISTO
                    rpo.Usuario = GlobalVar.gsUsuario;
                    ControlRpoLogica.ActualizaEtInt(rpo);
                }

                if (_sArea == "A")
                    {
                    rpo.Almacen = "L";
                    rpo.Usuario = GlobalVar.gsUsuario;
                    ControlRpoLogica.ActualizaAlma(rpo);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), "Error en LineSetActLogica.Guardar()", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            Close();
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            try
            {

                ControlRpoLogica rpo = new ControlRpoLogica();
                rpo.Folio = _lFolio;
                rpo.Consec = _iConsec;
                rpo.Usuario = GlobalVar.gsUsuario;
                if (_sArea == "E")
                {
                    rpo.Etiqueta = "P";//en proceso
                    rpo.Usuario = GlobalVar.gsUsuario;
                    ControlRpoLogica.ActualizaEti(rpo);
                }
                if (_sArea == "EI")
                {
                    rpo.EtiquetaInterna = "P";//en proceso
                    rpo.Usuario = GlobalVar.gsUsuario;
                    ControlRpoLogica.ActualizaEtInt(rpo);
                }
                if (_sArea == "A")
                {
                    //rpo.Almacen = "P";//en proceso
                    //rpo.Usuario = GlobalVar.gsUsuario;
                    //ControlRpoLogica.ActualizaAlma(rpo);

                    wfCapturaPop_1t CapPop = new wfCapturaPop_1t("");
                    CapPop._lsProceso = _lsProceso;
                    CapPop._llFolio = _lFolio;
                    CapPop._liConsec = _iConsec;
                    CapPop._lsPlanta = "EMPN";
                    CapPop._sClave = "ALMACENISTA";
                    CapPop.ShowDialog();


                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), "Error en LineSetActLogica.Guardar()", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            Close();
        }
       

        private void btnStoped_Click(object sender, EventArgs e)
        {
            try
            {

                ControlRpoLogica rpo = new ControlRpoLogica();
                rpo.Folio = _lFolio;
                rpo.Consec = _iConsec;
                rpo.Usuario = GlobalVar.gsUsuario;
                if (_sArea == "E")
                {
                    rpo.Etiqueta = "D";//detenido

                    wfCapturaPop_1t CapPop = new wfCapturaPop_1t("");
                    CapPop._lsProceso = _lsProceso;
                    CapPop._llFolio = _lFolio;
                    CapPop._liConsec = _iConsec;
                    CapPop._lsPlanta = "EMPN";
                    CapPop._sClave = "DETENIDO";
                    CapPop._lsArea = _sArea;
                    CapPop._lsTipo = _sTipo;
                    CapPop.ShowDialog();

                    if(!string.IsNullOrEmpty(CapPop._sClave))
                        rpo.EtiNota = CapPop._sClave;

                    ControlRpoLogica.ActualizaEti(rpo);
                }
                if (_sArea == "EI")
                {
                    rpo.EtiquetaInterna = "D";//detenido

                    wfCapturaPop_1t CapPop = new wfCapturaPop_1t("");
                    CapPop._lsProceso = _lsProceso;
                    CapPop._llFolio = _lFolio;
                    CapPop._liConsec = _iConsec;
                    CapPop._lsPlanta = "EMPN";
                    CapPop._sClave = "DETENIDO";
                    CapPop._lsArea = _sArea;
                    CapPop._lsTipo = _sTipo;
                    CapPop.ShowDialog();

                    if (!string.IsNullOrEmpty(CapPop._sClave))
                        rpo.EtIntNota = CapPop._sClave;

                    ControlRpoLogica.ActualizaEtInt(rpo);
                }
                if (_sArea == "A")
                {

                    DataTable dt = ControlRpoLogica.Consultar(rpo);
                    string sAlmaEst = dt.Rows[0]["almacen"].ToString();
                    if (sAlmaEst == "D")
                    {
                        MessageBox.Show("El RPO ya se encuentra Detenido", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else
                    {
                        rpo.Almacen = "D";//detenido
                        wfCapturaPop_1det CapPopD = new wfCapturaPop_1det();
                        CapPopD._lsProceso = _lsProceso;
                        CapPopD._sClave = "DETENIDO";
                        CapPopD._lsArea = _sArea;
                        CapPopD._lsTipo = _sTipo;
                        CapPopD.ShowDialog();

                        if (!string.IsNullOrEmpty(CapPopD._sClave))
                        {
                            if (CapPopD._sClave == "OTR")
                            {
                                CapPopD._sClave = CapPopD._lsParte;
                                CapPopD._lsParte = string.Empty;
                            }

                            rpo.AlmNota = CapPopD._sClave;
                            rpo.ParteDetenido = CapPopD._lsParte;
                            rpo.CantDetenido = CapPopD._ldCant;
                            rpo.Usuario = GlobalVar.gsUsuario;
                            ControlRpoLogica.ActualizaAlma(rpo);
                        }
                        else
                            MessageBox.Show("No puede Detener un RPO sin Especificar el Motivo", "Error de Captura", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), "Error en LineSetActLogica.Guardar()", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            Close();
        }

        private void btnComplete_Click(object sender, EventArgs e)
        {
            try
            {

                ControlRpoLogica rpo = new ControlRpoLogica();
                rpo.Folio = _lFolio;
                rpo.Consec = _iConsec;
                rpo.Usuario = GlobalVar.gsUsuario;
                if (_sArea == "E")
                {
                    //DataTable dt = ControlRpoLogica.Consultar(rpo);
                    //string sAlm = dt.Rows[0]["almacen"].ToString();
                    //if (sAlm != "L")
                    //{
                    //    MessageBox.Show("No puede entregar etiquetas debido a que Almacen tiene estatus LISTO", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    return;
                    //}

                    wfCapturaPop_1t CapPop = new wfCapturaPop_1t("");
                    CapPop._lsProceso = _lsProceso;
                    CapPop._llFolio = _lFolio;
                    CapPop._liConsec = _iConsec;
                    CapPop._lsPlanta = "EMPN";
                    CapPop._sClave = "ENTREGA";
                    CapPop.ShowDialog();

                    if (string.IsNullOrEmpty(CapPop._sClave))
                    {
                        MessageBox.Show("Favor de registrar el materialista que recibe las etiquetas", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    rpo.Etiqueta = "E";//ENTREGADO -> LIBERA LOCACION
                    rpo.Almacen = "L";
                    int iPos = CapPop._sClave.IndexOf(":");
                    if (iPos == -1)//TRIGGER GET NAME
                        rpo.Entrega = CapPop._sClave; // MATERIALISTA QUE RECIBE LA ETIQUETA
                    else
                    {//GET DATA FROM TRESS
                        string sClave = CapPop._sClave.Substring(0, iPos);
                        string sNombre = CapPop._sClave.Substring(iPos + 1);
                        rpo.Entrega = sClave;
                        rpo.NombreOper = sNombre;
                    }
                    ControlRpoLogica.ActualizaEti(rpo);
                    //1.1.1.74 GGUILLEN
                    ControlRpoLogica.ActualizaAlma(rpo);
                }
                if (_sArea == "EI")
                {
                    //DataTable dt = ControlRpoLogica.Consultar(rpo);
                    //string sAlm = dt.Rows[0]["almacen"].ToString();
                    //if (sAlm != "L")
                    //{
                    //    MessageBox.Show("No puede entregar etiquetas debido a que Almacen tiene estatus LISTO", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    return;
                    //}

                    //wfCapturaPop_1t CapPop = new wfCapturaPop_1t("");
                    //CapPop._lsProceso = _lsProceso;
                    //CapPop._llFolio = _lFolio;
                    //CapPop._liConsec = _iConsec;
                    //CapPop._lsPlanta = "EMPN";
                    //CapPop._sClave = "ENTREGA";
                    //CapPop.ShowDialog();

                    //if (string.IsNullOrEmpty(CapPop._sClave))
                    //{
                    //    MessageBox.Show("Favor de registrar el materialista que recibe las etiquetas", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    //    return;
                    //}

                    rpo.EtiquetaInterna = "E";//ENTREGADO -> LIBERA LOCACION
                    rpo.Almacen = "L";
                    //int iPos = CapPop._sClave.IndexOf(":");
                    //if (iPos == -1)//TRIGGER GET NAME
                    //    rpo.Entrega = CapPop._sClave; // MATERIALISTA QUE RECIBE LA ETIQUETA
                    //else
                    //{//GET DATA FROM TRESS
                    //    string sClave = CapPop._sClave.Substring(0, iPos);
                    //    string sNombre = CapPop._sClave.Substring(iPos + 1);
                    //    rpo.Entrega = sClave;
                    //    rpo.NombreOper = sNombre;
                    //}
                    ControlRpoLogica.ActualizaEtInt(rpo);
                    //1.1.1.74 GGUILLEN
                    ControlRpoLogica.ActualizaAlma(rpo);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Favor de Notificar al Administrador" + Environment.NewLine + ex.ToString(), "Error en LineSetActLogica.Guardar()", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            Close();
        }
    }
}
