using System;

namespace NT106_BattleshipClient
{
    public partial class frmUserInfo : BaseForm
    {
        public frmUserInfo()
        {
            InitializeComponent();

            // chống nháy form
            EnableFormDoubleBuffering();
            LoadUserInfo();

        }

        private void LoadUserInfo()
        {
            lblID.Text = $"ID : {GlobalData.UserId}";
            lblTen.Text = $"Tên : {GlobalData.Username}";
            lblEmail.Text = $"Email : {GlobalData.Email}";


            lblSao.Text = $"Số Sao : {GlobalData.SoSao}";
            lblTongSoTran.Text = $"Tổng số trận : {GlobalData.TongSoTran}";
            lblTiLeThang.Text = $"Tỉ lệ thắng : {GlobalData.TiLeThang}%";


        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmUserInfo_Load(object sender, EventArgs e)
        {
            //Ẩn thanh tiêu đề nếu cần
            //    this.FormBorderStyle = FormBorderStyle.None;
        }

        private void btnLichSuDau_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmMatchHistory matchHistoryForm = new frmMatchHistory();
            matchHistoryForm.ShowDialog();
            this.Show();
        }

        private void panelUserInfo_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {

        }
    }
}
