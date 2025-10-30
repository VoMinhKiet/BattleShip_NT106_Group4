using System;
using System.Drawing;
using System.Windows.Forms;
using System.Media; 
using System.IO; 

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


            if (!DesignMode)
            {
                LoadCustomCursorsAndSound();
                
            }
        }


        private void LoadCustomCursorsAndSound()
        {

            string resourcesPath = "Resources/Cursor/";



            try
            {
                // Tải Con trỏ
                cursorDefault = new Cursor(resourcesPath + "CursorImage.cur");
                cursorClick = new Cursor(resourcesPath + "CursorAnimation.cur");


                this.Cursor = cursorDefault;


                clickSoundPlayer = new System.Media.SoundPlayer(resourcesPath + "ClickSound.wav");
                clickSoundPlayer.Load();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Lỗi tải tài nguyên con trỏ/âm thanh: " + ex.Message, "Lỗi Tải Tài Nguyên", MessageBoxButtons.OK, MessageBoxIcon.Error);
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


                if (clickSoundPlayer != null)
                {
                    clickSoundPlayer.Stop();
                    clickSoundPlayer.Play();
                }
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