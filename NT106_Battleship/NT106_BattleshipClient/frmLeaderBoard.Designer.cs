namespace NT106_BattleshipClient
{
    partial class frmLeaderBoard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLeaderBoard));
            this.lblTieuDe = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.STT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TenNguoiChoi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Hang = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SoSao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TiLeThang = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TongSoTran = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTieuDe
            // 
            this.lblTieuDe.AutoSize = true;
            this.lblTieuDe.BackColor = System.Drawing.Color.Transparent;
            this.lblTieuDe.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTieuDe.Location = new System.Drawing.Point(352, 53);
            this.lblTieuDe.Name = "lblTieuDe";
            this.lblTieuDe.Size = new System.Drawing.Size(297, 46);
            this.lblTieuDe.TabIndex = 0;
            this.lblTieuDe.Text = "BẢNG XẾP HẠNG";
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.STT,
            this.TenNguoiChoi,
            this.Hang,
            this.SoSao,
            this.TiLeThang,
            this.TongSoTran});
            this.dataGridView1.GridColor = System.Drawing.Color.Black;
            this.dataGridView1.Location = new System.Drawing.Point(101, 128);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(803, 339);
            this.dataGridView1.TabIndex = 1;
            // 
            // STT
            // 
            this.STT.HeaderText = "STT";
            this.STT.MinimumWidth = 6;
            this.STT.Name = "STT";
            this.STT.Width = 125;
            // 
            // TenNguoiChoi
            // 
            this.TenNguoiChoi.HeaderText = "Tên Người Chơi";
            this.TenNguoiChoi.MinimumWidth = 6;
            this.TenNguoiChoi.Name = "TenNguoiChoi";
            this.TenNguoiChoi.Width = 125;
            // 
            // Hang
            // 
            this.Hang.HeaderText = "Hạng";
            this.Hang.MinimumWidth = 6;
            this.Hang.Name = "Hang";
            this.Hang.Width = 125;
            // 
            // SoSao
            // 
            this.SoSao.HeaderText = "Số Sao";
            this.SoSao.MinimumWidth = 6;
            this.SoSao.Name = "SoSao";
            this.SoSao.Width = 125;
            // 
            // TiLeThang
            // 
            this.TiLeThang.HeaderText = "Tỉ Lệ Thắng";
            this.TiLeThang.MinimumWidth = 6;
            this.TiLeThang.Name = "TiLeThang";
            this.TiLeThang.Width = 125;
            // 
            // TongSoTran
            // 
            this.TongSoTran.HeaderText = "Tổng Số Trận";
            this.TongSoTran.MinimumWidth = 6;
            this.TongSoTran.Name = "TongSoTran";
            this.TongSoTran.Width = 125;
            // 
            // frmLeaderBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(916, 489);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.lblTieuDe);
            this.Name = "frmLeaderBoard";
            this.Text = "frmLeaderBoard";
            this.Load += new System.EventHandler(this.frmLeaderBoard_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTieuDe;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn STT;
        private System.Windows.Forms.DataGridViewTextBoxColumn TenNguoiChoi;
        private System.Windows.Forms.DataGridViewTextBoxColumn Hang;
        private System.Windows.Forms.DataGridViewTextBoxColumn SoSao;
        private System.Windows.Forms.DataGridViewTextBoxColumn TiLeThang;
        private System.Windows.Forms.DataGridViewTextBoxColumn TongSoTran;
    }
}