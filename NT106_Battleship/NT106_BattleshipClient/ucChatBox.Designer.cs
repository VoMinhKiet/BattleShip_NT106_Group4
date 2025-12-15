namespace NT106_BattleshipClient
{
    partial class ucChatBox
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucChatBox));
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.pnlNhapLieu = new System.Windows.Forms.Panel();
            this.txtTinNhan = new System.Windows.Forms.TextBox();
            this.btnGui = new System.Windows.Forms.Button();
            this.rtbLichSuTinNhan = new System.Windows.Forms.RichTextBox();
            this.pnlNhapLieu.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlNhapLieu
            // 
            this.pnlNhapLieu.BackColor = System.Drawing.Color.DimGray;
            this.pnlNhapLieu.Controls.Add(this.txtTinNhan);
            this.pnlNhapLieu.Controls.Add(this.btnGui);
            this.pnlNhapLieu.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlNhapLieu.Location = new System.Drawing.Point(0, 348);
            this.pnlNhapLieu.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnlNhapLieu.Name = "pnlNhapLieu";
            this.pnlNhapLieu.Size = new System.Drawing.Size(625, 81);
            this.pnlNhapLieu.TabIndex = 0;
            // 
            // txtTinNhan
            // 
            this.txtTinNhan.BackColor = System.Drawing.Color.AliceBlue;
            this.txtTinNhan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTinNhan.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTinNhan.Location = new System.Drawing.Point(13, 2);
            this.txtTinNhan.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtTinNhan.Multiline = true;
            this.txtTinNhan.Name = "txtTinNhan";
            this.txtTinNhan.Size = new System.Drawing.Size(469, 79);
            this.txtTinNhan.TabIndex = 1;
            // 
            // btnGui
            // 
            this.btnGui.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGui.BackColor = System.Drawing.Color.Bisque;
            this.btnGui.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGui.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnGui.Location = new System.Drawing.Point(516, 17);
            this.btnGui.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnGui.Name = "btnGui";
            this.btnGui.Size = new System.Drawing.Size(101, 45);
            this.btnGui.TabIndex = 0;
            this.btnGui.Text = "Gửi";
            this.btnGui.UseVisualStyleBackColor = false;
            this.btnGui.Click += new System.EventHandler(this.btnGui_Click);
            // 
            // rtbLichSuTinNhan
            // 
            this.rtbLichSuTinNhan.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.rtbLichSuTinNhan.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbLichSuTinNhan.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.rtbLichSuTinNhan.Location = new System.Drawing.Point(20, 18);
            this.rtbLichSuTinNhan.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rtbLichSuTinNhan.Name = "rtbLichSuTinNhan";
            this.rtbLichSuTinNhan.ReadOnly = true;
            this.rtbLichSuTinNhan.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbLichSuTinNhan.Size = new System.Drawing.Size(572, 304);
            this.rtbLichSuTinNhan.TabIndex = 1;
            this.rtbLichSuTinNhan.Text = "";
            // 
            // ucChatBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.rtbLichSuTinNhan);
            this.Controls.Add(this.pnlNhapLieu);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "ucChatBox";
            this.Size = new System.Drawing.Size(625, 429);
            this.pnlNhapLieu.ResumeLayout(false);
            this.pnlNhapLieu.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Panel pnlNhapLieu;
        private System.Windows.Forms.TextBox txtTinNhan;
        private System.Windows.Forms.Button btnGui;
        private System.Windows.Forms.RichTextBox rtbLichSuTinNhan;
    }
}
