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
    public partial class frmUserInfo : BaseForm
    {
        public frmUserInfo()
        {
            InitializeComponent();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmUserInfo_Load(object sender, EventArgs e)
        {
            //Ẩn thanh tiêu đề nếu cần
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void btnLichSuDau_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmMatchHistory matchHistoryForm = new frmMatchHistory();
            matchHistoryForm.ShowDialog();
            this.Show();
        }
    }
}
