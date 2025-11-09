using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NT106_BattleshipClient
{
    public partial class frmResult : Form
    {
        private Label lbTitle;
        private Label lbPoint;
        private Button btnPlayAgain;
        private Button btnReturn;

        public frmResult(string resultText = "YOU WON/LOSE!", string pointText = "Point + ", int Point = 0)
        {
            InitializeComponent();

            // Form settings
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.TopMost = true;
            this.ShowInTaskbar = false;
            this.Size = new Size(400, 200);

            // Title
            lbTitle = new Label()
            {
                Text = resultText,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 50
            };
            this.Controls.Add(lbTitle);

            // Point
            lbPoint = new Label()
            {
                Text = pointText + Point,
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 30
            };
            this.Controls.Add(lbPoint);

            // Buttons panel
            Panel buttonPanel = new Panel()
            {
                Dock = DockStyle.Bottom,
                Height = 60
            };

            // Play Again button
            btnPlayAgain = new Button()
            {
                Text = "Play again",
                Width = 120,
                Height = 35,
                Location = new Point(60, 10)
            }; btnPlayAgain.Click += BtnPlayAgain_Click;

            // Return button
            btnReturn = new Button()
            {
                Text = "Return",
                Width = 120,
                Height = 35,
                Location = new Point(220, 10)
            }; btnReturn.Click += BtnReturn_Click;

            buttonPanel.Controls.Add(btnPlayAgain);
            buttonPanel.Controls.Add(btnReturn);

            this.Controls.Add(buttonPanel);
        }
        private void BtnPlayAgain_Click(object sender, EventArgs e)
        {
            frmLobby LobbyControl = new frmLobby();
            LobbyControl.Show();
            this.Hide(); //Should change to Close after testing


        }

        private void BtnReturn_Click(object sender, EventArgs e)
        {
            frmMainMenu MainMenuControl = new frmMainMenu();
            MainMenuControl.Show();
            this.Hide();
        }
    }
}
