using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO; 
using System.Media; 
using System.Windows.Forms;
using System.Reflection; //đang test 


namespace NT106_BattleshipClient
{

    public partial class BaseForm : Form
    {

        protected Cursor cursorDefault = null;
        protected Cursor cursorClick = null;


        protected System.Media.SoundPlayer clickSoundPlayer = null;


        public BaseForm()
        {
            InitializeComponent();

           
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Kiểm tra design-time: chỉ load tài nguyên khi chạy app thực tế
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                LoadCustomCursorsAndSound();
                AttachMouseEventsRecursive(this);
            }
        }
        //test chống nháy các form
        #region DoubleBuffer helpers for derived forms


        protected void EnableFormDoubleBuffering()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint
                          | ControlStyles.UserPaint
                          | ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();
            this.DoubleBuffered = true;
        }


        protected void SetControlDoubleBuffered(Control ctl, bool enabled = true)
        {
            if (ctl == null) return;

            // Thử set bằng reflection để cover protected DoubleBuffered property (ví dụ Panel)
            var prop = ctl.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (prop != null && prop.CanWrite)
            {
                prop.SetValue(ctl, enabled, null);
            }

            // Cố gắng gọi SetStyle/UpdateStyles nếu control hỗ trợ (non-public)
            var miSetStyle = ctl.GetType().GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (miSetStyle != null)
            {
                object[] args = new object[] { ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, enabled };
                try { miSetStyle.Invoke(ctl, args); }
                catch { /* ignore */ }

                var miUpdate = ctl.GetType().GetMethod("UpdateStyles", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                miUpdate?.Invoke(ctl, null);
            }
        }

    
        protected void SetDoubleBufferedForAllChildren(Control parent, bool enabled = true)
        {
            if (parent == null) return;
            foreach (Control c in parent.Controls)
            {
                SetControlDoubleBuffered(c, enabled);
                if (c.HasChildren)
                    SetDoubleBufferedForAllChildren(c, enabled);
            }
        }


        protected bool UseCompositedFlag = false; // mặc định false

        protected void SetUseComposited(bool enable)
        {
            UseCompositedFlag = enable;
        }

        // Override CreateParams để áp dụng WS_EX_COMPOSITED nếu UseCompositedFlag = true
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                if (UseCompositedFlag)
                {
                    const int WS_EX_COMPOSITED = 0x02000000;
                    cp.ExStyle |= WS_EX_COMPOSITED;
                }
                return cp;
            }
        }

        #endregion
        //end test
        private void LoadCustomCursorsAndSound()
        {

            string resourcesDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Cursor");

            try
            {
                // Tải Con trỏ
                string defaultCursorPath = Path.Combine(resourcesDir, "CursorImage.cur");
                string clickCursorPath = Path.Combine(resourcesDir, "CursorAnimation.cur");
                string clickSoundPath = Path.Combine(resourcesDir, "ClickSound.wav"); ;


                // kiểm tra và load cursor mặc định (nếu có)
                if (File.Exists(defaultCursorPath))
                {
                    cursorDefault = new Cursor(defaultCursorPath);
                    this.Cursor = cursorDefault;
                }
                else
                {
                    Debug.WriteLine($"Cursor default not found: {defaultCursorPath}");
                }

                // load cursor click (nếu có)
                if (File.Exists(clickCursorPath))
                {
                    cursorClick = new Cursor(clickCursorPath);
                }
                else
                {
                    Debug.WriteLine($"Cursor click not found: {clickCursorPath}");
                }

                // load sound (nếu có)
                if (File.Exists(clickSoundPath))
                {
                    clickSoundPlayer = new System.Media.SoundPlayer(clickSoundPath);
                    // bạn có thể LoadAsync() nếu muốn
                    try { clickSoundPlayer.Load(); }
                    catch (Exception exLoad) { Debug.WriteLine("SoundPlayer load failed: " + exLoad.Message); clickSoundPlayer = null; }
                }
                else
                {
                    Debug.WriteLine($"Click sound not found: {clickSoundPath}");
                }
            }
            catch (Exception ex)
            {
                // KHÔNG show MessageBox trên lỗi load tài nguyên (sẽ gây annoying khi lỗi)
                Debug.WriteLine("Lỗi tải tài nguyên con trỏ/âm thanh: " + ex);
                // nếu muốn hiển thị cho dev khi debug:
#if DEBUG
                MessageBox.Show("Lỗi tải tài nguyên con trỏ/âm thanh: " + ex.Message, "Lỗi Tải Tài Nguyên", MessageBoxButtons.OK, MessageBoxIcon.Error);
#endif
            }
        }


        protected void AttachMouseEventsRecursive(Control parent)
        {
            foreach (Control control in parent.Controls)
            {

                control.MouseEnter += OnControlMouseEnter;
                control.MouseLeave += OnControlMouseLeave;
                control.MouseDown += OnControlMouseDown;
                control.MouseUp += OnControlMouseUp;


                if (control.HasChildren)
                {
                    AttachMouseEventsRecursive(control);
                }
            }
        }



        protected virtual void OnControlMouseEnter(object sender, EventArgs e)
        {
            this.Cursor = cursorDefault;
        }

        protected virtual void OnControlMouseLeave(object sender, EventArgs e)
        {
            this.Cursor = cursorDefault;
        }

        protected virtual void OnControlMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && cursorClick != null)
            {
                this.Cursor = cursorClick;


                /*if (clickSoundPlayer != null)
                {
                    clickSoundPlayer.Stop();
                    clickSoundPlayer.Play();
                }*/
            }   
        }

        protected virtual void OnControlMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && cursorDefault != null)
            {
                this.Cursor = cursorDefault; 
            }
        }


        private void BaseForm_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                AttachMouseEventsRecursive(this);
            }
        }
    }
}