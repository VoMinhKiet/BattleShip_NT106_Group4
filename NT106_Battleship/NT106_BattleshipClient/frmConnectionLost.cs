using System;
using System.Windows.Forms;

namespace NT106_BattleshipClient
{
    public partial class frmConnectionLost : BaseForm
    {
        public frmConnectionLost()
        {
            InitializeComponent();

            // chống nháy form
            EnableFormDoubleBuffering();
        }
        private void btnThuLai_Click(object sender, EventArgs e)
        {
            //Báo cho form cha biết kết quả là "Retry"
            this.DialogResult = DialogResult.Retry;
            this.Close();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            //Báo cho form cha biết kết quả là "Cancel"
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
