
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SistemaReservas
{
    public partial class FrmMesas : Form
    {
        // CONEXION A MYSQL
        string conexion = "Server=localhost;Database=sistema_reservas;Uid=root;Pwd=TU_CLAVE;";

        int idMesaSeleccionada = 0;

        public FrmMesas()
        {
            InitializeComponent();
            CrearInterfaz();
            CargarMesas();
        }

        // =====================================
        // CREAR INTERFAZ
        // =====================================

        TextBox txtNumero = new TextBox();
        TextBox txtCapacidad = new TextBox();
        DataGridView dgvMesas = new DataGridView();

        Button btnGuardar = new Button();
        Button btnEliminar = new Button();
        Button btnLimpiar = new Button();
        Button btnActualizar = new Button();

        private void CrearInterfaz()
        {
            this.Text = "Administración de Mesas";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            Label titulo = new Label();
            titulo.Text = "GESTIÓN DE MESAS";
            titulo.Font = new Font("Arial", 22, FontStyle.Bold);
            titulo.ForeColor = Color.DarkBlue;
            titulo.Location = new Point(30, 20);
            titulo.AutoSize = true;
            this.Controls.Add(titulo);

            // NUMERO DE MESA
            Label lblNumero = new Label();
            lblNumero.Text = "Número de mesa:";
            lblNumero.Location = new Point(40, 100);
            lblNumero.AutoSize = true;
            this.Controls.Add(lblNumero);

            txtNumero.Location = new Point(180, 95);
            txtNumero.Width = 200;
            this.Controls.Add(txtNumero);

            // CAPACIDAD
            Label lblCapacidad = new Label();
            lblCapacidad.Text = "Capacidad:";
            lblCapacidad.Location = new Point(40, 145);
            lblCapacidad.AutoSize = true;
            this.Controls.Add(lblCapacidad);

            txtCapacidad.Location = new Point(180, 140);
            txtCapacidad.Width = 200;
            this.Controls.Add(txtCapacidad);

            // BOTON GUARDAR
            btnGuardar.Text = "GUARDAR";
            btnGuardar.Location = new Point(420, 95);
            btnGuardar.Size = new Size(120, 35);
            btnGuardar.BackColor = Color.Green;
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Click += BtnGuardar_Click;
            this.Controls.Add(btnGuardar);

            // BOTON ELIMINAR
            btnEliminar.Text = "ELIMINAR";
            btnEliminar.Location = new Point(420, 140);
            btnEliminar.Size = new Size(120, 35);
            btnEliminar.BackColor = Color.Red;
            btnEliminar.ForeColor = Color.White;
            btnEliminar.Click += BtnEliminar_Click;
            this.Controls.Add(btnEliminar);

            // BOTON LIMPIAR
            btnLimpiar.Text = "LIMPIAR";
            btnLimpiar.Location = new Point(560, 95);
            btnLimpiar.Size = new Size(120, 35);
            btnLimpiar.Click += BtnLimpiar_Click;
            this.Controls.Add(btnLimpiar);

            // BOTON ACTUALIZAR
            btnActualizar.Text = "ACTUALIZAR";
            btnActualizar.Location = new Point(560, 140);
            btnActualizar.Size = new Size(120, 35);
            btnActualizar.Click += BtnActualizar_Click;
            this.Controls.Add(btnActualizar);

            // TABLA
            dgvMesas.Location = new Point(30, 220);
            dgvMesas.Size = new Size(820, 300);
            dgvMesas.ReadOnly = true;
            dgvMesas.AllowUserToAddRows = false;
            dgvMesas.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvMesas.MultiSelect = false;
            dgvMesas.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            dgvMesas.CellClick += DgvMesas_CellClick;

            this.Controls.Add(dgvMesas);
        }

        // =====================================
        // INSERTAR MESA
        // =====================================

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (txtNumero.Text.Trim() == "" ||
                txtCapacidad.Text.Trim() == "")
            {
                MessageBox.Show("Complete todos los campos.");
                return;
            }

            if (!int.TryParse(txtNumero.Text, out int numero) ||
                !int.TryParse(txtCapacidad.Text, out int capacidad) ||
                numero <= 0 || capacidad <= 0)
            {
                MessageBox.Show("Ingrese números enteros positivos.");
                return;
            }

            try
            {
                using (MySqlConnection cn =
                    new MySqlConnection(conexion))
                {
                    cn.Open();

                    using (MySqlCommand cmd =
                        new MySqlCommand("InsertarMesa", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("p_numero", numero);
                        cmd.Parameters.AddWithValue("p_capacidad", capacidad);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Mesa registrada correctamente.");

                Limpiar();
                CargarMesas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message);
            }
        }

        // =====================================
        // CONSULTAR MESAS
        // =====================================

        private void CargarMesas()
        {
            try
            {
                using (MySqlConnection cn =
                    new MySqlConnection(conexion))
                {
                    cn.Open();

                    using (MySqlCommand cmd =
                        new MySqlCommand("SeleccionarMesas", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        DataTable tabla = new DataTable();

                        using (MySqlDataAdapter adapter =
                            new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(tabla);
                        }

                        dgvMesas.DataSource = tabla;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al consultar: " + ex.Message);
            }
        }

        // =====================================
        // SELECCIONAR MESA
        // =====================================

        private void DgvMesas_CellClick(
            object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila =
                    dgvMesas.Rows[e.RowIndex];

                idMesaSeleccionada =
                    Convert.ToInt32(fila.Cells["id_mesa"].Value);

                txtNumero.Text =
                    fila.Cells["numero_mesa"].Value.ToString();

                txtCapacidad.Text =
                    fila.Cells["capacidad"].Value.ToString();
            }
        }

        // =====================================
        // ELIMINAR MESA
        // =====================================

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (idMesaSeleccionada == 0)
            {
                MessageBox.Show("Seleccione una mesa de la tabla.");
                return;
            }

            DialogResult respuesta = MessageBox.Show(
                "¿Está seguro de eliminar esta mesa?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (respuesta == DialogResult.Yes)
            {
                try
                {
                    using (MySqlConnection cn =
                        new MySqlConnection(conexion))
                    {
                        cn.Open();

                        using (MySqlCommand cmd =
                            new MySqlCommand("EliminarMesa", cn))
                        {
                            cmd.CommandType =
                                CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue(
                                "p_id", idMesaSeleccionada);

                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Mesa eliminada correctamente.");

                    Limpiar();
                    CargarMesas();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "No se pudo eliminar la mesa. Puede tener reservaciones asociadas.\n"
                        + ex.Message);
                }
            }
        }

        // =====================================
        // ACTUALIZAR TABLA
        // =====================================

        private void BtnActualizar_Click(object sender, EventArgs e)
        {
            CargarMesas();
            MessageBox.Show("Lista actualizada.");
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
            txtNumero.Clear();
            txtCapacidad.Clear();
            idMesaSeleccionada = 0;
            dgvMesas.ClearSelection();
        }
    }
}
