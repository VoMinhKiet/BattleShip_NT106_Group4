using Microsoft.AspNetCore.SignalR.Client;
using NT106_BattleshipClient.Models;
using System;
using System.Drawing;
using System.Threading.Tasks;
//using System.Web.UI.WebControls;
using System.Windows.Forms;

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
        private bool _ElizabethSwannSkillUse = false;
        private bool _HectorBarbossaSkillUse = false;
        public int skillUsage = 3;
        public bool first = true;
        private bool _turnHandlerRegistered = false;
        private readonly TranDauDto _currentMatch;
        private readonly RoomDto _room;
        private readonly LeaderBoardDto _LeaderBoard;
        private HubConnection _hub;
        private HubConnection _rankingHub;
        private bool _battleEnded = false;
        private frmRoom _frmRoom;
        private frmShip_Sorting _frmShip_Sorting;


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

            Button btnSkill = new Button();
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



        // register other handlers (hits)


        // now it's safe for host to decide/start tur
        private void FrmIn_Battle_KeyDown(object sender, KeyEventArgs e)
        {
            // Toggle orientation when user presses 'R' (no UI indicator required)
            if (e.KeyCode == Keys.R)
            {
                _ElizabethSwannSkillOrientationRow = !_ElizabethSwannSkillOrientationRow;
            }
        }
        /*private void ReceiveTurn()
        {
            _hub.Remove("Turn");

            _hub.On<bool>("Turn", (isHostTurn) =>
            {
                if (this.IsHandleCreated && this.InvokeRequired)
                    this.BeginInvoke(new Action(() => HandleTurnMessage(isHostTurn)));
                else
                    HandleTurnMessage(isHostTurn);
            });
        }*/

        /*private void HandleTurnMessage(bool isHostTurn)
        {
            // prevent multiple concurrent popups / handle reuse errors
            if (_turnPopupOpen) return;
            _turnPopupOpen = true;

            try
            {
                bool yourTurn = (_isHost && isHostTurn) || (!_isHost && !isHostTurn);
                isYourTurn = yourTurn;
                isLeftTimerRunning = yourTurn;
                isRightTimerRunning = !yourTurn;

                string message = yourTurn ? "You go First!" : "You go Second!";
                using (var popup = new frmTurnPopUp(message))
                {
                    // ShowDialog must run on UI thread; InvokeRequired checks already done by callers.
                    popup.ShowDialog(this);
                }
            }
            finally
            {
                _turnPopupOpen = false;
            }
        }
        */

        /* private async Task SubscribeAndSyncTurnAsync()
        {
            if (_hub == null) return;

            // register handler immediately (so server pushes won't be missed)
            //ReceiveTurn();

            // query server for the current turn state (server keeps canonical state)
            try
            {
                bool hostTurn = await _hub.InvokeAsync<bool>("GetTurnStatus", _room.Id);
                // apply current state on UI thread
                if (this.IsHandleCreated)
                    this.BeginInvoke(new Action(() => HandleTurnMessage(hostTurn)));
            }
            catch
            {
                //bat loi exception hoac log lai tuy y 
            }
        } */






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
                        btn.Click += GridButton_Click; // only opponent's grid is clickable
                        btn.Cursor = Cursors.Hand;
                    }
                    container.Controls.Add(btn);
                    grid[row, col] = btn;
                }
            }
        }
        /*private async void GridButton_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn == null) return;
            if (!isYourTurn) return;

            var pos = (Point)btn.Tag;
            int row = pos.X;
            int col = pos.Y;

            if (_ElizabethSwannSkillUse)
            {
                _ElizabethSwannSkillUse = false;

                ElizabethSwann(row, col);
                return;
            }
            if (_HectorBarbossaSkillUse)
            {
                _HectorBarbossaSkillUse = false;
                HectorBarbossa(row, col);
                return;
            }

            await HandleShotAsync(pos.X, pos.Y);
        }*/
        /*private async Task HandleShotAsync(int row, int col, bool suppressTurnSwitch = false)
         {
             // bounds
             if (row < 0 || col < 0 || row >= opponentGrid.GetLength(0) || col >= opponentGrid.GetLength(1))
                 return;

             var btn = opponentGrid[row, col];
             if (btn == null) return;

             // only act on unrevealed cells
             if (btn.BackColor != Color.LightBlue) return;

             // disable to prevent double actions
             btn.Enabled = false;

             bool isHit = OpponentShipPos != null &&
                          row >= 0 && row < OpponentShipPos.GetLength(0) &&
                          col >= 0 && col < OpponentShipPos.GetLength(1) &&
                          OpponentShipPos[row, col] == 1;

             // update UI immediately
             btn.BackColor = isHit ? Color.Red : Color.Green;

             try
             {
                 await _hub.InvokeAsync("Hit", _room.Id, row, col, isHit);
             }
             catch (Exception ex)
             {
                 // revert UI on failure
                 btn.Enabled = true;
                 btn.BackColor = Color.LightBlue;
                 MessageBox.Show($"Failed to send hit for {row},{col}: {ex.Message}", "SignalR Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 return;
             }

             if (isHit)
             {
                 yourScore++;
                 ScoreTracking();
             }
             else
             {
                 if (!suppressTurnSwitch)
                 TurnSwitch();
             }
         }*/
        /*public void ElizabethSwann(int row, int col)
        {
            // marshal to UI thread and run the async sequence
            if (this.IsHandleCreated && this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => { _ = ElizabethSwannAsync(row, col); }));
            }
            else
            {
                _ = ElizabethSwannAsync(row, col);
            }
        }*/

        /*private async Task ElizabethSwannAsync(int row, int col)
        {
            if (!isYourTurn) return;

            int rows = opponentGrid.GetLength(0);
            int cols = opponentGrid.GetLength(1);

            if (_ElizabethSwannSkillOrientationRow)
            {
                // click entire row from left to right
                for (int c = 0; c < cols; c++)
                {
                    // stop if it's no longer your turn (safety)
                    if (!isYourTurn) break;
                    await HandleShotAsync(row, c, suppressTurnSwitch: true);
                    await Task.Delay(40);
                }
            }
            else
            {
                // click entire column from top to bottom
                for (int r = 0; r < rows; r++)
                {
                    if (!isYourTurn) break;
                    await HandleShotAsync(r, col, suppressTurnSwitch: true);
                    await Task.Delay(40);
                }
            }

            // Always switch turn after using the skill (regardless of hit/miss)
            TurnSwitch();
        }*/

        /*public void HectorBarbossa(int row, int col)
        {
            if (this.IsHandleCreated && this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => { _ = HectorBarbossaAsync(row, col); }));
            }
            else
            {
                _ = HectorBarbossaAsync(row, col);
            }
        }*/
        /*private async Task HectorBarbossaAsync(int row, int col)
        {
            if (!isYourTurn) return;

            int rows = opponentGrid.GetLength(0);
            int cols = opponentGrid.GetLength(1);

            // iterate a 3x3 area centered on (row,col)
            for (int r = row - 1; r <= row + 1; r++)
            {
                for (int c = col - 1; c <= col + 1; c++)
                {
                    if (r < 0 || c < 0 || r >= rows || c >= cols) continue;

                    // stop early if turn changed (safety)
                    if (!isYourTurn) break;

                    await HandleShotAsync(r, c, suppressTurnSwitch: true);

                    // small delay to avoid flooding server; adjust if needed
                    await Task.Delay(40);
                }

                if (!isYourTurn) break;
            }

            // Always switch turn after using the skill (regardless of hit/miss)
            TurnSwitch();
        }*/
        private async void GridButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;
            if (!isYourTurn)
            {
                //MessageBox.Show("It's already hit!");
                return;
            }
            Point pos = (Point)btn.Tag;

            int row = pos.X;
            int col = pos.Y;

            // Prevent repeated clicks
            //btn.Enabled = isYourTurn;
            bool isHit = false;
            btn.Enabled = true;

            if (OpponentShipPos[row, col] == 1 && btn.BackColor == Color.LightBlue)
            {
                isHit = true;
            }
            else if (OpponentShipPos[row, col] == 0 && btn.BackColor == Color.LightBlue)
            {
                isHit = false;

            }
            else if (btn.BackColor == Color.Red)
            {
                MessageBox.Show("You already hit this spot and it's a HIT!");
                return;
            }

            try
            {
                if (isHit)
                {
                    btn.BackColor = Color.Red; // hit
                    await _hub.InvokeAsync("Hit", _room.Id, row, col, true);
                    yourScore++;
                    ScoreTracking();
                    isHit = false;
                    //return;
                    //TurnSwitch();

                }
                else if (!isHit)
                {
                    btn.BackColor = Color.Green; // miss
                    await _hub.InvokeAsync("Hit", _room.Id, row, col, false);
                    TurnSwitch();

                }

            }
            catch (Exception ex)
            {
                // re-enable button on failure and show error for debugging
                btn.Enabled = true;
                MessageBox.Show($"Failed to send hit: {ex.Message}", "SignalR Invoke Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                TurnSwitch();
            }
        });
        }
        public void BtnSkill_Click(object sender, EventArgs e)
        {
            if (!isYourTurn)
            {
                MessageBox.Show("It's not your turn!");
                return;
            }
            else
            {
                if (_isHost && skillUsage >= 1)
                {
                    if (_currentMatch.TenNV1 == "Elizabeth Swann")
                    {
                        _ElizabethSwannSkillUse = true;
                    }
                }
                if (!_isHost && skillUsage >= 1)
                {
                    if (_currentMatch.TenNV2 == "Elizabeth Swann")
                    {
                        _ElizabethSwannSkillUse = true;
                    }
                }
                if (_isHost && skillUsage >= 1)
                {
                    if (_currentMatch.TenNV1 == "Hector Barbossa")
                    {
                        _HectorBarbossaSkillUse = true;
                    }
                }
                if (!_isHost && skillUsage >= 1)
                {
                    if (_currentMatch.TenNV2 == "Hector Barbossa")
                    {
                        _HectorBarbossaSkillUse = true;
                    }
                }

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

        /*private void ReceiveHit()
        {
            _hub.On<int, int, bool>("ReceiveHit", (row, col, isHit) =>
            {
                if (this.IsHandleCreated)
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        // update UI quickly, then perform any turn switching without blocking
                        _ = ProcessReceiveHitAsync(row, col, isHit);
                    }));
                }
                else
                {
                    _ = ProcessReceiveHitAsync(row, col, isHit);
                }
            });
        }*/
        /*private async Task ProcessReceiveHitAsync(int row, int col, bool isHit)
        {
            Button btn = playerGrid[row, col];
            if (btn != null)
            {
                btn.Enabled = false;
                if (isHit)
                {
                    btn.BackColor = Color.Red;
                    opponentScore++;
                    ScoreTracking();
                    // do not change turn on hit
                }
                else
                {
                    btn.BackColor = Color.Green;
                    // fire-and-forget turn switch (TurnSwitchAsync returns completed task)
                    TurnSwitch();
                }
            }
        }*/
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
