namespace NT106_BattleshipClient
{
    partial class frmOTP
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOTP));
            this.lbltitleOTP = new System.Windows.Forms.Label();
            this.lblOTP = new System.Windows.Forms.Label();
            this.txtOTP = new System.Windows.Forms.TextBox();
            this.btnVeriOTP = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.picOTP = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picOTP)).BeginInit();
            this.SuspendLayout();
            // 
            // lbltitleOTP
            // 
            this.lbltitleOTP.AutoSize = true;
            this.lbltitleOTP.BackColor = System.Drawing.Color.SkyBlue;
            this.lbltitleOTP.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbltitleOTP.Location = new System.Drawing.Point(235, 20);
            this.lbltitleOTP.Name = "lbltitleOTP";
            this.lbltitleOTP.Size = new System.Drawing.Size(344, 54);
            this.lbltitleOTP.TabIndex = 0;
            this.lbltitleOTP.Text = "Xác thực mã OTP";
            // 
            // lblOTP
            // 
            this.lblOTP.AutoSize = true;
            this.lblOTP.BackColor = System.Drawing.SystemColors.HighlightText;
            this.lblOTP.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOTP.Location = new System.Drawing.Point(120, 104);
            this.lblOTP.Name = "lblOTP";
            this.lblOTP.Size = new System.Drawing.Size(209, 28);
            this.lblOTP.TabIndex = 1;
            this.lblOTP.Text = "Nhập mã xác thực OTP";
            // 
            // txtOTP
            // 
            this.txtOTP.BackColor = System.Drawing.SystemColors.HighlightText;
            this.txtOTP.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOTP.Location = new System.Drawing.Point(125, 135);
            this.txtOTP.Name = "txtOTP";
            this.txtOTP.Size = new System.Drawing.Size(576, 34);
            this.txtOTP.TabIndex = 2;
            // 
            // btnVeriOTP
            // 
            this.btnVeriOTP.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.btnVeriOTP.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVeriOTP.Location = new System.Drawing.Point(125, 200);
            this.btnVeriOTP.Name = "btnVeriOTP";
            this.btnVeriOTP.Size = new System.Drawing.Size(168, 62);
            this.btnVeriOTP.TabIndex = 3;
            this.btnVeriOTP.Text = "Xác thực";
            this.btnVeriOTP.UseVisualStyleBackColor = false;
            this.btnVeriOTP.Click += new System.EventHandler(this.btnVeriOTP_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Red;
            this.btnExit.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Location = new System.Drawing.Point(533, 200);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(168, 62);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "Thoát";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // picOTP
            // 
            this.picOTP.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picOTP.BackgroundImage")));
            this.picOTP.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picOTP.Location = new System.Drawing.Point(54, 104);
            this.picOTP.Name = "picOTP";
            this.picOTP.Size = new System.Drawing.Size(65, 65);
            this.picOTP.TabIndex = 5;
            this.picOTP.TabStop = false;
            // 
            // frmOTP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(808, 399);
            this.Controls.Add(this.picOTP);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnVeriOTP);
            this.Controls.Add(this.txtOTP);
            this.Controls.Add(this.lblOTP);
            this.Controls.Add(this.lbltitleOTP);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmOTP";
            this.Text = "frmOTP";
            ((System.ComponentModel.ISupportInitialize)(this.picOTP)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbltitleOTP;
        private System.Windows.Forms.Label lblOTP;
        private System.Windows.Forms.TextBox txtOTP;
        private System.Windows.Forms.Button btnVeriOTP;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.PictureBox picOTP;
    }
}