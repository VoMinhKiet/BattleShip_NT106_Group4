using System;
using System.Windows.Forms;

namespace NT106_BattleshipClient
{
    public partial class frmSurrender : BaseForm
    {
        public frmSurrender()
        {
            InitializeComponent();

            // chống nháy form
            EnableFormDoubleBuffering();

        }

        private void btnDauHang_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close(); // Đóng form pop-up
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close(); // Đóng form pop-up
        }
        private void frmSurrender_Load(object sender, EventArgs e)
        {

        }
    }
}
