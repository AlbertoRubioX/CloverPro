using Logica;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CloverPro
{
    public partial class wfRepGraficaRPO : Form
    {
        private string _lsProceso = "REP130";

        public wfRepGraficaRPO()
        {
            InitializeComponent();

        }
        private void wfRepGraficaRPO_Load(object sender, EventArgs e)
        {
            inicio();

        }


        private void inicio()
        {
            cbbPlanta.ResetText();
            DataTable data = PlantaLogica.Listar();
            cbbPlanta.DataSource = data;
            cbbPlanta.ValueMember = "planta";
            cbbPlanta.DisplayMember = "nombre";
            cbbPlanta.SelectedIndex = -1;

            dtpFecha.ResetText();

            if (GlobalVar.gsTurno == "2")
            {
                rbtTuno1.Checked = false;
                rbtTuno2.Checked = true;
            }
            else
            {
                rbtTuno1.Checked = true;
                rbtTuno2.Checked = false;
            }
        }

        //Accion de "btnPrint (Vista Previa)" para la generacion del documento del menu superior
        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (!Valida())
                return;

            wfImpresor Impresor = new wfImpresor();
            Impresor._lsProceso = _lsProceso;

            //rbtArmaNoArma si esta habilitado se asignara "1" al valor de la variable "_tipografico"
            //En caso de que no lo este y este hablilitado "rbtEnEsperaDet" se le asignara el valor "2"
            //_tipografico="0" ==> El grafico sera de Armados y no Armados
            //_tipografico="1" ==> El grafico sera En espera y detenidos

            if (rbtArmaNoArma.Checked)
                Impresor._tipografico = "0";
            if (rbtEnEsperaDet.Checked)
                Impresor._tipografico = "1";
            //Se obtiene la planta
            Impresor._lsPlanta = cbbPlanta.SelectedValue.ToString();
            //Se obtiene la fecha
            Impresor._Fecha = dtpFecha.Value;


            //De acuerdo al radiobutton que este seleccionado se dara el valor del turno
            //_lsTurno="1" ==> Se generará el reporte de acuerdo al Turno 1
            //_lsTurno="2" ==> Se generará el reporte de acuerdo al Turno 2
            //_lsTurno="A" ==> Se generará el reporte de acuerdo a Ambos turnos
            if (rbtTuno1.Checked)
                Impresor._lsTurno = "1";
            if(rbtTuno2.Checked)
                Impresor._lsTurno = "2";
            if (rbtAmbos.Checked)
                Impresor._lsTurno = "A";


            //De acuerdo al radioButton que este seleccionado se elegira el tipo de grafica que desea desplegar
            //_lsMedido="C" ==> Se generara el reporte de acuerdo a la cantida de cartuchos producidos
            //_lsMedido="RPO" ==> Se generara el reporte de acuerdo a la cantidad de RPOs
            if (rbtCantidad.Checked)
                Impresor._lsMedido = "C";
            if(rbtRPO.Checked)
                Impresor._lsMedido = "RPO";

            Impresor.Show();

        }

        //Accion de "btnNew (Nuevo)" para la generacion del documento del menu superior
        private void btnNew_Click(object sender, EventArgs e)
        {
            inicio();

        }

        private bool Valida()
        {
            bool bValida = false;

            if (string.IsNullOrEmpty(dtpFecha.Text))
            {
                MessageBox.Show("Favor de especificar la fecha del reporte", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtpFecha.Focus();
                return bValida;
            }
          

            if (cbbPlanta.SelectedIndex == -1)
            {
                MessageBox.Show("No se a especificado la planta", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbbPlanta.Focus();
                return bValida;
            }
           

            return true;

        }

        //Accion de "btExit (Salir)" del menu superior
        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        
    }
}
