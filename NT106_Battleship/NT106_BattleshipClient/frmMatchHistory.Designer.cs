namespace NT106_BattleshipClient
{
    partial class frmMatchHistory
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMatchHistory));
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlDong = new System.Windows.Forms.Panel();
            this.btnDong = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.guna2VScrollBar1 = new Guna.UI2.WinForms.Guna2VScrollBar();
            this.dgvLichSuDau = new System.Windows.Forms.DataGridView();
            this.colID1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNguoiChoi1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNhanVat1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colID2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNguoiChoi2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNhanVat2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colKetQua = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBatDau = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colKetThuc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlDong.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLichSuDau)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(20, 20);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1880, 1040);
            this.panel1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 86.25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.75F));
            this.tableLayoutPanel1.Controls.Add(this.pnlDong, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17.14286F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 82.85714F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1880, 1040);
            this.tableLayoutPanel1.TabIndex = 0;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // pnlDong
            // 
            this.pnlDong.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pnlDong.Controls.Add(this.btnDong);
            this.pnlDong.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDong.ForeColor = System.Drawing.Color.Black;
            this.pnlDong.Location = new System.Drawing.Point(1671, 0);
            this.pnlDong.Margin = new System.Windows.Forms.Padding(50, 0, 0, 30);
            this.pnlDong.Name = "pnlDong";
            this.pnlDong.Padding = new System.Windows.Forms.Padding(5);
            this.pnlDong.Size = new System.Drawing.Size(209, 148);
            this.pnlDong.TabIndex = 4;
            // 
            // btnDong
            // 
            this.btnDong.BackColor = System.Drawing.Color.SaddleBrown;
            this.btnDong.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDong.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDong.Font = new System.Drawing.Font("Segoe UI Black", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDong.ForeColor = System.Drawing.Color.SandyBrown;
            this.btnDong.Location = new System.Drawing.Point(5, 5);
            this.btnDong.Margin = new System.Windows.Forms.Padding(50, 0, 0, 40);
            this.btnDong.Name = "btnDong";
            this.btnDong.Size = new System.Drawing.Size(199, 138);
            this.btnDong.TabIndex = 3;
            this.btnDong.Text = "Đóng";
            this.btnDong.UseVisualStyleBackColor = false;
            this.btnDong.Click += new System.EventHandler(this.btnDong_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.SaddleBrown;
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 178);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(10);
            this.panel2.Size = new System.Drawing.Size(1621, 862);
            this.panel2.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Bisque;
            this.panel3.Controls.Add(this.guna2VScrollBar1);
            this.panel3.Controls.Add(this.dgvLichSuDau);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(10, 10);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(30);
            this.panel3.Size = new System.Drawing.Size(1601, 842);
            this.panel3.TabIndex = 0;
            // 
            // guna2VScrollBar1
            // 
            this.guna2VScrollBar1.Dock = System.Windows.Forms.DockStyle.Right;
            this.guna2VScrollBar1.FillColor = System.Drawing.Color.BurlyWood;
            this.guna2VScrollBar1.InUpdate = false;
            this.guna2VScrollBar1.LargeChange = 10;
            this.guna2VScrollBar1.Location = new System.Drawing.Point(1556, 30);
            this.guna2VScrollBar1.Name = "guna2VScrollBar1";
            this.guna2VScrollBar1.ScrollbarSize = 15;
            this.guna2VScrollBar1.Size = new System.Drawing.Size(15, 782);
            this.guna2VScrollBar1.TabIndex = 1;
            this.guna2VScrollBar1.ThumbColor = System.Drawing.Color.SaddleBrown;
            // 
            // dgvLichSuDau
            // 
            this.dgvLichSuDau.AllowUserToAddRows = false;
            this.dgvLichSuDau.AllowUserToDeleteRows = false;
            this.dgvLichSuDau.AllowUserToResizeColumns = false;
            this.dgvLichSuDau.AllowUserToResizeRows = false;
            this.dgvLichSuDau.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLichSuDau.BackgroundColor = System.Drawing.Color.Bisque;
            this.dgvLichSuDau.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Peru;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.BlanchedAlmond;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Peru;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.BlanchedAlmond;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvLichSuDau.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvLichSuDau.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLichSuDau.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colID1,
            this.colNguoiChoi1,
            this.colNhanVat1,
            this.colID2,
            this.colNguoiChoi2,
            this.colNhanVat2,
            this.colKetQua,
            this.colBatDau,
            this.colKetThuc});
            this.dgvLichSuDau.Cursor = System.Windows.Forms.Cursors.Default;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Info;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.DarkGoldenrod;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.BurlyWood;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.SaddleBrown;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvLichSuDau.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvLichSuDau.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLichSuDau.EnableHeadersVisualStyles = false;
            this.dgvLichSuDau.GridColor = System.Drawing.Color.SaddleBrown;
            this.dgvLichSuDau.Location = new System.Drawing.Point(30, 30);
            this.dgvLichSuDau.Margin = new System.Windows.Forms.Padding(0, 0, 80, 0);
            this.dgvLichSuDau.Name = "dgvLichSuDau";
            this.dgvLichSuDau.ReadOnly = true;
            this.dgvLichSuDau.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.PeachPuff;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.DarkGoldenrod;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.SaddleBrown;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.SandyBrown;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvLichSuDau.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvLichSuDau.RowHeadersVisible = false;
            this.dgvLichSuDau.RowHeadersWidth = 51;
            this.dgvLichSuDau.RowTemplate.Height = 24;
            this.dgvLichSuDau.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvLichSuDau.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLichSuDau.Size = new System.Drawing.Size(1541, 782);
            this.dgvLichSuDau.TabIndex = 0;
            this.dgvLichSuDau.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvLichSuDau_CellPainting);
            // 
            // colID1
            // 
            this.colID1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colID1.FillWeight = 5F;
            this.colID1.HeaderText = "ID1";
            this.colID1.MinimumWidth = 6;
            this.colID1.Name = "colID1";
            this.colID1.ReadOnly = true;
            // 
            // colNguoiChoi1
            // 
            this.colNguoiChoi1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colNguoiChoi1.FillWeight = 16F;
            this.colNguoiChoi1.HeaderText = "Người chơi 1";
            this.colNguoiChoi1.MinimumWidth = 6;
            this.colNguoiChoi1.Name = "colNguoiChoi1";
            this.colNguoiChoi1.ReadOnly = true;
            // 
            // colNhanVat1
            // 
            this.colNhanVat1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colNhanVat1.FillWeight = 15F;
            this.colNhanVat1.HeaderText = "Nhân vật 1";
            this.colNhanVat1.MinimumWidth = 6;
            this.colNhanVat1.Name = "colNhanVat1";
            this.colNhanVat1.ReadOnly = true;
            // 
            // colID2
            // 
            this.colID2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colID2.FillWeight = 5F;
            this.colID2.HeaderText = "ID2";
            this.colID2.MinimumWidth = 6;
            this.colID2.Name = "colID2";
            this.colID2.ReadOnly = true;
            // 
            // colNguoiChoi2
            // 
            this.colNguoiChoi2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colNguoiChoi2.FillWeight = 16F;
            this.colNguoiChoi2.HeaderText = "Người chơi 2";
            this.colNguoiChoi2.MinimumWidth = 6;
            this.colNguoiChoi2.Name = "colNguoiChoi2";
            this.colNguoiChoi2.ReadOnly = true;
            // 
            // colNhanVat2
            // 
            this.colNhanVat2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colNhanVat2.FillWeight = 15F;
            this.colNhanVat2.HeaderText = "Nhân vật 2";
            this.colNhanVat2.MinimumWidth = 6;
            this.colNhanVat2.Name = "colNhanVat2";
            this.colNhanVat2.ReadOnly = true;
            // 
            // colKetQua
            // 
            this.colKetQua.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colKetQua.FillWeight = 8F;
            this.colKetQua.HeaderText = "Kết quả";
            this.colKetQua.MinimumWidth = 6;
            this.colKetQua.Name = "colKetQua";
            this.colKetQua.ReadOnly = true;
            // 
            // colBatDau
            // 
            this.colBatDau.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colBatDau.FillWeight = 10F;
            this.colBatDau.HeaderText = "Bắt đầu";
            this.colBatDau.MinimumWidth = 6;
            this.colBatDau.Name = "colBatDau";
            this.colBatDau.ReadOnly = true;
            // 
            // colKetThuc
            // 
            this.colKetThuc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colKetThuc.FillWeight = 10F;
            this.colKetThuc.HeaderText = "Kết thúc";
            this.colKetThuc.MinimumWidth = 6;
            this.colKetThuc.Name = "colKetThuc";
            this.colKetThuc.ReadOnly = true;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.SaddleBrown;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.SandyBrown;
            this.label1.Location = new System.Drawing.Point(0, 80);
            this.label1.Margin = new System.Windows.Forms.Padding(0, 80, 0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(443, 98);
            this.label1.TabIndex = 1;
            this.label1.Text = "LỊCH SỬ ĐẤU";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frmMatchHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AntiqueWhite;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmMatchHistory";
            this.Padding = new System.Windows.Forms.Padding(20);
            this.Text = "frmMatchHistory";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMatchHistory_Load);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlDong.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLichSuDau)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridView dgvLichSuDau;
        private System.Windows.Forms.DataGridViewTextBoxColumn colID1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNguoiChoi1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNhanVat1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colID2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNguoiChoi2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNhanVat2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colKetQua;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBatDau;
        private System.Windows.Forms.DataGridViewTextBoxColumn colKetThuc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlDong;
        private System.Windows.Forms.Button btnDong;
        private Guna.UI2.WinForms.Guna2VScrollBar guna2VScrollBar1;
    }
}