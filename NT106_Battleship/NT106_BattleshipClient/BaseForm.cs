using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO; 
using System.Media; 
using System.Windows.Forms;

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