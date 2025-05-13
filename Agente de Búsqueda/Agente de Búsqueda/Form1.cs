using System;
using System.Drawing;
using System.Windows.Forms;

namespace Agente_de_Búsqueda
{
    public partial class Form1 : Form
    {
        private Button[,] initialButtons = new Button[3, 3];
        private Button[,] targetButtons = new Button[3, 3];
        private Label lblMovimientos;
        private int movimientos = 0;

        public Form1()
        {
            InitializeComponent();
            SetupUI();
            SetInitialState();
            SetTargetState();
        }

        private void SetupUI()
        {
            // Configuración básica del formulario
            this.Text = "Simulación 8-Puzzle con Búsqueda Ciega";
            this.ClientSize = new Size(600, 375);
            this.BackColor = Color.AliceBlue;

            // Título
            var title = new Label
            {
                Text = "Simulación 8-Puzzle con Búsqueda Ciega",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(90, 15),
                AutoSize = true
            };
            this.Controls.Add(title);

            // Crear botones para estado inicial
            CreateButtonGrid(initialButtons, 50, 55, "initial");

            // Crear botones para estado objetivo
            CreateButtonGrid(targetButtons, 350, 55, "target");

            // Etiquetas de estado
            var lblInitial = new Label
            {
                Text = "Estado Inicial",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(90, 260),
                AutoSize = true
            };
            var lblTarget = new Label
            {
                Text = "Estado Objetivo",
                Font = new Font ("Arial", 12, FontStyle.Bold),
                Location = new Point(380, 260),
                AutoSize = true
            };
            this.Controls.Add(lblInitial);
            this.Controls.Add(lblTarget);

            // Contador de movimientos
            lblMovimientos = new Label
            {
                Text = "Movimientos realizados: 0",
                Location = new Point(200, 300),
                AutoSize = true,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            this.Controls.Add(lblMovimientos);

            // Botón para reiniciar
            var btnReset = new Button
            {
                Text = "Reiniciar",
                Location = new Point(250, 330),
                Size = new Size(100, 30)
            };
            btnReset.Click += BtnReset_Click;
            this.Controls.Add(btnReset);

            // Botón para solucion
            var btnSolution = new Button
            {
                Text = "Reiniciar",
                Location = new Point(250, 330),
                Size = new Size(100, 30)
            };
            btnSolution.Click += BtnSolution_Click;
            this.Controls.Add(btnSolution);
        }

        private void CreateButtonGrid(Button[,] buttons, int startX, int startY, string prefix)
        {
            int buttonSize = 60;
            int margin = 5;

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    buttons[row, col] = new Button
                    {
                        Size = new Size(buttonSize, buttonSize),
                        Location = new Point(startX + col * (buttonSize + margin),
                                             startY + row * (buttonSize + margin)),
                        Font = new Font("Arial", 14),
                        Tag = $"{prefix}_{row}{col}",
                        BackColor = Color.LightGray
                    };

                    if (prefix == "initial")
                    {
                        buttons[row, col].Click += InitialButton_Click;
                    }

                    this.Controls.Add(buttons[row, col]);
                }
            }
        }

        private void SetInitialState()
        {
            // Estado inicial como en la imagen
            initialButtons[0, 0].Text = "2";
            initialButtons[0, 1].Text = "8";
            initialButtons[0, 2].Text = "3";
            initialButtons[1, 0].Text = "1";
            initialButtons[1, 1].Text = "6";
            initialButtons[1, 2].Text = "4";
            initialButtons[2, 0].Text = "7";
            initialButtons[2, 1].Text = "";
            initialButtons[2, 2].Text = "5";

            // Estilo para el espacio vacío
            initialButtons[2, 1].BackColor = Color.White;
        }

        private void SetTargetState()
        {
            // Estado objetivo
            targetButtons[0, 0].Text = "1";
            targetButtons[0, 1].Text = "2";
            targetButtons[0, 2].Text = "3";
            targetButtons[1, 0].Text = "8";
            targetButtons[1, 1].Text = "";
            targetButtons[1, 2].Text = "4";
            targetButtons[2, 0].Text = "7";
            targetButtons[2, 1].Text = "6";
            targetButtons[2, 2].Text = "5";

            // Estilo para el espacio vacío
            targetButtons[1, 1].BackColor = Color.White;
        }

        private void InitialButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            if (clickedButton.Text == "") return;

            // Encontrar posición del botón clickeado
            int row = -1, col = -1;
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    if (initialButtons[r, c] == clickedButton)
                    {
                        row = r;
                        col = c;
                        break;
                    }
                }
                if (row != -1) break;
            }

            // Verificar botones adyacentes
            CheckAndSwap(row, col, row - 1, col); // Arriba
            CheckAndSwap(row, col, row + 1, col); // Abajo
            CheckAndSwap(row, col, row, col - 1); // Izquierda
            CheckAndSwap(row, col, row, col + 1); // Derecha
        }

        private void CheckAndSwap(int row1, int col1, int row2, int col2)
        {
            if (row2 < 0 || row2 > 2 || col2 < 0 || col2 > 2) return;

            if (initialButtons[row2, col2].Text == "")
            {
                initialButtons[row2, col2].Text = initialButtons[row1, col1].Text;
                initialButtons[row1, col1].Text = "";
                initialButtons[row1, col1].BackColor = Color.LightGray;
                initialButtons[row2, col2].BackColor = Color.White;

                movimientos++;
                lblMovimientos.Text = $"Movimientos realizados: {movimientos}";

                if (CheckWinCondition())
                {
                    MessageBox.Show($"¡Felicidades! Resolviste el puzzle en {movimientos} movimientos.",
                                  "¡Ganaste!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private bool CheckWinCondition()
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (initialButtons[row, col].Text != targetButtons[row, col].Text)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            SetInitialState();
            movimientos = 0;
            lblMovimientos.Text = "Movimientos realizados: 0";
        }

        private void BtnSolution_Click(object sender, EventArgs e)
        {
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
