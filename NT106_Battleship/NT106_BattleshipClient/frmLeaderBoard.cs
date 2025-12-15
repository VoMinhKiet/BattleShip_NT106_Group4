using System;

namespace NT106_BattleshipClient
{
    public partial class frmLeaderBoard : BaseForm
    {
        public frmLeaderBoard()
        {
            InitializeComponent();

            // chống nháy form
            EnableFormDoubleBuffering();
        }

        private void frmLeaderBoard_Load(object sender, EventArgs e)
        {
            //  this.FormBorderStyle = FormBorderStyle.None;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
