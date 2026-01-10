using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NT106_BattleshipClient
{
    public partial class frmResult : BaseForm
    {
        private Label lbTitle;
        private Label lbPoint;
        private Button btnPlayAgain;
        private Button btnReturn;

        public frmResult(string resultText = "YOU WON/LOSE!", int Point = 0)
        {
            InitializeComponent();
            // chống nháy form
            EnableFormDoubleBuffering();
            // Form settings
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.TopMost = true;
            this.ShowInTaskbar = false;
            this.Size = new Size(400, 200);


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


            lbPoint = new Label()
            {
                Text = "Point " + Point,
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 30
            };
            this.Controls.Add(lbPoint);


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
            if (FormManager.frmLobby == null || FormManager.frmLobby.IsDisposed)
                FormManager.frmLobby = new frmLobby();

            FormManager.frmLobby.Show();
            FormManager.frmLobby.BringToFront();
            this.Close();
        }

        private void BtnReturn_Click(object sender, EventArgs e)
        {
            if (FormManager.frmMainMenu == null || FormManager.frmMainMenu.IsDisposed)
                FormManager.frmMainMenu = new frmMainMenu();

            FormManager.frmMainMenu.Show();
            FormManager.frmMainMenu.BringToFront();
            this.Close();
        }

        private void frmResult_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.Sizable; 
            this.ControlBox = true;
            this.MinimizeBox = true;
            this.MaximizeBox = true;

            this.WindowState = FormWindowState.Normal;
        }
    }
}
