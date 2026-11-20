
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SistemaReservas
{
    public partial class FrmReservaciones : Form
    {
        string conexion = "Server=localhost;Database=sistema_reservas;Uid=root;Pwd=TU_CLAVE;";

        int idReservacionSeleccionada = 0;

        ComboBox cmbCliente = new ComboBox();
        ComboBox cmbMesa = new ComboBox();

        DateTimePicker dtpFecha = new DateTimePicker();
        DateTimePicker dtpHora = new DateTimePicker();

        DataGridView dgvReservaciones = new DataGridView();

        Button btnGuardar = new Button();
        Button btnEliminar = new Button();
        Button btnLimpiar = new Button();
        Button btnActualizar = new Button();

        public FrmReservaciones()
        {
            InitializeComponent();
            CrearInterfaz();
            CargarClientes();
            CargarMesas();
            CargarReservaciones();
        }

        // =====================================
        // CREAR INTERFAZ
        // =====================================

        private void CrearInterfaz()
        {
            this.Text = "Gestión de Reservaciones";
            this.Size = new Size(1050, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            Label titulo = new Label();
            titulo.Text = "GESTIÓN DE RESERVACIONES";
            titulo.Font = new Font("Arial", 22, FontStyle.Bold);
            titulo.ForeColor = Color.DarkBlue;
            titulo.Location = new Point(30, 20);
            titulo.AutoSize = true;
            this.Controls.Add(titulo);

            // CLIENTE
            Label lblCliente = new Label();
            lblCliente.Text = "Cliente:";
            lblCliente.Location = new Point(40, 95);
            lblCliente.AutoSize = true;
            this.Controls.Add(lblCliente);

            cmbCliente.Location = new Point(180, 90);
            cmbCliente.Width = 230;
            cmbCliente.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Controls.Add(cmbCliente);

            // MESA
            Label lblMesa = new Label();
            lblMesa.Text = "Mesa:";
            lblMesa.Location = new Point(40, 140);
            lblMesa.AutoSize = true;
            this.Controls.Add(lblMesa);

            cmbMesa.Location = new Point(180, 135);
            cmbMesa.Width = 230;
            cmbMesa.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Controls.Add(cmbMesa);

            // FECHA
            Label lblFecha = new Label();
            lblFecha.Text = "Fecha de reserva:";
            lblFecha.Location = new Point(40, 185);
            lblFecha.AutoSize = true;
            this.Controls.Add(lblFecha);

            dtpFecha.Location = new Point(180, 180);
            dtpFecha.Width = 230;
            dtpFecha.Format = DateTimePickerFormat.Short;
            dtpFecha.MinDate = DateTime.Today;
            this.Controls.Add(dtpFecha);

            // HORA
            Label lblHora = new Label();
            lblHora.Text = "Hora:";
            lblHora.Location = new Point(40, 230);
            lblHora.AutoSize = true;
            this.Controls.Add(lblHora);

            dtpHora.Location = new Point(180, 225);
            dtpHora.Width = 230;
            dtpHora.Format = DateTimePickerFormat.Time;
            dtpHora.ShowUpDown = true;
            this.Controls.Add(dtpHora);

            // BOTON GUARDAR
            btnGuardar.Text = "GUARDAR";
            btnGuardar.Location = new Point(470, 90);
            btnGuardar.Size = new Size(130, 40);
            btnGuardar.BackColor = Color.Green;
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Click += BtnGuardar_Click;
            this.Controls.Add(btnGuardar);

            // BOTON ELIMINAR
            btnEliminar.Text = "ELIMINAR";
            btnEliminar.Location = new Point(620, 90);
            btnEliminar.Size = new Size(130, 40);
            btnEliminar.BackColor = Color.Red;
            btnEliminar.ForeColor = Color.White;
            btnEliminar.Click += BtnEliminar_Click;
            this.Controls.Add(btnEliminar);

            // BOTON LIMPIAR
            btnLimpiar.Text = "LIMPIAR";
            btnLimpiar.Location = new Point(470, 145);
            btnLimpiar.Size = new Size(130, 40);
            btnLimpiar.Click += BtnLimpiar_Click;
            this.Controls.Add(btnLimpiar);

            // BOTON ACTUALIZAR
            btnActualizar.Text = "ACTUALIZAR";
            btnActualizar.Location = new Point(620, 145);
            btnActualizar.Size = new Size(130, 40);
            btnActualizar.Click += BtnActualizar_Click;
            this.Controls.Add(btnActualizar);

            // TABLA
            dgvReservaciones.Location = new Point(30, 300);
            dgvReservaciones.Size = new Size(960, 280);
            dgvReservaciones.ReadOnly = true;
            dgvReservaciones.AllowUserToAddRows = false;
            dgvReservaciones.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvReservaciones.MultiSelect = false;
            dgvReservaciones.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            dgvReservaciones.CellClick += DgvReservaciones_CellClick;

            this.Controls.Add(dgvReservaciones);
        }

        // =====================================
        // CARGAR CLIENTES
        // =====================================

        private void CargarClientes()
        {
            try
            {
                using (MySqlConnection cn = new MySqlConnection(conexion))
                {
                    cn.Open();

                    string sql = "SELECT id_cliente, nombres FROM Clientes";

                    using (MySqlDataAdapter adapter =
                        new MySqlDataAdapter(sql, cn))
                    {
                        DataTable tabla = new DataTable();
                        adapter.Fill(tabla);

                        cmbCliente.DataSource = tabla;
                        cmbCliente.DisplayMember = "nombres";
                        cmbCliente.ValueMember = "id_cliente";
                        cmbCliente.SelectedIndex = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar clientes: " + ex.Message);
            }
        }

        // =====================================
        // CARGAR MESAS
        // =====================================

        private void CargarMesas()
        {
            try
            {
                using (MySqlConnection cn = new MySqlConnection(conexion))
                {
                    cn.Open();

                    string sql = "SELECT id_mesa, numero_mesa FROM Mesas";

                    using (MySqlDataAdapter adapter =
                        new MySqlDataAdapter(sql, cn))
                    {
                        DataTable tabla = new DataTable();
                        adapter.Fill(tabla);

                        cmbMesa.DataSource = tabla;
                        cmbMesa.DisplayMember = "numero_mesa";
                        cmbMesa.ValueMember = "id_mesa";
                        cmbMesa.SelectedIndex = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar mesas: " + ex.Message);
            }
        }

        // =====================================
        // INSERTAR RESERVACION
        // =====================================

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (cmbCliente.SelectedIndex == -1 ||
                cmbMesa.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione un cliente y una mesa.");
                return;
            }

            if (dtpFecha.Value.Date < DateTime.Today)
            {
                MessageBox.Show("Seleccione una fecha válida.");
                return;
            }

            try
            {
                using (MySqlConnection cn = new MySqlConnection(conexion))
                {
                    cn.Open();

                    using (MySqlCommand cmd =
                        new MySqlCommand("InsertarReservacion", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue(
                            "p_cliente",
                            cmbCliente.SelectedValue);

                        cmd.Parameters.AddWithValue(
                            "p_mesa",
                            cmbMesa.SelectedValue);

                        cmd.Parameters.AddWithValue(
                            "p_fecha",
                            dtpFecha.Value.Date);

                        cmd.Parameters.AddWithValue(
                            "p_hora",
                            dtpHora.Value.TimeOfDay);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Reservación registrada correctamente.");

                Limpiar();
                CargarReservaciones();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al registrar la reservación. " +
                    "Compruebe que la mesa no esté reservada " +
                    "en esa misma fecha y hora.\n" + ex.Message);
            }
        }

        // =====================================
        // CONSULTAR RESERVACIONES
        // =====================================

        private void CargarReservaciones()
        {
            try
            {
                using (MySqlConnection cn = new MySqlConnection(conexion))
                {
                    cn.Open();

                    using (MySqlCommand cmd =
                        new MySqlCommand("SeleccionarReservaciones", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        DataTable tabla = new DataTable();

                        using (MySqlDataAdapter adapter =
                            new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(tabla);
                        }

                        dgvReservaciones.DataSource = tabla;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al consultar: " + ex.Message);
            }
        }

        // =====================================
        // SELECCIONAR RESERVACION
        // =====================================

        private void DgvReservaciones_CellClick(
            object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila =
                    dgvReservaciones.Rows[e.RowIndex];

                idReservacionSeleccionada =
                    Convert.ToInt32(
                        fila.Cells["id_reservacion"].Value);

                // Seleccionar cliente
                string cliente =
                    fila.Cells["cliente"].Value.ToString();

                cmbCliente.Text = cliente;

                // Seleccionar mesa
                string mesa =
                    fila.Cells["mesa"].Value.ToString();

                cmbMesa.Text = mesa;

                // Seleccionar fecha
                dtpFecha.Value = Convert.ToDateTime(
                    fila.Cells["fecha_reserva"].Value);

                // Seleccionar hora
                dtpHora.Value = DateTime.Today.Add(
                    TimeSpan.Parse(
                        fila.Cells["hora_reserva"].Value.ToString()));
            }
        }

        // =====================================
        // ELIMINAR RESERVACION
        // =====================================

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (idReservacionSeleccionada == 0)
            {
                MessageBox.Show("Seleccione una reservación.");
                return;
            }

            DialogResult respuesta = MessageBox.Show(
                "¿Desea eliminar esta reservación?",
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
                            new MySqlCommand("EliminarReservacion", cn))
                        {
                            cmd.CommandType =
                                CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue(
                                "p_id", idReservacionSeleccionada);

                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Reservación eliminada correctamente.");

                    Limpiar();
                    CargarReservaciones();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar: " + ex.Message);
                }
            }
        }

        // =====================================
        // ACTUALIZAR
        // =====================================

        private void BtnActualizar_Click(object sender, EventArgs e)
        {
            CargarClientes();
            CargarMesas();
            CargarReservaciones();

            MessageBox.Show("Información actualizada.");
        }

        // =====================================
        // LIMPIAR
        // =====================================

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void Limpiar()
        {
            cmbCliente.SelectedIndex = -1;
            cmbMesa.SelectedIndex = -1;

            dtpFecha.Value = DateTime.Today;
            dtpHora.Value = DateTime.Today;

            idReservacionSeleccionada = 0;

            dgvReservaciones.ClearSelection();
        }
    }
}
