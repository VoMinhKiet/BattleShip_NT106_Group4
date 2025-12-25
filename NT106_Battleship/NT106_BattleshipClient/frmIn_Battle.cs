using Microsoft.AspNetCore.SignalR.Client;
using NT106_BattleshipClient.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
//using System.Web.UI.WebControls;
using System.Windows.Forms;
using static NT106_BattleshipClient.GlobalData;

namespace NT106_BattleshipClient
{
    public partial class frmIn_Battle : BaseForm
    {
        private int _resultSent = 0;

        private Timer timer;
        private TimeSpan leftTime;
        private TimeSpan rightTime;
        private Label lblLeftTimer;
        private Label lblRightTimer;
        private bool isLeftTimerRunning = false;
        private bool isRightTimerRunning = false;
        private Button[,] playerGrid = new Button[10, 10];
        private Button[,] opponentGrid = new Button[10, 10];
        public int[,] YourShipPos = new int[11, 11];
        public int[,] OpponentShipPos = new int[11, 11];
        public int mapsize;
        public bool isYourTurn = false;
        private int _currentUserId;
        private bool _isHost;
        public int yourScore = 0;
        public int opponentScore = 0;
        private bool _ElizabethSwannSkillOrientationRow = true;
        private bool _isUsingSkill = false;
        private List<Point> _previewCells = new List<Point>();
        private Button btnSkill;
        private bool _elizabethIsRow = true;
        private bool _suppressTurnSwitch = false;




        public bool first = true;
        private bool _turnHandlerRegistered = false;
        private readonly TranDauDto _currentMatch;
        private readonly RoomDto _room;
        private readonly LeaderBoardDto _LeaderBoard;
        private HubConnection _hub;
        private HubConnection _rankingHub;
        private bool _battleEnded = false;


        private bool _turnPopupOpen = false;
        int random = new Random().Next(0, 2);


        public frmIn_Battle(int[,] ShipPos, int[,] otherShipPos, RoomDto room, TranDauDto currentMatch, int size, HubConnection hub)
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
            this.KeyDown += FrmInBattle_KeyDown;


            this.KeyPreview = true;
            this.KeyDown += FrmIn_Battle_KeyDown;

            mapsize = size;
            _currentMatch = currentMatch;
            _room = room;
            _idPhongCho = room.Id;
            _idTranDau = currentMatch.Id;
            _hub = hub;

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

             btnSkill = new Button();
            btnSkill.Text = "Skill Ready!";
            btnSkill.Font = new Font("Arial", 18, FontStyle.Bold);
            btnSkill.Width = 255;
            btnSkill.Height = 60;
            btnSkill.Left = pnlOpponentGrid.Left; // distance from left side of form
            btnSkill.Top = pnlOpponentGrid.Bottom; // below your grid panel
            btnSkill.Click += BtnSkill_Click;
            this.Controls.Add(btnSkill);
            DecideTurn();
            //ReceiveTurn();
            ReceiveHit();
        }


        private void FrmIn_Battle_KeyDown(object sender, KeyEventArgs e)
        {
            // Toggle orientation when user presses 'R' (no UI indicator required)
            if (e.KeyCode == Keys.R)
            {
                _ElizabethSwannSkillOrientationRow = !_ElizabethSwannSkillOrientationRow;
            }
        }
        public async void DecideTurn()
        {
            if (random == 0 && _isHost)
            {
                isYourTurn = true;
                await _hub.InvokeAsync("Turn", _room.Id, true);
                using (var p = new frmTurnPopUp("Your turn!")) p.ShowDialog(this);
                isLeftTimerRunning = true;
            }
            else if (random == 1 && _isHost)
            {
                isYourTurn = false;
                await _hub.InvokeAsync("Turn", _room.Id, false);
                using (var p = new frmTurnPopUp("Opponent Turn!")) p.ShowDialog(this);
                isRightTimerRunning = true;
            }
            if (!_isHost)
            {
                _hub.On<bool>("Turn", (isHostTurn) =>
                {
                    isYourTurn = !isHostTurn;
                    if (isYourTurn == true)
                    {
                        isRightTimerRunning = false;
                        isLeftTimerRunning = true;
                    }
                    if (isYourTurn == false)
                    {
                        isLeftTimerRunning = false;
                        isRightTimerRunning = true;
                    }
                });
            }
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
                        btn.Click += GridButton_Click;// only opponent's grid is clickable
                        btn.MouseMove += GridSkillPreview_MouseMove;
                        btn.MouseLeave += ClearPreview;

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
            Point pos = (Point)btn.Tag;
            int row = pos.X;
            int col = pos.Y;

            if (!isYourTurn && !_isUsingSkill)
            {
                return;
            }

            // ===== SKILL: Elizabeth / Will =====
            if (_isUsingSkill &&
               (GlobalData.SelectedCharacter == CharacterType.ElizabethSwann ||
                GlobalData.SelectedCharacter == CharacterType.WillTurner))
            {
                if (_previewCells.Count == 0)
                    return;

                foreach (var p in _previewCells)
                {
                    FireCell(p.X, p.Y);
                }

                ClearPreview(null, null);
                EndSkill();
                return;
            }


            // ===== SKILL: Jack =====
            if (_isUsingSkill && GlobalData.SelectedCharacter == CharacterType.JackSparrow)
            {
                FireCell(row, col);
                GlobalData.SkillRemainingShots--;
                btnSkill.Text = $"Đạn: {GlobalData.SkillRemainingShots}";

                if (GlobalData.SkillRemainingShots == 0)
                    EndSkill();

                return;
            }

            // ===== BẮN THƯỜNG =====
            bool isHit = OpponentShipPos[row, col] == 1;
            FireCell(row, col);

            if (!isHit)
                TurnSwitch();
        }

        private void GridSkillPreview_MouseMove(object sender, MouseEventArgs e)
        {
            
            if (!_isUsingSkill) return;
            _previewCells.Clear();

            Button btn = sender as Button;
            Point pos = (Point)btn.Tag;

            ClearPreview(null, null);

            if (_isUsingSkill && GlobalData.SelectedCharacter == CharacterType.ElizabethSwann)
            {



                ClearPreview(null, null);
                _previewCells.Clear();

                if (_elizabethIsRow)
                {
                    for (int c = 0; c < mapsize; c++)
                    {
                        opponentGrid[pos.X, c].BackColor = Color.FromArgb(120, Color.Red);
                        _previewCells.Add(new Point(pos.X, c));
                    }
                }
                else
                {
                    for (int r = 0; r < mapsize; r++)
                    {
                        opponentGrid[r, pos.Y].BackColor = Color.FromArgb(120, Color.Red);
                        _previewCells.Add(new Point(r, pos.Y));
                    }
                }

                return;
            }

            else if (GlobalData.SelectedCharacter == CharacterType.WillTurner)
            {
                ClearPreview(null, null);
                _previewCells.Clear();

                for (int r = pos.X - 1; r <= pos.X + 1; r++)
                {
                    for (int c = pos.Y - 1; c <= pos.Y + 1; c++)
                    {
                        if (r >= 0 && r < mapsize && c >= 0 && c < mapsize)
                        {
                            opponentGrid[r, c].BackColor = Color.FromArgb(120, Color.Red);
                            _previewCells.Add(new Point(r, c));
                        }
                    }
                }

                return;
            }
        }

        private async void FireCell(int row, int col)
        {

            Color c = opponentGrid[row, col].BackColor;

            // chỉ chặn khi ô đã bắn thật
            if (c == Color.Red || c == Color.Green)
                return;

            bool isHit = OpponentShipPos[row, col] == 1;


            opponentGrid[row, col].BackColor = isHit ? Color.Red : Color.Green;

            if (isHit)
            {
                yourScore++;
                ScoreTracking();
            }


            await _hub.InvokeAsync("Hit", _room.Id, row, col, isHit);
        }

        private void EndSkill()
        {
            GlobalData.IsSkillUsed = true;
            _isUsingSkill = false;
            btnSkill.Text = "Hết Skill";
            btnSkill.Enabled = false;
            _suppressTurnSwitch = false;
        }


        private void ClearPreview(object sender, EventArgs e)
        {
            foreach (var p in _previewCells)
                opponentGrid[p.X, p.Y].BackColor = Color.LightBlue;

            _previewCells.Clear();
        }

        private void ReceiveHit()
        {
            _hub.On<int, int, bool>("ReceiveHit", (row, col, isHit) =>
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

                if (!_suppressTurnSwitch)
                {
                    TurnSwitch();
                }
            }
        });
            _hub.On<List<dynamic>>("ReceiveHectorSkill", shots =>
            {
                foreach (var s in shots)
                {
                    int r = (int)s.r;
                    int c = (int)s.c;
                    bool isHit = (bool)s.hit;

                    playerGrid[r, c].BackColor = isHit ? Color.Red : Color.Green;

                    if (isHit)
                    {
                        opponentScore++;
                        ScoreTracking();
                    }
                }
            });
        }
        public void BtnSkill_Click(object sender, EventArgs e)
        {
            if (!isYourTurn) return;
            if (GlobalData.IsSkillUsed) return;

            _isUsingSkill = true;

            switch (GlobalData.SelectedCharacter)
            {
                case CharacterType.ElizabethSwann:
                case CharacterType.WillTurner:

                    break;

                case CharacterType.HectorBarbossa:
                    SkillHectorBarbossa();
                    EndSkill();
                    break;

                case CharacterType.JackSparrow:
                    GlobalData.SkillRemainingShots = 5;
                    btnSkill.Text = "Đạn: 5";
                    _suppressTurnSwitch = true;
                    break;
            }


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

        private void FrmInBattle_KeyDown(object sender, KeyEventArgs e)
        {
            if (!_isUsingSkill) return;
            if (GlobalData.SelectedCharacter != CharacterType.ElizabethSwann) return;

            if (e.KeyCode == Keys.R)
            {
                _elizabethIsRow = !_elizabethIsRow;
                ClearPreview(null, null); // vẽ lại preview khi đổi hướng
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
            if (opponentScore == 14)
            {

                isLeftTimerRunning = false;
                isRightTimerRunning = false;
                IndexCurrentMatch();
                await SendBattleResultAsync(false);
                frmResult frmResult = new frmResult("You LOSE", -1);
                frmResult.ShowDialog();
                this.Close();
            }
            if (yourScore == 14)
            {
                isLeftTimerRunning = false;
                isRightTimerRunning = false;
                IndexCurrentMatch();
                await SendBattleResultAsync(true);
                frmResult frmResult = new frmResult("You WON!", 1);
                frmResult.ShowDialog();
                this.Close();
            }
        }
        private async void SkillHectorBarbossa()
        {
            Random rnd = new Random();
            List<(int r, int c, bool hit)> shots = new List<(int, int, bool)>();
            HashSet<Point> used = new HashSet<Point>();

            while (shots.Count < 6)
            {
                int r = rnd.Next(0, mapsize);
                int c = rnd.Next(0, mapsize);

                if (used.Contains(new Point(r, c))) continue;
                if (opponentGrid[r, c].BackColor != Color.LightBlue) continue;

                bool isHit = OpponentShipPos[r, c] == 1;
                used.Add(new Point(r, c));
                shots.Add((r, c, isHit));
            }

            foreach (var s in shots)
            {
                opponentGrid[s.r, s.c].BackColor = s.hit ? Color.Red : Color.Green;
                if (s.hit)
                {
                    yourScore++;
                    ScoreTracking();
                }
            }

            await _hub.InvokeAsync("HectorSkill", _room.Id, shots);
        }

        private async void frmIn_Battle_Load(object sender, EventArgs e)
        {
            await ChatSession.ChatBox.SetBattleContextAsync(_idTranDau);
            ChatSession.ChatBox.LoadHistory();



            this.FormBorderStyle = FormBorderStyle.None; // removes title bar
            this.WindowState = FormWindowState.Maximized; // maximize to full screen

            leftTime = TimeSpan.FromSeconds(180);
            rightTime = TimeSpan.FromSeconds(180);

            lblLeftTimer.Text = leftTime.ToString(@"mm\:ss");
            lblRightTimer.Text = rightTime.ToString(@"mm\:ss");

            timer = new Timer();
            timer.Interval = 1000; // every second
            timer.Tick += Timer_Tick;
            timer.Start(); // start automatically


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


    }
}
