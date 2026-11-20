
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SistemaReservas
{
    public partial class Form1 : Form
    {
        private Panel panelMenu;
        private Panel panelContenido;
        private Label lblTitulo;
        private Label lblSubtitulo;

        private Button btnMesas;
        private Button btnClientes;
        private Button btnReservaciones;
        private Button btnSalir;

        public Form1()
        {
            InitializeComponent();
            CrearInterfaz();
        }

        private void CrearInterfaz()
        {
            this.Text = "Sistema de Reservas";
            this.Size = new Size(1000, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.MinimumSize = new Size(850, 550);

            // MENU LATERAL
            panelMenu = new Panel();
            panelMenu.Dock = DockStyle.Left;
            panelMenu.Width = 230;
            panelMenu.BackColor = Color.FromArgb(30, 45, 65);
            this.Controls.Add(panelMenu);

            Label lblLogo = new Label();
            lblLogo.Text = "RESTAURANTE";
            lblLogo.ForeColor = Color.White;
            lblLogo.Font = new Font("Arial", 18, FontStyle.Bold);
            lblLogo.Location = new Point(20, 35);
            lblLogo.AutoSize = true;
            panelMenu.Controls.Add(lblLogo);

            Label lblMenu = new Label();
            lblMenu.Text = "MENÚ PRINCIPAL";
            lblMenu.ForeColor = Color.LightGray;
            lblMenu.Font = new Font("Arial", 10, FontStyle.Bold);
            lblMenu.Location = new Point(25, 110);
            lblMenu.AutoSize = true;
            panelMenu.Controls.Add(lblMenu);

            // BOTON MESAS
            btnMesas = CrearBoton("MESAS", 160);
            btnMesas.Click += BtnMesas_Click;
            panelMenu.Controls.Add(btnMesas);

            // BOTON CLIENTES
            btnClientes = CrearBoton("CLIENTES", 220);
            btnClientes.Click += BtnClientes_Click;
            panelMenu.Controls.Add(btnClientes);

            // BOTON RESERVACIONES
            btnReservaciones = CrearBoton("RESERVACIONES", 280);
            btnReservaciones.Click += BtnReservaciones_Click;
            panelMenu.Controls.Add(btnReservaciones);

            // BOTON SALIR
            btnSalir = CrearBoton("SALIR", 500);
            btnSalir.Click += BtnSalir_Click;
            panelMenu.Controls.Add(btnSalir);

            // PANEL CONTENIDO
            panelContenido = new Panel();
            panelContenido.Dock = DockStyle.Fill;
            panelContenido.BackColor = Color.FromArgb(245, 247, 250);
            this.Controls.Add(panelContenido);

            // TITULO
            lblTitulo = new Label();
            lblTitulo.Text = "Bienvenido";
            lblTitulo.Font = new Font("Arial", 25, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(30, 45, 65);
            lblTitulo.Location = new Point(50, 60);
            lblTitulo.AutoSize = true;
            panelContenido.Controls.Add(lblTitulo);

            // SUBTITULO
            lblSubtitulo = new Label();
            lblSubtitulo.Text =
                "Sistema de administración de reservas del restaurante";
            lblSubtitulo.Font = new Font("Arial", 12);
            lblSubtitulo.ForeColor = Color.Gray;
            lblSubtitulo.Location = new Point(53, 115);
            lblSubtitulo.AutoSize = true;
            panelContenido.Controls.Add(lblSubtitulo);

            Label lblInfo = new Label();
            lblInfo.Text =
                "Seleccione una opción del menú lateral para comenzar.";
            lblInfo.Font = new Font("Arial", 14);
            lblInfo.ForeColor = Color.FromArgb(70, 80, 90);
            lblInfo.Location = new Point(53, 200);
            lblInfo.AutoSize = true;
            panelContenido.Controls.Add(lblInfo);
        }

        private Button CrearBoton(string texto, int posicionY)
        {
            Button boton = new Button();

            boton.Text = texto;
            boton.Size = new Size(190, 45);
            boton.Location = new Point(20, posicionY);
            boton.FlatStyle = FlatStyle.Flat;
            boton.FlatAppearance.BorderSize = 0;
            boton.BackColor = Color.FromArgb(45, 65, 90);
            boton.ForeColor = Color.White;
            boton.Font = new Font("Arial", 10, FontStyle.Bold);
            boton.Cursor = Cursors.Hand;

            return boton;
        }

        private void BtnMesas_Click(object sender, EventArgs e)
        {
            AbrirFormulario("MESAS");
        }

        private void BtnClientes_Click(object sender, EventArgs e)
        {
            AbrirFormulario("CLIENTES");
        }

        private void BtnReservaciones_Click(object sender, EventArgs e)
        {
            AbrirFormulario("RESERVACIONES");
        }

        private void AbrirFormulario(string nombre)
        {
            MessageBox.Show(
                "El formulario de " + nombre +
                " se desarrollará en el siguiente paso.",
                "Sistema de Reservas",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            DialogResult respuesta = MessageBox.Show(
                "¿Desea salir del sistema?",
                "Confirmar salida",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (respuesta == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
