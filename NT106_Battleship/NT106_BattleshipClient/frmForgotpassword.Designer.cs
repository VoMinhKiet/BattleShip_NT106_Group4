namespace NT106_BattleshipClient
{
    partial class frmForgotpassword
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmForgotpassword));
            this.pnlForgotpassword = new System.Windows.Forms.Panel();
            this.bntExit = new System.Windows.Forms.Button();
            this.lblor = new System.Windows.Forms.Label();
            this.linkLogin = new System.Windows.Forms.LinkLabel();
            this.linkCreateAccount = new System.Windows.Forms.LinkLabel();
            this.btnResetpassword = new System.Windows.Forms.Button();
            this.picEmail = new System.Windows.Forms.PictureBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblForgotpassword = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.pnlForgotpassword.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picEmail)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlForgotpassword
            // 
            this.pnlForgotpassword.BackColor = System.Drawing.Color.Bisque;
            this.pnlForgotpassword.Controls.Add(this.bntExit);
            this.pnlForgotpassword.Controls.Add(this.lblor);
            this.pnlForgotpassword.Controls.Add(this.linkLogin);
            this.pnlForgotpassword.Controls.Add(this.linkCreateAccount);
            this.pnlForgotpassword.Controls.Add(this.btnResetpassword);
            this.pnlForgotpassword.Controls.Add(this.picEmail);
            this.pnlForgotpassword.Controls.Add(this.lblEmail);
            this.pnlForgotpassword.Controls.Add(this.txtEmail);
            this.pnlForgotpassword.Controls.Add(this.lblForgotpassword);
            this.pnlForgotpassword.Location = new System.Drawing.Point(67, 60);
            this.pnlForgotpassword.Name = "pnlForgotpassword";
            this.pnlForgotpassword.Size = new System.Drawing.Size(705, 450);
            this.pnlForgotpassword.TabIndex = 0;
            // 
            // bntExit
            // 
            this.bntExit.BackColor = System.Drawing.Color.Red;
            this.bntExit.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bntExit.Location = new System.Drawing.Point(387, 241);
            this.bntExit.Name = "bntExit";
            this.bntExit.Size = new System.Drawing.Size(249, 67);
            this.bntExit.TabIndex = 8;
            this.bntExit.Text = "Thoát";
            this.bntExit.UseVisualStyleBackColor = false;
            this.bntExit.Click += new System.EventHandler(this.bntExit_Click);
            // 
            // lblor
            // 
            this.lblor.AutoSize = true;
            this.lblor.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblor.Location = new System.Drawing.Point(218, 337);
            this.lblor.Name = "lblor";
            this.lblor.Size = new System.Drawing.Size(29, 25);
            this.lblor.TabIndex = 7;
            this.lblor.Text = "or";
            // 
            // linkLogin
            // 
            this.linkLogin.AutoSize = true;
            this.linkLogin.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLogin.LinkColor = System.Drawing.Color.DimGray;
            this.linkLogin.Location = new System.Drawing.Point(162, 362);
            this.linkLogin.Name = "linkLogin";
            this.linkLogin.Size = new System.Drawing.Size(150, 25);
            this.linkLogin.TabIndex = 6;
            this.linkLogin.TabStop = true;
            this.linkLogin.Text = "Trở về đăng nhập";
            this.linkLogin.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLogin_LinkClicked);
            // 
            // linkCreateAccount
            // 
            this.linkCreateAccount.AutoSize = true;
            this.linkCreateAccount.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkCreateAccount.LinkColor = System.Drawing.Color.DimGray;
            this.linkCreateAccount.Location = new System.Drawing.Point(157, 312);
            this.linkCreateAccount.Name = "linkCreateAccount";
            this.linkCreateAccount.Size = new System.Drawing.Size(155, 25);
            this.linkCreateAccount.TabIndex = 5;
            this.linkCreateAccount.TabStop = true;
            this.linkCreateAccount.Text = "Tạo tài khoản mới";
            this.linkCreateAccount.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkCreateAccount_LinkClicked);
            // 
            // btnResetpassword
            // 
            this.btnResetpassword.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.btnResetpassword.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnResetpassword.Location = new System.Drawing.Point(110, 241);
            this.btnResetpassword.Name = "btnResetpassword";
            this.btnResetpassword.Size = new System.Drawing.Size(249, 68);
            this.btnResetpassword.TabIndex = 4;
            this.btnResetpassword.Text = "Tạo mật khẩu mới";
            this.btnResetpassword.UseVisualStyleBackColor = false;
            this.btnResetpassword.Click += new System.EventHandler(this.btnResetpassword_Click);
            // 
            // picEmail
            // 
            this.picEmail.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picEmail.BackgroundImage")));
            this.picEmail.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picEmail.Location = new System.Drawing.Point(43, 136);
            this.picEmail.Name = "picEmail";
            this.picEmail.Size = new System.Drawing.Size(61, 59);
            this.picEmail.TabIndex = 3;
            this.picEmail.TabStop = false;
            this.picEmail.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmail.Location = new System.Drawing.Point(105, 136);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(169, 25);
            this.lblEmail.TabIndex = 2;
            this.lblEmail.Text = "Nhập Email của bạn";
            // 
            // txtEmail
            // 
            this.txtEmail.BackColor = System.Drawing.SystemColors.Info;
            this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmail.Location = new System.Drawing.Point(110, 164);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(526, 31);
            this.txtEmail.TabIndex = 1;
            // 
            // lblForgotpassword
            // 
            this.lblForgotpassword.AutoSize = true;
            this.lblForgotpassword.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblForgotpassword.Location = new System.Drawing.Point(194, 22);
            this.lblForgotpassword.Name = "lblForgotpassword";
            this.lblForgotpassword.Size = new System.Drawing.Size(296, 54);
            this.lblForgotpassword.TabIndex = 0;
            this.lblForgotpassword.Text = "Quên mật khẩu";
            // 
            // frmForgotpassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1168, 666);
            this.Controls.Add(this.pnlForgotpassword);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmForgotpassword";
            this.Text = "frmForgotpassword";
            this.pnlForgotpassword.ResumeLayout(false);
            this.pnlForgotpassword.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picEmail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlForgotpassword;
        private System.Windows.Forms.Label lblForgotpassword;
        private System.Windows.Forms.PictureBox picEmail;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Button btnResetpassword;
        private System.Windows.Forms.Label lblor;
        private System.Windows.Forms.LinkLabel linkLogin;
        private System.Windows.Forms.LinkLabel linkCreateAccount;
        private System.Windows.Forms.Button bntExit;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}