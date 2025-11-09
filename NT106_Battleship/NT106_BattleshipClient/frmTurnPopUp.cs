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
    public partial class frmTurnPopUp : Form
    {
        private Timer autoCloseTimer; //Timer
        private Label messageLabel; //Changing label

        public frmTurnPopUp(string message = "Your Turn!", int durationMilliseconds = 1500) //when you doing this popup use new frmTurnPopUp("message", popuptime);
        {
            InitializeComponent();

            // Remove border and title bar
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Background color base on text
            if (message == "Your Turn!")
            {
                this.BackColor = Color.Lime;
                this.TransparencyKey = Color.Lime;
            }
            else
            {
                this.BackColor = Color.Red;
                this.TransparencyKey = Color.Red;
            }

                // Optional: keep it always on top
                this.TopMost = true;

            // Optional: don't show on taskbar
            this.ShowInTaskbar = false;

            messageLabel = new Label()
            {
                Text = message,
                AutoSize = true,
                Font = new Font("Segoe UI", 34, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(180, 0, 0, 0), // semi-transparent black bg
                Padding = new Padding(20),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Add to form and center it
            this.Controls.Add(messageLabel);
            this.ClientSize = new Size(messageLabel.Width + 40, messageLabel.Height + 40);
            messageLabel.Location = new Point(
                (this.ClientSize.Width - messageLabel.Width) / 2,
                (this.ClientSize.Height - messageLabel.Height) / 2
            );

            autoCloseTimer = new Timer();
            autoCloseTimer.Interval = durationMilliseconds; // how long to stay visible (ms)
            autoCloseTimer.Tick += (s, e) =>
            {
                autoCloseTimer.Stop();
                this.Close();
            };
            autoCloseTimer.Start();
        }
    }
}
