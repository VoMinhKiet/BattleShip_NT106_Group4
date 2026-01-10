using Microsoft.AspNetCore.SignalR.Client;
using NT106_BattleshipClient.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
//using System.Threading;
using System.Windows.Forms;

namespace NT106_BattleshipClient
{
    public partial class frmShip_Sorting : BaseForm
    {
        private Timer timer;
        private TimeSpan leftTime;
        private TimeSpan rightTime;
        private Label lblLeftTimer;
        private Label lblRightTimer;
        private bool isLeftTimerRunning = true;
        private bool isRightTimerRunning = true;
        private Button[,] playerGrid = new Button[10, 10];
        private int[,] ShipPos = new int[10, 10]; //1 = có tàu ở ô [x, y]
        private int[,] otherShipPos = new int[10, 10];
        private bool dragging = false;
        private Point dragCursor;
        private Point dragStart;
        private Panel dragPanel;
        private const int SnapDistance = 40;
        private enum ShipOrientation { Vertical, Horizontal };
        private Dictionary<Panel, ShipOrientation> shipOrientation = new Dictionary<Panel, ShipOrientation>();
        int[] rowIndices = new int[6];
        int[] colIndices = new int[6];
        int[] orientations = new int[6]; // 1 = Vertical, 0 = Horizontal
        int[] otherRow = new int[6];
        int[] otherCol = new int[6];
        int[] otherOrientations = new int[6];
        public int mapsize;
        private readonly TranDauDto _currentMatch;
        private readonly RoomDto _room;
        private HubConnection _hub;
        public Panel Ship5;
        public Panel Ship4;
        public Panel Ship3;
        public Panel Ship2;
        public bool AutoSorted = false;
        public frmShip_Sorting(RoomDto room, TranDauDto currentMatch, int size, HubConnection hub)
        {

            this.FormBorderStyle = FormBorderStyle.None; // removes title bar
            this.WindowState = FormWindowState.Maximized; // maximize to full screen
            this.ShowInTaskbar = true;
            this.BackgroundImage = Properties.Resources.In_Battle_Background;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.SetStyle(ControlStyles.DoubleBuffer |
              ControlStyles.UserPaint |
              ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
            InitializeComponent();

            mapsize = size;
            _room = room;
            _hub = hub;
            _currentMatch = currentMatch ?? throw new ArgumentNullException(nameof(currentMatch));
            this.KeyPreview = true;               //check nhấn bàn phím
            this.KeyDown += FrmShip_Sorting_KeyDown;

            // chống nháy form
            EnableFormDoubleBuffering();

            this.FormBorderStyle = FormBorderStyle.None;

            pnlYourGrid.Top = (this.ClientSize.Height - pnlYourGrid.Bottom / 4);
            CreateTopPanel();
            CreateGrid(pnlYourGrid, playerGrid);
            Ship5 = CreateShip(1, 5, 0);
            Ship4 = CreateShip(1, 4, 1);
            Ship3 = CreateShip(1, 3, 2);
            Ship2 = CreateShip(1, 2, 3);
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
            txtLeftName.Text = GlobalData.Username; //will be changed with a variable storing player's name
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
            if (GlobalData.Username == _room.TenKhach)
            {
                txtRightName.Text = _room.TenChuPhong; //will be changed with a variable storing player's name
            }
            else txtRightName.Text = _room.TenKhach;
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
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            leftTime = TimeSpan.FromSeconds(45);
            rightTime = TimeSpan.FromSeconds(45);

            //this.FormBorderStyle = FormBorderStyle.Sizable; // ← QUAN TRỌNG
            //this.ControlBox = true;
            //this.MinimizeBox = true;
            //this.MaximizeBox = true;

            //this.WindowState = FormWindowState.Normal;

            timer = new Timer();
            timer.Interval = 1000; // every second
            timer.Tick += Timer_Tick;
            timer.Start(); // start automatically
            if (_hub != null)
            {
                ReceivedShipPositions();
                ReadyUpFlag();
            }
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (isLeftTimerRunning)
            {
                if (leftTime.TotalSeconds <= 0)
                {
                    lblLeftTimer.Text = "Ready";
                    isLeftTimerRunning = false;
                }
                leftTime = leftTime.Subtract(TimeSpan.FromSeconds(1));
                lblLeftTimer.Text = leftTime.ToString(@"ss");
            }
            if (isRightTimerRunning)
            {
                if (rightTime.TotalSeconds <= 0)
                {
                    lblRightTimer.Text = "Ready";
                    isRightTimerRunning = false;
                }
                rightTime = rightTime.Subtract(TimeSpan.FromSeconds(1));
                lblRightTimer.Text = rightTime.ToString(@"ss");
            }
        }
        private async void BtnReady_Click(object sender, EventArgs e)
        {
            isLeftTimerRunning = false;

            Button btn = sender as Button;
            //tinh toan ship pos cua minh
            for (int size = 2; size <= 5; size++)
            {
                if (orientations[size] == 1)
                   
                {
                    MessageBox.Show($"Ship size {size} at Row {colIndices[size]} , Col {rowIndices[size]} , Vertical");
                    for (int i = rowIndices[size]; i < rowIndices[size] + size; i++)
                    {
                        ShipPos[i, colIndices[size]] = 1;
                    }
                }
                else
                {
                    MessageBox.Show($"Ship size {size} at Row {colIndices[size]} , Col {rowIndices[size]} , Horizontal");
                    for (int i = colIndices[size]; i < colIndices[size] + size; i++)
                    {
                        ShipPos[rowIndices[size], i] = 1;
                    }
                }
            }
            //MessageBox.Show($"Character {_currentMatch.TenNV1} , {_currentMatch.TenNV2}" );
            lblLeftTimer.Text = "Ready!";

            // LOGIC MỚI CHO PVE
            if (_hub == null) // Chơi với máy
            {
                // Tự động tạo tàu cho Bot
                GenerateBotShips();

                frmIn_Battle frmBattle = new frmIn_Battle(ShipPos, otherShipPos, _room, _currentMatch, mapsize);
                frmBattle.Show();
                this.Hide();

                frmBattle.FormClosed += (s, args) => this.Close();
                return;
            }

            await _hub.SendAsync("SendShipPos", _room.Id, rowIndices, colIndices, orientations);
            await _hub.InvokeAsync("UpdateReadyFlag", _room.Id, true);
            if (isRightTimerRunning == false)
            {
                frmIn_Battle frmIn_Battle = new frmIn_Battle(ShipPos, otherShipPos, _room, _currentMatch, mapsize);

                frmIn_Battle.Show();
                this.Hide();
            }
        }

        public void CreateGrid(Panel container, Button[,] grid)
        {
            //Grid để xếp tàu
            Label lblYourShip = new Label();
            lblYourShip.Text = "Your Ships";
            lblYourShip.BackColor = Color.Transparent;
            lblYourShip.Font = new Font("Arial", 18, FontStyle.Bold);
            lblYourShip.AutoSize = true;
            lblYourShip.Left = 92 + (500 - lblYourShip.Width) / 2;
            lblYourShip.Top = 297 - 500 - lblYourShip.Height - 5;
            this.Controls.Add(lblYourShip);
            int size = 500 / mapsize;
            container.Controls.Clear();

            for (int row = 0; row < mapsize; row++)
            {
                for (int col = 0; col < mapsize; col++)
                {
                    Button btn = new Button();
                    btn.Width = size;
                    btn.Height = size;
                    btn.Left = col * size;
                    btn.Top = row * size;
                    btn.Tag = new Point(row, col); // store coordinates
                    btn.BackColor = Color.LightBlue;
                    //btn.Click += GridButton_Click; // assign click handler
                    container.Controls.Add(btn);
                    grid[row, col] = btn;
                }
            }
        }
        private void BtnAutoSort_Click(object sender, EventArgs e)
        {
            // 1. Nếu đã từng xếp tự động, hoặc tàu đang nằm trên bàn cờ, cần reset bàn cờ trước
            // Reset màu grid về mặc định
            for (int Y = 0; Y < mapsize; Y++)
            {
                for (int X = 0; X < mapsize; X++)
                {
                    playerGrid[Y, X].BackColor = Color.LightBlue;
                    ShipPos[Y, X] = 0; // Reset cả mảng dữ liệu logic (quan trọng!)
                }
            }

            // 2. Xử lý các panel tàu kéo thả (Ship5, Ship4...)
            // Kiểm tra null để tránh lỗi crash khi bấm lần 2
            if (Ship5 != null && !Ship5.IsDisposed) { this.Controls.Remove(Ship5); Ship5.Dispose(); Ship5 = null; }
            if (Ship4 != null && !Ship4.IsDisposed) { this.Controls.Remove(Ship4); Ship4.Dispose(); Ship4 = null; }
            if (Ship3 != null && !Ship3.IsDisposed) { this.Controls.Remove(Ship3); Ship3.Dispose(); Ship3 = null; }
            if (Ship2 != null && !Ship2.IsDisposed) { this.Controls.Remove(Ship2); Ship2.Dispose(); Ship2 = null; }

            Random _randomPos = new Random();
            Random _randomOri = new Random();
            bool success = false;

            // 3. Dùng vòng lặp while để thử lại thay vì gọi đệ quy (tránh crash)
            int attempts = 0; // Biến đếm số lần thử để tránh treo máy
            while (!success && attempts < 1000)
            {
                attempts++;
                int placedShips = 0;

                // Reset lại mảng vị trí tạm thời cho lần thử này
                // (Lưu ý: Logic gốc của Đạt đang vẽ thẳng lên Grid, nên đoạn này ta phải clear màu nếu thử lại)
                if (attempts > 1)
                {
                    for (int Y = 0; Y < mapsize; Y++)
                        for (int X = 0; X < mapsize; X++)
                            playerGrid[Y, X].BackColor = Color.LightBlue;
                }

                // Logic xếp tàu (Giữ nguyên logic random của Đạt nhưng bỏ các đoạn đệ quy)
                // Mình tối ưu lại vòng lặp tàu để code gọn hơn
                int[] shipSizes = { 5, 4, 3, 2 }; // Kích thước tàu

                for (int i = 0; i < shipSizes.Length; i++)
                {
                    int currentSize = shipSizes[i];
                    bool ShipCurrentPlaced = false;

                    // Cố gắng đặt tàu hiện tại (thử tối đa 100 lần cho mỗi tàu để không bị loop vô tận)
                    int tryPlace = 0;
                    while (!ShipCurrentPlaced && tryPlace < 100)
                    {
                        tryPlace++;
                        int Y = _randomPos.Next(0, mapsize);
                        int X = _randomPos.Next(0, mapsize);
                        int ori = _randomOri.Next(0, 2); // 0: Ngang, 1: Dọc

                        if (CanPlaceShip(Y, X, currentSize, ori))
                        {
                            PlaceShip(Y, X, currentSize, ori, i); // i dùng để chọn màu
                            ShipCurrentPlaced = true;
                            placedShips++;
                        }
                    }
                }

                if (placedShips == 4)
                {

                    success = true;
                }
            }

            if (!success)
            {
                MessageBox.Show("Không thể xếp tàu tự động sau nhiều lần thử. Vui lòng thử lại!");
            }
            else
            {
                AutoSorted = true;
            }
        }

        // Hàm phụ trợ 1: Kiểm tra xem có đặt được tàu không
        private bool CanPlaceShip(int row, int col, int size, int ori)
        {
            if (ori == 1) // Dọc
            {
                if (row + size > mapsize) return false;
                for (int i = 0; i < size; i++)
                    if (playerGrid[row + i, col].BackColor != Color.LightBlue) return false;
            }
            else // Ngang
            {
                if (col + size > mapsize) return false;
                for (int i = 0; i < size; i++)
                    if (playerGrid[row, col + i].BackColor != Color.LightBlue) return false;
            }
            return true;
        }

        // Hàm phụ trợ 2: Đặt tàu và tô màu
        private void PlaceShip(int row, int col, int size, int ori, int shipIndex)
        {
            // Lưu thông tin để gửi server
            rowIndices[size] = row;
            colIndices[size] = col;
            orientations[size] = ori;

            Color shipColor = Color.Purple;
            if (shipIndex == 1) shipColor = Color.Blue;
            if (shipIndex == 2) shipColor = Color.Yellow;
            if (shipIndex == 3) shipColor = Color.Orange;

            if (ori == 1) // Dọc
            {
                for (int i = 0; i < size; i++)
                    playerGrid[row + i, col].BackColor = shipColor;
            }
            else // Ngang
            {
                for (int i = 0; i < size; i++)
                    playerGrid[row, col + i].BackColor = shipColor;
            }
        }

        // HÀM TẠO TÀU BOT
        private void GenerateBotShips()
        {
            Array.Clear(otherShipPos, 0, otherShipPos.Length);
            Random rand = new Random();
            int[] shipSizes = { 5, 4, 3, 2 };

            foreach (int size in shipSizes)
            {
                bool placed = false;
                while (!placed)
                {
                    int r = rand.Next(0, mapsize);
                    int c = rand.Next(0, mapsize);
                    int ori = rand.Next(0, 2);

                    if (CanPlaceBotShip(r, c, size, ori))
                    {
                        if (ori == 1) for (int i = 0; i < size; i++) otherShipPos[r + i, c] = 1;
                        else for (int i = 0; i < size; i++) otherShipPos[r, c + i] = 1;
                        placed = true;
                    }
                }
            }
        }

        private bool CanPlaceBotShip(int r, int c, int size, int ori)
        {
            if (ori == 1)
            {
                if (r + size > mapsize) return false;
                for (int i = 0; i < size; i++) if (otherShipPos[r + i, c] == 1) return false;
            }
            else
            {
                if (c + size > mapsize) return false;
                for (int i = 0; i < size; i++) if (otherShipPos[r, c + i] == 1) return false;
            }
            return true;
        }

        private Panel CreateShip(int col, int row, int offset)
        {
            int size = 500 / mapsize;
            Panel ship = new Panel();
            ship.Size = new Size(col * size, row * size);
            ship.Left = this.ClientSize.Width - 92 - -500 + size * offset;
            ship.Top = 297;
            ship.BackColor = Color.White;
            ship.BorderStyle = BorderStyle.FixedSingle;
            //Flag xoay tàu, biến toàn cục
            shipOrientation[ship] = ShipOrientation.Vertical;

            for (int i = 0; i < row; i++)
            {
                Button btn = new Button();
                btn.Width = size;
                btn.Height = size;
                btn.Left = 0;
                btn.Top = i * size;
                if (offset == 0)
                {
                    btn.BackColor = Color.Purple;
                }
                if (offset == 1)
                {
                    btn.BackColor = Color.Blue;
                }
                if (offset == 2)
                {
                    btn.BackColor = Color.Yellow;
                }
                if (offset == 3)
                {
                    btn.BackColor = Color.Orange;
                }
                //logic kéo thả
                btn.MouseDown += Button_MouseDown;
                btn.MouseMove += Button_MouseMove;
                btn.MouseUp += Button_MouseUp;

                ship.Controls.Add(btn);
            }
            this.Controls.Add(ship);
            return ship;
        }
        //Handler nhấn phím R
        private void FrmShip_Sorting_KeyDown(object sender, KeyEventArgs e)
        {

            if (dragging && dragPanel != null && e.KeyCode == Keys.R)
            {
                RotateShip(dragPanel);

                dragCursor = Cursor.Position;
                dragStart = dragPanel.Location;

                e.Handled = true;
            }
        }
        //Handler logic kéo thả
        private void Button_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Button btn = (Button)sender;
                dragPanel = (Panel)btn.Parent;

                dragging = true;
                dragCursor = Cursor.Position;
                dragStart = dragPanel.Location;
                dragPanel.BringToFront();
            }
        }
        //Handler logic kéo thả
        private void Button_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging && dragPanel != null)
            {
                Point diff = new Point(
                    Cursor.Position.X - dragCursor.X,
                    Cursor.Position.Y - dragCursor.Y
                );

                dragPanel.Location = new Point(
                    dragStart.X + diff.X,
                    dragStart.Y + diff.Y
                );
            }
        }
        //Handler logic kéo thả và snapping
        private void Button_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;

            if (dragPanel != null)
                SnapShipToGrid(dragPanel);

            dragPanel = null;
        }
        //handler snapping
        private Point GetShipAnchorPoint(Panel ship)
        {
            if (ship.Controls.Count == 0)
                return ship.Location;

            Button first = ship.Controls[0] as Button;

            // Lấy tọa độ button trong grid
            return ship.Parent.PointToClient(ship.PointToScreen(first.Location));
        }
        private void SnapShipToGrid(Panel ship)
        {
            Button nearest = null;
            double nearestDist = double.MaxValue;
            Point anchor = GetShipAnchorPoint(ship);

            foreach (Button btn in playerGrid)
            {
                if (btn == null)
                    continue;

                // Chuyển đổi từ btn thành tọa độ
                Point screenBtn = btn.Parent.PointToScreen(new Point(btn.Left + btn.Width / 2,
                                                                     btn.Top + btn.Height / 2));
                Point formBtn = this.PointToClient(screenBtn);

                double dx = anchor.X - formBtn.X;
                double dy = anchor.Y - formBtn.Y;

                double dist = dx * dx + dy * dy;

                if (dist < nearestDist)
                {
                    nearestDist = dist;
                    nearest = btn;
                }
            }

            if (nearest == null)
                return;

            double snapDistance = Math.Sqrt(nearestDist);

            if (snapDistance > SnapDistance)
                return;
            Button first = ship.Controls.Cast<Button>().OrderBy(b => b.Top).ThenBy(b => b.Left).First(); // chiu luon

            // convert nearest grid button to FORM coordinates, chịu
            Point screenNearest = nearest.Parent.PointToScreen(nearest.Location);
            Point formNearest = this.PointToClient(screenNearest);

            ship.Left = formNearest.X - first.Left;
            ship.Top = formNearest.Y - first.Top;

            int size = ship.Controls.Count;
            if (size >= 2 && size <= 5)
            {
                Point snappedMatrixPos = (Point)nearest.Tag;

                rowIndices[size] = snappedMatrixPos.X;
                colIndices[size] = snappedMatrixPos.Y;
                orientations[size] = (shipOrientation[ship] == ShipOrientation.Vertical) ? 1 : 0;
            }
        }
        //Xoay tàu
        private void RotateShip(Panel ship)
        {
            int size = 500 / mapsize;

            ShipOrientation current = shipOrientation[ship];


            if (current == ShipOrientation.Vertical)
                shipOrientation[ship] = ShipOrientation.Horizontal;
            else
                shipOrientation[ship] = ShipOrientation.Vertical;

            ShipOrientation newOri = shipOrientation[ship];

            int count = ship.Controls.Count;


            if (newOri == ShipOrientation.Horizontal)
                ship.Size = new Size(count * size, size);
            else
                ship.Size = new Size(size, count * size);


            for (int i = 0; i < count; i++)
            {
                Button btn = ship.Controls[i] as Button;

                if (newOri == ShipOrientation.Horizontal)
                {
                    btn.Left = i * size;
                    btn.Top = 0;
                }
                else
                {
                    btn.Left = 0;
                    btn.Top = i * size;
                }
            }

        }
        private void ReceivedShipPositions()
        {
            _hub.On<int[], int[], int[]>("ReceivedShips", (rows, cols, oris) =>
            {
                this.BeginInvoke(new Action(() =>
                {
                    // tinh toan ship pos cua doi thu nhan ve
                    for (int size = 2; size <= 5; size++)
                    {
                        if (oris[size] == 1)
                        {
                            for (int i = rows[size]; i <= rows[size] + size - 1; i++)
                            {
                                otherShipPos[i, cols[size]] = 1;
                            }
                        }
                        else
                        {
                            for (int i = cols[size]; i <= cols[size] + size - 1; i++)
                            {
                                otherShipPos[rows[size], i] = 1;
                            }
                        }
                    }
                }));
            });
        }
        private void ReadyUpFlag()
        {
            _hub.On<bool>("ReceiveReadyFlag", (flag) =>
            {
                this.BeginInvoke(new Action(() =>
                {
                    isRightTimerRunning = false;
                    lblRightTimer.Text = "Ready!";
                    if (isLeftTimerRunning == false)
                    {

                        frmIn_Battle frmIn_Battle = new frmIn_Battle(ShipPos, otherShipPos, _room, _currentMatch, mapsize);


                        frmIn_Battle.FormClosed += (s, args) =>
                        {
                            this.Close();
                        };

                        frmIn_Battle.Show();
                        this.Hide();
                    }
                }));
            });
        }

        private void frmShip_Sorting_Load_1(object sender, EventArgs e)
        {

        }
    }
}
