
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SistemaReservas
{
    public partial class FrmClientes : Form
    {
        string conexion = "Server=localhost;Database=sistema_reservas;Uid=root;Pwd=TU_CLAVE;";

        int idClienteSeleccionado = 0;

        TextBox txtNombres = new TextBox();
        TextBox txtTelefono = new TextBox();
        TextBox txtCorreo = new TextBox();

        DataGridView dgvClientes = new DataGridView();

        Button btnGuardar = new Button();
        Button btnEliminar = new Button();
        Button btnLimpiar = new Button();
        Button btnActualizar = new Button();

        public FrmClientes()
        {
            InitializeComponent();
            CrearInterfaz();
            CargarClientes();
        }

        // =====================================
        // CREAR INTERFAZ
        // =====================================

        private void CrearInterfaz()
        {
            this.Text = "Gestión de Clientes";
            this.Size = new Size(950, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            Label titulo = new Label();
            titulo.Text = "GESTIÓN DE CLIENTES";
            titulo.Font = new Font("Arial", 22, FontStyle.Bold);
            titulo.ForeColor = Color.DarkBlue;
            titulo.Location = new Point(30, 20);
            titulo.AutoSize = true;
            this.Controls.Add(titulo);

            // NOMBRES
            Label lblNombres = new Label();
            lblNombres.Text = "Nombres:";
            lblNombres.Location = new Point(40, 100);
            lblNombres.AutoSize = true;
            this.Controls.Add(lblNombres);

            txtNombres.Location = new Point(160, 95);
            txtNombres.Width = 230;
            this.Controls.Add(txtNombres);

            // TELEFONO
            Label lblTelefono = new Label();
            lblTelefono.Text = "Teléfono:";
            lblTelefono.Location = new Point(40, 145);
            lblTelefono.AutoSize = true;
            this.Controls.Add(lblTelefono);

            txtTelefono.Location = new Point(160, 140);
            txtTelefono.Width = 230;
            this.Controls.Add(txtTelefono);

            // CORREO
            Label lblCorreo = new Label();
            lblCorreo.Text = "Correo electrónico:";
            lblCorreo.Location = new Point(40, 190);
            lblCorreo.AutoSize = true;
            this.Controls.Add(lblCorreo);

            txtCorreo.Location = new Point(160, 185);
            txtCorreo.Width = 230;
            this.Controls.Add(txtCorreo);

            // BOTON GUARDAR
            btnGuardar.Text = "GUARDAR";
            btnGuardar.Location = new Point(450, 95);
            btnGuardar.Size = new Size(130, 40);
            btnGuardar.BackColor = Color.Green;
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Click += BtnGuardar_Click;
            this.Controls.Add(btnGuardar);

            // BOTON ELIMINAR
            btnEliminar.Text = "ELIMINAR";
            btnEliminar.Location = new Point(600, 95);
            btnEliminar.Size = new Size(130, 40);
            btnEliminar.BackColor = Color.Red;
            btnEliminar.ForeColor = Color.White;
            btnEliminar.Click += BtnEliminar_Click;
            this.Controls.Add(btnEliminar);

            // BOTON LIMPIAR
            btnLimpiar.Text = "LIMPIAR";
            btnLimpiar.Location = new Point(450, 150);
            btnLimpiar.Size = new Size(130, 40);
            btnLimpiar.Click += BtnLimpiar_Click;
            this.Controls.Add(btnLimpiar);

            // BOTON ACTUALIZAR
            btnActualizar.Text = "ACTUALIZAR";
            btnActualizar.Location = new Point(600, 150);
            btnActualizar.Size = new Size(130, 40);
            btnActualizar.Click += BtnActualizar_Click;
            this.Controls.Add(btnActualizar);

            // TABLA CLIENTES
            dgvClientes.Location = new Point(30, 270);
            dgvClientes.Size = new Size(870, 260);
            dgvClientes.ReadOnly = true;
            dgvClientes.AllowUserToAddRows = false;
            dgvClientes.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvClientes.MultiSelect = false;
            dgvClientes.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            dgvClientes.CellClick += DgvClientes_CellClick;

            this.Controls.Add(dgvClientes);
        }

        // =====================================
        // INSERTAR CLIENTE
        // =====================================

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (txtNombres.Text.Trim() == "" ||
                txtTelefono.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese los nombres y el teléfono.");
                return;
            }

            if (txtNombres.Text.Trim().Length > 100 ||
                txtTelefono.Text.Trim().Length > 20 ||
                txtCorreo.Text.Trim().Length > 100)
            {
                MessageBox.Show("Verifique la longitud de los datos.");
                return;
            }

            if (txtCorreo.Text.Trim() != "" &&
                !System.Text.RegularExpressions.Regex.IsMatch(
                    txtCorreo.Text.Trim(),
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Ingrese un correo electrónico válido.");
                return;
            }

            try
            {
                using (MySqlConnection cn =
                    new MySqlConnection(conexion))
                {
                    cn.Open();

                    using (MySqlCommand cmd =
                        new MySqlCommand("InsertarCliente", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue(
                            "p_nombres", txtNombres.Text.Trim());

                        cmd.Parameters.AddWithValue(
                            "p_telefono", txtTelefono.Text.Trim());

                        cmd.Parameters.AddWithValue(
                            "p_correo",
                            string.IsNullOrWhiteSpace(txtCorreo.Text)
                                ? (object)DBNull.Value
                                : txtCorreo.Text.Trim());

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Cliente registrado correctamente.");

                Limpiar();
                CargarClientes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message);
            }
        }

        // =====================================
        // CONSULTAR CLIENTES
        // =====================================

        private void CargarClientes()
        {
            try
            {
                using (MySqlConnection cn =
                    new MySqlConnection(conexion))
                {
                    cn.Open();

                    using (MySqlCommand cmd =
                        new MySqlCommand("SeleccionarClientes", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        DataTable tabla = new DataTable();

                        using (MySqlDataAdapter adapter =
                            new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(tabla);
                        }

                        dgvClientes.DataSource = tabla;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al consultar: " + ex.Message);
            }
        }

        // =====================================
        // SELECCIONAR CLIENTE
        // =====================================

        private void DgvClientes_CellClick(
            object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila =
                    dgvClientes.Rows[e.RowIndex];

                idClienteSeleccionado =
                    Convert.ToInt32(fila.Cells["id_cliente"].Value);

                txtNombres.Text =
                    fila.Cells["nombres"].Value.ToString();

                txtTelefono.Text =
                    fila.Cells["telefono"].Value.ToString();

                txtCorreo.Text =
                    fila.Cells["correo"].Value == DBNull.Value
                    ? ""
                    : fila.Cells["correo"].Value.ToString();
            }
        }

        // =====================================
        // ELIMINAR CLIENTE
        // =====================================

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (idClienteSeleccionado == 0)
            {
                MessageBox.Show("Seleccione un cliente de la tabla.");
                return;
            }

            DialogResult respuesta = MessageBox.Show(
                "¿Está seguro de eliminar este cliente?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (respuesta == DialogResult.Yes)
            {
                try
                {
                    using (MySqlConnection cn =
                        new MySqlConnection(conexion))
                    {
                        cn.Open();

                        using (MySqlCommand cmd =
                            new MySqlCommand("EliminarCliente", cn))
                        {
                            cmd.CommandType =
                                CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue(
                                "p_id", idClienteSeleccionado);

                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Cliente eliminado correctamente.");

                    Limpiar();
                    CargarClientes();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "No se pudo eliminar el cliente. " +
                        "Puede tener reservaciones asociadas.\n" +
                        ex.Message);
                }
            }
        }

        // =====================================
        // ACTUALIZAR
        // =====================================

        private void BtnActualizar_Click(object sender, EventArgs e)
        {
            CargarClientes();
            MessageBox.Show("Lista de clientes actualizada.");
        }

        // =====================================
        // LIMPIAR CAMPOS
        // =====================================

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void Limpiar()
        {
            txtNombres.Clear();
            txtTelefono.Clear();
            txtCorreo.Clear();

            idClienteSeleccionado = 0;
            dgvClientes.ClearSelection();
        }
    }
}
