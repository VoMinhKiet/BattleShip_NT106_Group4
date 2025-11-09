using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NT106_BattleshipClient
{
    public partial class frmShip_Sorting : Form
    {
        private Timer timer;
        private TimeSpan leftTime;
        private TimeSpan rightTime;
        private Label lblLeftTimer;
        private Label lblRightTimer;
        private bool isLeftTimerRunning = true;
        private bool isRightTimerRunning = true;
        private Button[,] playerGrid = new Button[10, 10];
        private bool[,] playerShips = new bool[10, 10]; // true = ship is here\
        public frmShip_Sorting()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None; // removes title bar
            this.WindowState = FormWindowState.Maximized; // maximize to full screen
            this.ShowInTaskbar = false;
            pnlYourGrid.Top = (this.ClientSize.Height - pnlYourGrid.Bottom / 4);
            CreateTopPanel();
            CreateGrid(pnlYourGrid, playerGrid);
            this.Load += frmShip_Sorting_Load;
        }
        public void CreateTopPanel()
        {
            // Create main top panel
            Panel topPanel = new Panel();
            topPanel.Height = 180; // adjust as needed
            topPanel.Dock = DockStyle.Top; // stick to the top
            topPanel.BackColor = Color.White;
            topPanel.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(topPanel);

            // LEFT SIDE CONTROLS
            // Timer box
            lblLeftTimer = new Label();
            lblLeftTimer.Text = "45";
            lblLeftTimer.Font = new Font("Arial", 20, FontStyle.Bold);
            lblLeftTimer.BackColor = Color.LightGray;
            lblLeftTimer.TextAlign = ContentAlignment.MiddleCenter;
            lblLeftTimer.Width = 140;
            lblLeftTimer.Height = 60;
            lblLeftTimer.Left = 255;
            lblLeftTimer.Top = (topPanel.Height - lblLeftTimer.Height) / 2;
            topPanel.Controls.Add(lblLeftTimer);

            // Circle (use Panel as circle)
            Panel leftCircle = new Panel();
            leftCircle.Width = 60;
            leftCircle.Height = 60;
            leftCircle.Left = lblLeftTimer.Right + 25;
            leftCircle.Top = (topPanel.Height - leftCircle.Height) / 2;

            // Make panel circular
            System.Drawing.Drawing2D.GraphicsPath pathLeft = new System.Drawing.Drawing2D.GraphicsPath();
            pathLeft.AddEllipse(0, 0, leftCircle.Width, leftCircle.Height);
            leftCircle.Region = new Region(pathLeft);

            //Add green overlay, using for now as no avatar yet
            leftCircle.Paint += (s, e) =>
            {
                // Enable anti-aliasing for smoother edges
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // Draw a filled green ellipse covering the entire panel
                using (Brush greenBrush = new SolidBrush(Color.LightGreen))
                {
                    e.Graphics.FillEllipse(greenBrush, 0, 0, leftCircle.Width, leftCircle.Height);
                }

                // Optional: draw a green border
                using (Pen borderPen = new Pen(Color.LightGreen, 2))
                {
                    e.Graphics.DrawEllipse(borderPen, 0, 0, leftCircle.Width - 1, leftCircle.Height - 1);
                }
            };

            //This one input players avatar
            //leftCircle.BackgroundImage = Image.FromFile("path"); // <-- put your image path here, will figure out later
            //leftCircle.BackgroundImageLayout = ImageLayout.Stretch; // Fill the circle


            topPanel.Controls.Add(leftCircle);

            // Name TextBox
            TextBox txtLeftName = new TextBox();
            txtLeftName.Width = 140;
            txtLeftName.Left = leftCircle.Right + 25;
            txtLeftName.Top = lblLeftTimer.Top + 15;
            txtLeftName.Font = new Font("Arial", 18, FontStyle.Bold);
            txtLeftName.TextAlign = HorizontalAlignment.Center;
            txtLeftName.Text = "Player 1!"; //will be changed with a variable storing player's name
            topPanel.Controls.Add(txtLeftName);

            // RIGHT SIDE CONTROLS
            // Timer box
            lblRightTimer = new Label();
            lblRightTimer.Text = "45";
            lblRightTimer.Font = new Font("Arial", 20, FontStyle.Bold);
            lblRightTimer.BackColor = Color.LightGray;
            lblRightTimer.TextAlign = ContentAlignment.MiddleCenter;
            lblRightTimer.Width = 140;
            lblRightTimer.Height = 60;
            lblRightTimer.Left = topPanel.Width - 255 - 140;
            lblRightTimer.Top = (topPanel.Height - lblRightTimer.Height) / 2;
            lblRightTimer.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            topPanel.Controls.Add(lblRightTimer);

            // Circle
            Panel rightCircle = new Panel();
            rightCircle.Width = 60;
            rightCircle.Height = 60;
            rightCircle.Left = lblRightTimer.Left - rightCircle.Width - 25;
            rightCircle.Top = (topPanel.Height - rightCircle.Height) / 2;
            rightCircle.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // Make panel circular
            System.Drawing.Drawing2D.GraphicsPath pathRight = new System.Drawing.Drawing2D.GraphicsPath();
            pathRight.AddEllipse(0, 0, rightCircle.Width, rightCircle.Height);
            rightCircle.Region = new Region(pathRight);

            //Add green overlay, using for now as no avatar yet
            rightCircle.Paint += (s, e) =>
            {
                // Enable anti-aliasing for smoother edges
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // Draw a filled green ellipse covering the entire panel
                using (Brush redBrush = new SolidBrush(Color.Red))
                {
                    e.Graphics.FillEllipse(redBrush, 0, 0, rightCircle.Width, rightCircle.Height);
                }

                // Optional: draw a green border
                using (Pen borderPen = new Pen(Color.Red, 2))
                {
                    e.Graphics.DrawEllipse(borderPen, 0, 0, rightCircle.Width - 1, rightCircle.Height - 1);
                }
            };
            topPanel.Controls.Add(rightCircle);

            // Name TextBox
            TextBox txtRightName = new TextBox();
            txtRightName.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtRightName.Width = 140;
            txtRightName.Left = rightCircle.Left - txtRightName.Width - 25;
            txtRightName.Top = lblLeftTimer.Top + 15;
            txtRightName.Font = new Font("Arial", 18, FontStyle.Bold);
            txtRightName.TextAlign = HorizontalAlignment.Center;
            txtRightName.Text = "Player 2"; //will be changed with a variable storing player's name
            topPanel.Controls.Add(txtRightName);

            // Ready Button
            Button btnReady = new Button();
            btnReady.Text = "Ready";
            btnReady.Font = new Font("Arial", 18, FontStyle.Bold);
            btnReady.Width = 500 / 2;
            btnReady.Height = 60;
            btnReady.Left = pnlYourGrid.Left; // distance from left side of form
            btnReady.Top = pnlYourGrid.Bottom; // below your grid panel
            btnReady.Click += BtnReady_Click;
            this.Controls.Add(btnReady);

            //Auto Sort Button adding later
            Button btnAutoSort = new Button();
            btnAutoSort.Text = "Auto Sort";
            btnAutoSort.Font = new Font("Arial", 18, FontStyle.Bold);
            btnAutoSort.Width = 500 / 2;
            btnAutoSort.Height = 60;
            btnAutoSort.Left = pnlYourGrid.Left + 250; // distance from left side of form
            btnAutoSort.Top = pnlYourGrid.Bottom; // below your grid panel
            btnAutoSort.Click += BtnAutoSort_Click;
            this.Controls.Add(btnAutoSort);
        }
        //timer's here
        private void frmShip_Sorting_Load(object sender, EventArgs e)
        {
            leftTime = TimeSpan.FromSeconds(45);
            rightTime = TimeSpan.FromSeconds(45);

            timer = new Timer();
            timer.Interval = 1000; // every second
            timer.Tick += Timer_Tick;
            timer.Start(); // start automatically
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (isLeftTimerRunning)
            {
                if(leftTime.TotalSeconds <= 0)
                {
                    lblLeftTimer.Text = "Ready";
                    isLeftTimerRunning = false;
                }
                leftTime = leftTime.Subtract(TimeSpan.FromSeconds(1));
                lblLeftTimer.Text = leftTime.ToString(@"ss");
            }
            if (isRightTimerRunning)
            {
                if(rightTime.TotalSeconds <= 0)
                {
                    lblRightTimer.Text = "Ready";
                    isRightTimerRunning = false;
                }
                rightTime = rightTime.Subtract(TimeSpan.FromSeconds(1));
                lblRightTimer.Text = rightTime.ToString(@"ss");
            }
        }
        private void BtnReady_Click(object sender, EventArgs e)
        {
            isLeftTimerRunning = false;
            Button btn = sender as Button;
            lblLeftTimer.Text = "Ready";
            //adding ships check
        }
        public void CreateGrid(Panel container, Button[,] grid)
        {
            Label lblYourShip = new Label();
            lblYourShip.Text = "Your Ships";
            lblYourShip.Font = new Font("Arial", 18, FontStyle.Bold);
            lblYourShip.AutoSize = true;
            lblYourShip.Left = pnlYourGrid.Left +(pnlYourGrid.Width - lblYourShip.Width) / 2;
            lblYourShip.Top = pnlYourGrid.Top - lblYourShip.Height - 5;
            this.Controls.Add(lblYourShip);

            int size = 50; // button size
            container.Controls.Clear();

            for (int row = 0; row < 10; row++)
            {
                for (int col = 0; col < 10; col++)
                {
                    Button btn = new Button();
                    btn.Width = size;
                    btn.Height = size;
                    btn.Left = col * size;
                    btn.Top = row * size;
                    btn.Tag = new Point(row, col); // store coordinates
                    btn.BackColor = Color.LightBlue;
                    btn.Click += GridButton_Click; // assign click handler
                    container.Controls.Add(btn);
                    grid[row, col] = btn;
                }
            }
        }
        private void GridButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            Point pos = (Point)btn.Tag;

            int row = pos.X;
            int col = pos.Y;
            // Check if already placed
            if (isLeftTimerRunning != true)
            {
                return;
            }
            else if (playerShips[row, col]) //need a way to index ship location. database require
            {
                MessageBox.Show("Ship already placed here!");// need to fix again as click anywhere = close
                return;
            }
            else
            {
                // Place the ship
                playerShips[row, col] = true;
                btn.BackColor = Color.Lime; // indicate ship visually
            }
        }
        private void BtnAutoSort_Click(object sender, EventArgs e)
        {
            //autosort function here
        }
    }
}
