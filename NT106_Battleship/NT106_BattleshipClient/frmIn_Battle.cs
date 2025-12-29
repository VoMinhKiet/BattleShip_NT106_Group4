using Microsoft.AspNetCore.SignalR.Client;
using NT106_BattleshipClient.Models;
using NT106_BattleshipClient.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
//using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace NT106_BattleshipClient
{
    public partial class frmIn_Battle : BaseForm
    {
        private bool _iAmBatchShooter = false;

        private bool _WillTurnerSkillUse = false;
        private bool _isSkillBatchActive = false;
        private bool _batchHadMiss = false;
        private bool _jackMode = false;
        private int _jackShotsLeft = 0;
        private void ReceiveSkillBatch()
        {
            _battleHub.On<bool>("SkillBatch", started =>
            {
                _isSkillBatchActive = started;
            });
        }


        private List<Point> GetHectorRandom5Shots()
        {
            var shots = new List<Point>();
            var rand = new Random();
            int tries = 0;

            while (shots.Count < 5 && tries < 2000)
            {
                tries++;
                int r = rand.Next(0, mapsize);
                int c = rand.Next(0, mapsize);

                var b = opponentGrid[r, c];
                if (b.BackColor == Color.Red || b.BackColor == Color.Green) continue;

                var p = new Point(r, c);
                if (!shots.Contains(p)) shots.Add(p);
            }

            return shots;
        }
        private async Task FireMultiShotAsync(List<Point> shots)
        {
            if (shots == null || shots.Count == 0) return;

            // ===== PvE =====
            if (_isPvE)
            {
                bool anyMiss = false;

                foreach (var p in shots)
                {
                    var btn2 = opponentGrid[p.X, p.Y];
                    if (btn2.BackColor == Color.Red || btn2.BackColor == Color.Green) continue;

                    bool hit = (OpponentShipPos[p.X, p.Y] == 1);
                    btn2.BackColor = hit ? Color.Red : Color.Green;

                    if (hit) { yourScore++; ScoreTracking(); }
                    else anyMiss = true;
                }

                if (anyMiss)
                {
                    TurnSwitch();
                    Task.Delay(800).ContinueWith(_ => BotShootTurn());
                }
                return;
            }

            // ===== Online =====
            _iAmBatchShooter = true;
            _batchHadMiss = false;

            await _battleHub.InvokeAsync("SkillBatch", _room.Id, true);

            foreach (var p in shots)
            {
                var btn2 = opponentGrid[p.X, p.Y];
                if (btn2.BackColor == Color.Red || btn2.BackColor == Color.Green) continue;

                bool hit = (OpponentShipPos[p.X, p.Y] == 1);
                btn2.BackColor = hit ? Color.Red : Color.Green;

                await _battleHub.InvokeAsync("Hit", _room.Id, p.X, p.Y, hit);

                if (hit) { yourScore++; ScoreTracking(); }
                else _batchHadMiss = true;
            }

            await _battleHub.InvokeAsync("SkillBatch", _room.Id, false);

            // Batch xong: nếu có miss -> đổi lượt bằng Hub
            if (_batchHadMiss)
                await EndTurnOnlineAsync();

            _batchHadMiss = false;
            _iAmBatchShooter = false;
        }




        private List<Point> GetWill3x3Shots(int baseRow, int baseCol)
        {
            var shots = new List<Point>();
            for (int dr = -1; dr <= 1; dr++)
                for (int dc = -1; dc <= 1; dc++)
                {
                    int r = baseRow + dr;
                    int c = baseCol + dc;
                    if (r >= 0 && r < mapsize && c >= 0 && c < mapsize)
                        shots.Add(new Point(r, c));
                }
            return shots;
        }

        private List<Point> GetElizaRowShots(int row)
        {
            var shots = new List<Point>();
            for (int c = 0; c < mapsize; c++)
                shots.Add(new Point(row, c));
            return shots;
        }

        private int _resultSent = 0;

        private Timer timer;
        private TimeSpan leftTime;
        private TimeSpan rightTime;
        private Label lblLeftTimer;
        private Label lblRightTimer;
        private bool isLeftTimerRunning = false;
        private bool isRightTimerRunning = false;
        private Button[,] playerGrid;
        private Button[,] opponentGrid;
        public int[,] YourShipPos;
        public int[,] OpponentShipPos;
        public int mapsize;
        public bool isYourTurn = false;
        private int _currentUserId;
        private bool _isHost;
        public int yourScore = 0;
        public int opponentScore = 0;
        private bool _ElizabethSwannSkillOrientationRow = true;
        private bool _ElizabethSwannSkillUse = false;
        private bool _HectorBarbossaSkillUse = false;
        public int skillUsage = 3;
        public bool first = true;
        private bool _turnHandlerRegistered = false;
        private readonly TranDauDto _currentMatch;
        private readonly RoomDto _room;
        private readonly LeaderBoardDto _LeaderBoard;
        private HubConnection _battleHub;
        private HubConnection _rankingHub;
        private bool _battleEnded = false;

        private bool _isPvE = false; // Cờ nhận biết chơi với máy
        private List<Point> _botTargets = new List<Point>(); // Danh sách các ô Bot dự định bắn tiếp theo (Khi trúng tàu)

        private bool _turnPopupOpen = false;
        int random = new Random().Next(0, 2);

        private readonly TranDauApiService _tranDauApi = new TranDauApiService();

        public frmIn_Battle(int[,] ShipPos, int[,] otherShipPos, RoomDto room, TranDauDto currentMatch, int size)
        {
            this.FormBorderStyle = FormBorderStyle.None; // removes title bar
            this.WindowState = FormWindowState.Maximized; // maximize to full screen
            this.ShowInTaskbar = false;
            this.BackgroundImage = Properties.Resources.In_Battle_Background;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            //this.TopMost = true; //remember to close this after
            this.SetStyle(ControlStyles.DoubleBuffer |
              ControlStyles.UserPaint |
              ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
            InitializeComponent();

            this.KeyPreview = true;
            this.KeyDown += FrmIn_Battle_KeyDown;

            mapsize = size;
            playerGrid = new Button[mapsize, mapsize];
            opponentGrid = new Button[mapsize, mapsize];

            _currentMatch = currentMatch;
            _room = room;

            _isPvE = (_room.IDKhach == null) || (_room.IDKhach == GlobalData.BotId);

            _idPhongCho = room.Id;
            _idTranDau = currentMatch.Id;

            _rankingHub = new HubConnectionBuilder()
              .WithUrl("http://localhost:5074/battleRankingHub")
              .WithAutomaticReconnect()
              .Build();

            RegisterBattleRankingHandler();

            YourShipPos = ShipPos;
            OpponentShipPos = otherShipPos;
            _currentUserId = GlobalData.UserId;
            _isHost = (_currentUserId == room.IDChuPhong);
            // chống nháy form
            EnableFormDoubleBuffering();


            CreateTopPanel();

            // create yours panel and it settings
            Panel pnlYourGrid = new Panel();
            pnlYourGrid.Width = 500;            // width of the panel
            pnlYourGrid.Height = 500;           // height of the panel
            pnlYourGrid.Left = 92;
            pnlYourGrid.Top = 297;
            this.Controls.Add(pnlYourGrid);
            CreateGrid(pnlYourGrid, playerGrid, YourShipPos, true);

            // create label for your ship
            Label lblYourShip = new Label();
            lblYourShip.Text = "Your Ships";
            lblYourShip.BackColor = Color.Transparent;
            lblYourShip.Font = new Font("Arial", 18, FontStyle.Bold);
            lblYourShip.AutoSize = true;
            lblYourShip.Left = pnlYourGrid.Left + (pnlYourGrid.Width - lblYourShip.Width) / 2 - 20;
            lblYourShip.Top = pnlYourGrid.Top - lblYourShip.Height - 10;
            this.Controls.Add(lblYourShip);

            // create enemy's panel and it settings
            Panel pnlOpponentGrid = new Panel();
            pnlOpponentGrid.Width = 500;
            pnlOpponentGrid.Height = 500;
            pnlOpponentGrid.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            pnlOpponentGrid.Left = this.ClientSize.Width - pnlYourGrid.Left - pnlYourGrid.Width;
            pnlOpponentGrid.Top = 297;
            this.Controls.Add(pnlOpponentGrid);
            CreateGrid(pnlOpponentGrid, opponentGrid, OpponentShipPos, false);

            //create label for enemy's ship
            Label lblOpponentShip = new Label();
            lblOpponentShip.Text = "Opponent Ships";
            lblOpponentShip.BackColor = Color.Transparent;
            lblOpponentShip.Font = new Font("Arial", 18, FontStyle.Bold);
            lblOpponentShip.AutoSize = true;
            lblOpponentShip.Left = this.ClientSize.Width - pnlOpponentGrid.Left - pnlOpponentGrid.Width + (pnlOpponentGrid.Width - lblOpponentShip.Width) / 2 + 70;
            lblOpponentShip.Top = pnlOpponentGrid.Top - lblOpponentShip.Height - 10;
            lblOpponentShip.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.Controls.Add(lblOpponentShip);

            Button btnSkill = new Button();
            btnSkill.Text = "Skill Ready!";
            btnSkill.Font = new Font("Arial", 18, FontStyle.Bold);
            btnSkill.Width = 255;
            btnSkill.Height = 60;
            btnSkill.Left = pnlOpponentGrid.Left; // distance from left side of form
            btnSkill.Top = pnlOpponentGrid.Bottom; // below your grid panel
            btnSkill.Click += BtnSkill_Click;
            this.Controls.Add(btnSkill);

            //ReceiveTurn();
            if (_isPvE)
            {
                isYourTurn = true;
                isLeftTimerRunning = true;
                isRightTimerRunning = false;
            }
        }


        private void FrmIn_Battle_KeyDown(object sender, KeyEventArgs e)
        {
            // Toggle orientation when user presses 'R' (no UI indicator required)
            if (e.KeyCode == Keys.R)
            {
                _ElizabethSwannSkillOrientationRow = !_ElizabethSwannSkillOrientationRow;
            }
        }

        private void RegisterTurnHandler()
        {
            if (_turnHandlerRegistered) return;
            _turnHandlerRegistered = true;

            _battleHub.On<bool>("Turn", isHostTurn =>
            {
                isYourTurn = (_isHost && isHostTurn) || (!_isHost && !isHostTurn);

                if (isYourTurn)
                {
                    isRightTimerRunning = false;
                    isLeftTimerRunning = true;
                    using (var p = new frmTurnPopUp("Your Turn!")) p.ShowDialog(this);
                }
                else
                {
                    isLeftTimerRunning = false;
                    isRightTimerRunning = true;
                    using (var p = new frmTurnPopUp("Opponent Turn!")) p.ShowDialog(this);
                }
            });
        }

        public async void DecideTurn()
        {
            if (!_isHost) return;
            bool hostStarts = (random == 0);
            await _battleHub.InvokeAsync("Turn", _room.Id, hostStarts);
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
            lblLeftTimer.Text = "180";
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
            txtLeftName.ReadOnly = true;
            topPanel.Controls.Add(txtLeftName);

            // RIGHT SIDE CONTROLS
            // Timer box
            lblRightTimer = new Label();
            lblRightTimer.Text = "180";
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
            txtRightName.ReadOnly = true; // fix later jesus
            topPanel.Controls.Add(txtRightName);
        }
        public void CreateGrid(Panel container, Button[,] grid, int[,] ShipPos, bool Yours)
        {
            int size = 500 / mapsize; // button size
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
                    if (Yours && ShipPos[row, col] == 1)
                    {
                        btn.BackColor = Color.Black; // ship present
                    }
                    else
                    {
                        btn.BackColor = Color.LightBlue;
                    }
                    if (!Yours)
                    {
                        btn.Click += GridButton_Click; // only opponent's grid is clickable
                        btn.Cursor = Cursors.Hand;
                    }
                    container.Controls.Add(btn);
                    grid[row, col] = btn;
                }
            }
        }
        private async void GridButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;
            if (!isYourTurn) return;

            Point pos = (Point)btn.Tag;
            int row = pos.X;
            int col = pos.Y;

            if (btn.BackColor == Color.Red || btn.BackColor == Color.Green) return;

            // ===== JACK MODE =====
            if (_jackMode)
            {
                bool hit = (OpponentShipPos[row, col] == 1);
                btn.BackColor = hit ? Color.Red : Color.Green;

                if (!_isPvE)
                    await _battleHub.InvokeAsync("Hit", _room.Id, row, col, hit);

                if (hit) { yourScore++; ScoreTracking(); }
                else _batchHadMiss = true;

                _jackShotsLeft--;

                if (_jackShotsLeft <= 0)
                {
                    _jackMode = false;

                    if (_isPvE)
                    {
                        if (_batchHadMiss)
                        {
                            _batchHadMiss = false;
                            TurnSwitch();
                            Task.Delay(800).ContinueWith(_ => BotShootTurn());
                        }
                        else _batchHadMiss = false;
                    }
                    else
                    {
                        // kết thúc batch
                        await _battleHub.InvokeAsync("SkillBatch", _room.Id, false);

                        // nếu có miss -> đổi lượt bằng Hub
                        if (_batchHadMiss)
                            await EndTurnOnlineAsync();

                        _batchHadMiss = false;
                        _iAmBatchShooter = false;
                    }

                    MessageBox.Show("Jack skill finished!");
                }

                return;
            }

            // ===== ELIZA =====
            if (_ElizabethSwannSkillUse)
            {
                _ElizabethSwannSkillUse = false;
                await FireMultiShotAsync(GetElizaRowShots(row));
                return;
            }

            // ===== WILL =====
            if (_WillTurnerSkillUse)
            {
                _WillTurnerSkillUse = false;
                await FireMultiShotAsync(GetWill3x3Shots(row, col));
                return;
            }

            // ===== NORMAL SHOOT =====
            if (_isPvE)
            {
                bool hitPvE = (OpponentShipPos[row, col] == 1);
                btn.BackColor = hitPvE ? Color.Red : Color.Green;

                if (hitPvE) { yourScore++; ScoreTracking(); }
                else { TurnSwitch(); Task.Delay(1000).ContinueWith(_ => BotShootTurn()); }

                return;
            }

            // Online normal
            bool hitOnline = (OpponentShipPos[row, col] == 1);
            btn.BackColor = hitOnline ? Color.Red : Color.Green;

            await _battleHub.InvokeAsync("Hit", _room.Id, row, col, hitOnline);

            if (hitOnline)
            {
                yourScore++;
                ScoreTracking();
            }
            else
            {
                await EndTurnOnlineAsync();
            }
        }


        private async Task EndTurnOnlineAsync()
        {
            bool isHostTurnNext = !_isHost;
            await _battleHub.InvokeAsync("Turn", _room.Id, isHostTurnNext);
        }
        private void ReceiveHit()
        {
            _battleHub.On<int, int, bool>("ReceiveHit", (row, col, isHit) =>
            {
                if (YourShipPos[row, col] == 1 && isHit)
                {
                    playerGrid[row, col].BackColor = Color.Red;
                    opponentScore++;
                    ScoreTracking();
                }
                else
                {
                    playerGrid[row, col].BackColor = Color.Green;
                }
            });
        }



        private void BotShootTurn()
        {
            if (this.IsDisposed || yourScore == 14 || opponentScore == 14) return;

            this.BeginInvoke(new Action(() =>
            {
                Point target = new Point(-1, -1);
                Random rand = new Random();

                // BƯỚC 1: CHỌN MỤC TIÊU

                // Nếu hàng chờ có mục tiêu (do lần trước bắn trúng) -> Lấy ra bắn
                while (_botTargets.Count > 0)
                {
                    // Lấy điểm cuối cùng trong danh sách (cơ chế Stack giúp bot bắn dọc theo thân tàu tốt hơn)
                    int lastIndex = _botTargets.Count - 1;
                    Point p = _botTargets[lastIndex];
                    _botTargets.RemoveAt(lastIndex);

                    // Kiểm tra nếu ô này chưa bắn thì bắn
                    if (IsValidShot(p.X, p.Y))
                    {
                        target = p;
                        break;
                    }
                }

                // Nếu hàng chờ rỗng (hoặc các ô trong hàng chờ đã bị bắn hết) -> Bắn Random
                if (target.X == -1)
                {
                    int attempts = 0;
                    do
                    {
                        target = new Point(rand.Next(0, mapsize), rand.Next(0, mapsize));
                        attempts++;
                    } while ((playerGrid[target.X, target.Y].BackColor == Color.Red ||
                            playerGrid[target.X, target.Y].BackColor == Color.Green)
                            && attempts < 200);
                }

                int r = target.X;
                int c = target.Y;

                // BƯỚC 2: XỬ LÝ BẮN

                // Kiểm tra trúng/trượt
                bool botHit = (YourShipPos[r, c] == 1);

                if (botHit)
                {
                    playerGrid[r, c].BackColor = Color.Red;
                    opponentScore++;
                    ScoreTracking();

                    // THÊM 4 Ô XUNG QUANH VÀO HÀNG CHỜ=
                    AddTargetToBot(r - 1, c); // Trên
                    AddTargetToBot(r + 1, c); // Dưới
                    AddTargetToBot(r, c - 1); // Trái
                    AddTargetToBot(r, c + 1); // Phải

                    // Bot bắn trúng thì được bắn tiếp
                    if (opponentScore < 14)
                    {
                        Task.Delay(800).ContinueWith(_ => BotShootTurn());
                    }
                }
                else
                {
                    playerGrid[r, c].BackColor = Color.Green;
                    TurnSwitch(); // Trượt -> Trả lượt cho người
                }
            }));
        }

        // Kiểm tra xem ô (r, c) có hợp lệ để bắn không
        private bool IsValidShot(int r, int c)
        {
            // 1. Phải nằm trong bản đồ
            if (r < 0 || r >= mapsize || c < 0 || c >= mapsize) return false;

            // 2.Ô hợp lệ là ô chưa bị bắn (Màu hiện tại KHÔNG PHẢI Đỏ và KHÔNG PHẢI Xanh Lá)
            Color cColor = playerGrid[r, c].BackColor;
            if (cColor == Color.Red || cColor == Color.Green) return false;

            return true;
        }

        // Thêm mục tiêu vào danh sách
        private void AddTargetToBot(int r, int c)
        {
            if (IsValidShot(r, c))
            {
                // Tránh thêm trùng lặp
                if (!_botTargets.Contains(new Point(r, c)))
                {
                    _botTargets.Add(new Point(r, c));
                }
            }
        }

        public async void BtnSkill_Click(object sender, EventArgs e)
        {
            if (!isYourTurn)
            {
                MessageBox.Show("It's not your turn!");
                return;
            }

            if (skillUsage <= 0)
            {
                MessageBox.Show("No skill usage left!");
                return;
            }

            string myChar = _isHost ? _currentMatch.TenNV1 : _currentMatch.TenNV2;

            // ===== Hector: bấm nút là bắn ngay 5 ô random =====
            if (myChar == "Hector Barbossa")
            {
                skillUsage--;
                var shots = GetHectorRandom5Shots();
                await FireMultiShotAsync(shots);
                return;
            }

            // ===== Jack: bật chế độ chọn 5 phát =====
            if (myChar == "Jack Sparrow")
            {
                if (_jackMode)
                {
                    MessageBox.Show("Jack skill is active. Pick your remaining shots.");
                    return;
                }

                skillUsage--;
                _jackMode = true;
                _jackShotsLeft = 5;

                if (!_isPvE)
                {
                    _iAmBatchShooter = true;
                    _batchHadMiss = false;
                    await _battleHub.InvokeAsync("SkillBatch", _room.Id, true);
                }

                MessageBox.Show("Jack: pick 5 cells to shoot. Turn won't switch until finished.");
                return;
            }

            // ===== Eliza: click 1 ô để bắn cả hàng =====
            if (myChar == "Elizabeth Swann")
            {
                if (_ElizabethSwannSkillUse)
                {
                    MessageBox.Show("Eliza skill is already active. Click a cell to use it.");
                    return;
                }

                skillUsage--;
                _ElizabethSwannSkillUse = true;
                MessageBox.Show("Eliza: click a cell to shoot the whole row.");
                return;
            }

            if (myChar == "Will Turner")
            {
                if (_WillTurnerSkillUse)
                {
                    MessageBox.Show("Will skill is already active. Click a cell to use it.");
                    return;
                }

                skillUsage--;
                _WillTurnerSkillUse = true;
                MessageBox.Show("Will: click a cell to shoot 3x3 around it.");
                return;
            }

            MessageBox.Show("Skill for this character is not implemented.");
        }

        private async void Timer_Tick(object sender, EventArgs e)
        {
            if (this.IsDisposed || !this.IsHandleCreated)
                return;

            if (isLeftTimerRunning)
            {
                if (leftTime.TotalSeconds <= 0)
                {
                    lblLeftTimer.Text = "0";
                    isLeftTimerRunning = false;
                    isRightTimerRunning = false;

                    // ÉP THUA
                    opponentScore = 14;
                    ScoreTracking();
                    return;
                }

                leftTime = leftTime.Subtract(TimeSpan.FromSeconds(1));
                lblLeftTimer.Text = leftTime.ToString(@"mm\:ss");
            }

            if (isRightTimerRunning)
            {
                if (rightTime.TotalSeconds <= 0)
                {
                    lblRightTimer.Text = "0";
                    isRightTimerRunning = false;
                    isLeftTimerRunning = false;

                    // ÉP THẮNG
                    yourScore = 14;
                    ScoreTracking();
                    return;
                }

                rightTime = rightTime.Subtract(TimeSpan.FromSeconds(1));
                lblRightTimer.Text = rightTime.ToString(@"mm\:ss");
            }
        }

      
        private void TurnSwitch()
        {
            // flip local flag
            isYourTurn = !isYourTurn;

            if (isYourTurn)
            {
                using (var popup = new frmTurnPopUp("Your Turn!"))
                {
                    isLeftTimerRunning = true;
                    isRightTimerRunning = false;
                    popup.ShowDialog(this);
                }
            }
            else
            {
                using (var popup = new frmTurnPopUp("Opponent Turn!"))
                {
                    isRightTimerRunning = true;
                    isLeftTimerRunning = false;
                    popup.ShowDialog(this);
                }
            }

            // NOTE: original did NOT call the hub to broadcast the change
        }

        private async void ScoreTracking()
        {
            //if (opponentScore == 14)
            //{
            //    isLeftTimerRunning = false;
            //    isRightTimerRunning = false;

            //    //IndexCurrentMatch();

            //    if (_currentMatch.Id > 0) // Chỉ gọi nếu đã có ID hợp lệ
            //    {
            //        // Mình thua -> Winner là đối thủ
            //        int winnerId = (_currentUserId == _currentMatch.IdPlayer1)
            //                        ? _currentMatch.IdPlayer2 ?? 0
            //                        : _currentMatch.IdPlayer1;

            //        // Gọi API cập nhật Winner
            //        await _tranDauApi.EndMatchAsync(_currentMatch.Id, winnerId);
            //    }

            //    await SendBattleResultAsync(false);
            //    frmResult frmResult = new frmResult("You LOSE", -1);
            //    frmResult.ShowDialog();
            //    this.Close();
            //}
            //if (yourScore == 14)
            //{
            //    isLeftTimerRunning = false;
            //    isRightTimerRunning = false;

            //    //IndexCurrentMatch();

            //    if (_currentMatch.Id > 0)
            //    {
            //        // Mình thắng -> Winner là mình
            //        await _tranDauApi.EndMatchAsync(_currentMatch.Id, _currentUserId);
            //    }

            //    await SendBattleResultAsync(true);
            //    frmResult frmResult = new frmResult("You WON!", 1);
            //    frmResult.ShowDialog();
            //    this.Close();
            //}

            if (opponentScore == 14 || yourScore == 14)
            {
                isLeftTimerRunning = false; isRightTimerRunning = false;

                // Lưu kết quả DB
                if (_currentMatch.Id > 0)
                {
                    int winnerId;
                    if (yourScore == 14) winnerId = _currentUserId;
                    else winnerId = _isPvE ? GlobalData.BotId.Value : (_currentMatch.IdPlayer2 ?? 0);

                    await _tranDauApi.EndMatchAsync(_currentMatch.Id, winnerId);
                }

                // Chỉ gửi SignalR nếu Online
                if (!_isPvE) await SendBattleResultAsync(yourScore == 14);

                frmResult frm = new frmResult(yourScore == 14 ? "You WON!" : "You LOSE", yourScore == 14 ? 1 : -1);
                frm.ShowDialog();
                this.Close();
            }
        }
        private void SkillJackSparrow()
        {
            // Implement skill logic here
        }
        private void SkillHectorBarbossa()
        {
            // Implement skill logic here
        }
        private void SkillDavyJones()
        {
            // Implement skill logic here
        }


        private async void frmIn_Battle_Load(object sender, EventArgs e)
        {
            await ChatSession.ChatBox.SetBattleContextAsync(_idTranDau);
            ChatSession.ChatBox.LoadHistory();

            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            leftTime = TimeSpan.FromSeconds(180);
            rightTime = TimeSpan.FromSeconds(180);

            lblLeftTimer.Text = leftTime.ToString(@"mm\:ss");
            lblRightTimer.Text = rightTime.ToString(@"mm\:ss");

            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Start();

            // ====== ONLINE BATTLE HUB ======
            if (!_isPvE)
            {
                _battleHub = new HubConnectionBuilder()
                    .WithUrl("http://localhost:5074/tranDauHub")
                    .WithAutomaticReconnect()
                    .Build();

                // register handlers TRƯỚC StartAsync
                RegisterTurnHandler();
                ReceiveSkillBatch();
                ReceiveHit();

                await _battleHub.StartAsync();
                await _battleHub.InvokeAsync("JoinBattle", _room.Id);

                // Host phát lượt đầu (1 lần)
                if (_isHost)
                    DecideTurn();
            }
            else
            {
                // PvE: local turn
                isYourTurn = true;
                isLeftTimerRunning = true;
                isRightTimerRunning = false;
            }

            // ====== RANKING HUB ======
            if (_rankingHub.State == HubConnectionState.Disconnected)
            {
                await _rankingHub.StartAsync();
            }
        }

        private void IndexCurrentMatch()
        {
            _currentMatch.Id = _room.Id;
            _currentMatch.IdPlayer1 = _room.IDChuPhong;
            _currentMatch.IdPlayer2 = _room.IDKhach;
            _currentMatch.TenNV1 = _room.TenChuPhong;
            _currentMatch.TenNV2 = _room.TenKhach;
            _currentMatch.KichThuoc = mapsize;
            _currentMatch.Winner = null; // phai config sau
            _currentMatch.TimeStart = _room.NgayTao;
            _currentMatch.TimeEnd = DateTime.Now;

        }
        private ucChatBox _chatBox;
        private int _idPhongCho;
        private int _idTranDau;


        private async void btnTinNhan_Click(object sender, EventArgs e)
        {


            var chat = ChatSession.ChatBox;

            if (!this.Controls.Contains(chat))
                this.Controls.Add(chat);

            chat.LoadHistory();
            chat.Visible = !chat.Visible;
            chat.BringToFront();
        }

        private void RegisterBattleRankingHandler()
        {
            _rankingHub.On<dynamic>("BattleRankingUpdated", data =>
            {
                int soTranThang = (int)data.SoTranThang;
                int soTranThua = (int)data.SoTranThua;

                // cập nhật cache
                GlobalData.SoSao = (int)data.CapSao;
                GlobalData.TongSoTran = soTranThang + soTranThua;

                if (GlobalData.TongSoTran > 0)
                {
                    GlobalData.TiLeThang =
                        Math.Round(soTranThang * 100.0 / GlobalData.TongSoTran, 2);
                }
                else
                {
                    GlobalData.TiLeThang = 0;
                }


                GlobalData.NotifyUserInfoUpdated();
            });
        }

        private async Task SendBattleResultAsync(bool isWin)
        {
            if (System.Threading.Interlocked.CompareExchange(ref _resultSent, 1, 0) != 0)
                return;

            if (!_isHost) return;

            int hostId = _room.IDChuPhong;
            int guestId = _room.IDKhach.GetValueOrDefault();

            await _rankingHub.InvokeAsync("FinishBattle", new
            {
                IdNguoiDung = hostId,
                IsWin = isWin
            });

            await _rankingHub.InvokeAsync("FinishBattle", new
            {
                IdNguoiDung = guestId,
                IsWin = !isWin
            });
        }

        protected override async void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (_battleHub != null)
            {
                try
                {
                    await _battleHub.InvokeAsync("LeaveBattle", _room.Id);
                    await _battleHub.StopAsync();
                    await _battleHub.DisposeAsync();
                }
                catch { }
            }
        }

    }
}
