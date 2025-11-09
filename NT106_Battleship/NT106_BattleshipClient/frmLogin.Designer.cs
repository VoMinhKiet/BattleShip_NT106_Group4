namespace NT106_BattleshipClient
{
    partial class frmLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogin));
            this.pnlLogin = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.picPassword = new System.Windows.Forms.PictureBox();
            this.picUser = new System.Windows.Forms.PictureBox();
            this.linkForgotPassword = new System.Windows.Forms.LinkLabel();
            this.linkCreateAccount = new System.Windows.Forms.LinkLabel();
            this.btnLogin = new System.Windows.Forms.Button();
            this.chkShowpassword = new System.Windows.Forms.CheckBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblLogin = new System.Windows.Forms.Label();
            this.pnlLogin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picUser)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlLogin
            // 
            this.pnlLogin.BackColor = System.Drawing.Color.Bisque;
            this.pnlLogin.Controls.Add(this.btnExit);
            this.pnlLogin.Controls.Add(this.picPassword);
            this.pnlLogin.Controls.Add(this.picUser);
            this.pnlLogin.Controls.Add(this.linkForgotPassword);
            this.pnlLogin.Controls.Add(this.linkCreateAccount);
            this.pnlLogin.Controls.Add(this.btnLogin);
            this.pnlLogin.Controls.Add(this.chkShowpassword);
            this.pnlLogin.Controls.Add(this.lblPassword);
            this.pnlLogin.Controls.Add(this.lblUsername);
            this.pnlLogin.Controls.Add(this.txtPassword);
            this.pnlLogin.Controls.Add(this.txtUsername);
            this.pnlLogin.Controls.Add(this.lblLogin);
            this.pnlLogin.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlLogin.Location = new System.Drawing.Point(57, 42);
            this.pnlLogin.Name = "pnlLogin";
            this.pnlLogin.Size = new System.Drawing.Size(707, 578);
            this.pnlLogin.TabIndex = 0;
            this.pnlLogin.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlLogin_Paint);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Tomato;
            this.btnExit.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Location = new System.Drawing.Point(370, 391);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(210, 60);
            this.btnExit.TabIndex = 11;
            this.btnExit.Text = "Thoát";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // picPassword
            // 
            this.picPassword.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picPassword.BackgroundImage")));
            this.picPassword.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picPassword.Location = new System.Drawing.Point(48, 199);
            this.picPassword.Name = "picPassword";
            this.picPassword.Size = new System.Drawing.Size(59, 59);
            this.picPassword.TabIndex = 10;
            this.picPassword.TabStop = false;
            // 
            // picUser
            // 
            this.picUser.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picUser.BackgroundImage")));
            this.picUser.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picUser.Location = new System.Drawing.Point(48, 120);
            this.picUser.Name = "picUser";
            this.picUser.Size = new System.Drawing.Size(59, 59);
            this.picUser.TabIndex = 9;
            this.picUser.TabStop = false;
            // 
            // linkForgotPassword
            // 
            this.linkForgotPassword.AutoSize = true;
            this.linkForgotPassword.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkForgotPassword.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkForgotPassword.LinkColor = System.Drawing.Color.DimGray;
            this.linkForgotPassword.Location = new System.Drawing.Point(409, 454);
            this.linkForgotPassword.Name = "linkForgotPassword";
            this.linkForgotPassword.Size = new System.Drawing.Size(134, 25);
            this.linkForgotPassword.TabIndex = 8;
            this.linkForgotPassword.TabStop = true;
            this.linkForgotPassword.Text = "Quên mật khẩu";
            this.linkForgotPassword.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkForgotPassword_LinkClicked);
            // 
            // linkCreateAccount
            // 
            this.linkCreateAccount.AutoSize = true;
            this.linkCreateAccount.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkCreateAccount.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkCreateAccount.LinkColor = System.Drawing.Color.DimGray;
            this.linkCreateAccount.Location = new System.Drawing.Point(143, 454);
            this.linkCreateAccount.Name = "linkCreateAccount";
            this.linkCreateAccount.Size = new System.Drawing.Size(155, 25);
            this.linkCreateAccount.TabIndex = 7;
            this.linkCreateAccount.TabStop = true;
            this.linkCreateAccount.Text = "Tạo tài khoản mới";
            this.linkCreateAccount.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkCreateAccount_LinkClicked);
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.btnLogin.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogin.Location = new System.Drawing.Point(113, 391);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(210, 60);
            this.btnLogin.TabIndex = 6;
            this.btnLogin.Text = "Đăng nhập";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // chkShowpassword
            // 
            this.chkShowpassword.AutoSize = true;
            this.chkShowpassword.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkShowpassword.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkShowpassword.Location = new System.Drawing.Point(431, 264);
            this.chkShowpassword.Name = "chkShowpassword";
            this.chkShowpassword.Size = new System.Drawing.Size(149, 29);
            this.chkShowpassword.TabIndex = 5;
            this.chkShowpassword.Text = "Hiện mật khẩu";
            this.chkShowpassword.UseVisualStyleBackColor = true;
            this.chkShowpassword.CheckedChanged += new System.EventHandler(this.chkShowpassword_CheckedChanged);
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPassword.Location = new System.Drawing.Point(108, 199);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(86, 25);
            this.lblPassword.TabIndex = 4;
            this.lblPassword.Text = "Mật khẩu";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsername.Location = new System.Drawing.Point(108, 120);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(137, 25);
            this.lblUsername.TabIndex = 3;
            this.lblUsername.Text = "Tên người dùng";
            this.lblUsername.Click += new System.EventHandler(this.label2_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.SystemColors.Info;
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.Location = new System.Drawing.Point(113, 227);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(467, 31);
            this.txtPassword.TabIndex = 2;
            // 
            // txtUsername
            // 
            this.txtUsername.BackColor = System.Drawing.SystemColors.Info;
            this.txtUsername.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsername.Location = new System.Drawing.Point(113, 148);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(467, 31);
            this.txtUsername.TabIndex = 1;
            // 
            // lblLogin
            // 
            this.lblLogin.AutoSize = true;
            this.lblLogin.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogin.Location = new System.Drawing.Point(229, 30);
            this.lblLogin.Name = "lblLogin";
            this.lblLogin.Size = new System.Drawing.Size(219, 54);
            this.lblLogin.TabIndex = 0;
            this.lblLogin.Text = "Đăng nhập";
            // 
            // frmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1177, 660);
            this.Controls.Add(this.pnlLogin);
            this.Name = "frmLogin";
            this.Text = "frmLogin";
            this.Load += new System.EventHandler(this.frmLogin_Load);
            this.pnlLogin.ResumeLayout(false);
            this.pnlLogin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picUser)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlLogin;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblLogin;
        private System.Windows.Forms.LinkLabel linkForgotPassword;
        private System.Windows.Forms.LinkLabel linkCreateAccount;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.CheckBox chkShowpassword;
        private System.Windows.Forms.PictureBox picPassword;
        private System.Windows.Forms.PictureBox picUser;
        private System.Windows.Forms.Button btnExit;
    }
}