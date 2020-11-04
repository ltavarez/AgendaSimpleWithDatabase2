using BusinessLayer;
using Database.Modelos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;
using AgendaSimple.CustomControlItem;

namespace AgendaSimple
{
    public partial class FrmAgendaSimple : Form
    {

        private readonly ServicioPersona _servicio;
        private readonly ServicioTipoContacto _servicioTipoContacto; 
        public int _id;
        public FrmAgendaSimple()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);


            _servicio = new ServicioPersona(connection);
            _servicioTipoContacto = new ServicioTipoContacto(connection);
            _id = 0;
        }

        #region "Eventos"

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (_id == 0)
            {
                AddPersona();
            }
            else
            {
                EditPersona();
            }

        }

        private void FrmAgendaSimple_Load(object sender, EventArgs e)
        {
            LoadPersona();
            LoadComboBox();
        }

        private void DgvPersonas_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                _id = Convert.ToInt32(DgvPersonas.Rows[e.RowIndex].Cells[0].Value.ToString());
                BtnDeseleccionar.Visible = true;

                Persona personaEditar = new Persona();

                personaEditar = _servicio.GetById(_id);

                CbxTipoContacto.SelectedIndex = CbxTipoContacto.FindStringExact(personaEditar.TipoContacto);

                TxtNombre.Text = personaEditar.Nombre;
                TxtApellido.Text = personaEditar.Apellido;
                TxtTelefono.Text = personaEditar.Telefono;

            }
        }

        private void BtnDeseleccionar_Click(object sender, EventArgs e)
        {
            Deselect();
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            EliminarPersona();
        }

        private void cerrarSessionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("hice click en cerrar session", "Ejemplo");
        }


        #endregion

        #region "Metodos privados"

        private void LoadPersona()
        {
            DgvPersonas.DataSource = _servicio.GetAll();
            DgvPersonas.ClearSelection();

        }

        private void AddPersona()
        {
            ComboBoxItem selectedItem = CbxTipoContacto.SelectedItem as ComboBoxItem;

            Persona persona = new Persona();
            persona.Nombre = TxtNombre.Text;
            persona.Apellido = TxtApellido.Text;
            persona.Telefono = TxtTelefono.Text;
            persona.IdTipoContacto = Convert.ToInt32(selectedItem.Value); 

            bool result = _servicio.Add(persona);

            if (result)
            {
                MessageBox.Show("Se ha guardado con exito", "Notificacion");
            }
            else
            {
                MessageBox.Show("Ha sucedido un error", "Error");
            }

            ClearData();
            LoadPersona();
        }

        private void LoadComboBox()
        {
            ComboBoxItem opcionPorDefecto = new ComboBoxItem
            {
                Text = "Seleccione una opcion",
                Value = null
            };

            CbxTipoContacto.Items.Add(opcionPorDefecto);

            List<TipoContacto> listaTipo = _servicioTipoContacto.GetList();

            foreach (TipoContacto item in listaTipo)
            {

                ComboBoxItem comboItem = new ComboBoxItem
                {
                    Text = item.Name,
                    Value = item.Id
                };

                CbxTipoContacto.Items.Add(comboItem);

            }

            CbxTipoContacto.SelectedItem = opcionPorDefecto;

        }
        private void EditPersona()
        {
            ComboBoxItem selectedItem = CbxTipoContacto.SelectedItem as ComboBoxItem;
            Persona persona = new Persona();
            persona.Id = _id;
            persona.Nombre = TxtNombre.Text;
            persona.Apellido = TxtApellido.Text;
            persona.Telefono = TxtTelefono.Text;
            persona.IdTipoContacto = Convert.ToInt32(selectedItem.Value); 

            bool result = _servicio.Edit(persona);

            if (result)
            {
                MessageBox.Show("Se ha guardado con exito", "Notificacion");
            }
            else
            {
                MessageBox.Show("Ha sucedido un error", "Error");
            }


            ClearData();
            Deselect();
            LoadPersona();
        }

        private void Deselect()
        {
            DgvPersonas.ClearSelection();
            _id = 0;
            BtnDeseleccionar.Visible = false;
            ClearData();
        }

        private void ClearData()
        {
            TxtNombre.Clear();
            TxtApellido.Clear();
            TxtTelefono.Clear();

            CbxTipoContacto.SelectedIndex = 0;

        }

        private void EliminarPersona()
        {

            if (_id == 0)
            {
                MessageBox.Show("Debe seleccionar una persona", "Notificacion");
            }
            else
            {

                DialogResult respuesta = MessageBox.Show("Esta seguro que desea eliminar este contacto", "Advertencia",
                    MessageBoxButtons.OKCancel);

                if (respuesta == DialogResult.OK)
                {
                    bool result = _servicio.Delete(_id);

                    if (result)
                    {
                        MessageBox.Show("Se ha eliminado con exito", "Notificacion");
                    }
                    else
                    {
                        MessageBox.Show("Ha sucedido un error", "Error");
                    }

                    LoadPersona();
                    Deselect();
                }

            }

        }





        #endregion


    }
}
