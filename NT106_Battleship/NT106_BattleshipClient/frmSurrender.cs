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
    public partial class frmSurrender : Form
    {
        public frmSurrender()
        {
            InitializeComponent();
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
