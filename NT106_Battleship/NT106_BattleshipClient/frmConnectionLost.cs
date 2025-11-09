using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NT106_BattleshipClient
{
    public partial class frmConnectionLost : BaseForm
    {
        public frmConnectionLost()
        {
            InitializeComponent();
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
