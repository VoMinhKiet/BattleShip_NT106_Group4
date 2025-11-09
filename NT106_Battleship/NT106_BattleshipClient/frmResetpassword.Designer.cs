namespace NT106_BattleshipClient
{
    partial class frmResetpassword
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmResetpassword));
            this.pnlResetpassword = new System.Windows.Forms.Panel();
            this.lblor = new System.Windows.Forms.Label();
            this.linkCreateAccount = new System.Windows.Forms.LinkLabel();
            this.linkLogin = new System.Windows.Forms.LinkLabel();
            this.bntExit = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.chkShowpassword = new System.Windows.Forms.CheckBox();
            this.picConfirmpassword = new System.Windows.Forms.PictureBox();
            this.picNewpassword = new System.Windows.Forms.PictureBox();
            this.lblConfirmpassword = new System.Windows.Forms.Label();
            this.lblNewpassword = new System.Windows.Forms.Label();
            this.txtConfirmpassword = new System.Windows.Forms.TextBox();
            this.txtNewpassword = new System.Windows.Forms.TextBox();
            this.lblResetpassword = new System.Windows.Forms.Label();
            this.pnlResetpassword.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picConfirmpassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picNewpassword)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlResetpassword
            // 
            this.pnlResetpassword.BackColor = System.Drawing.Color.Bisque;
            this.pnlResetpassword.Controls.Add(this.lblor);
            this.pnlResetpassword.Controls.Add(this.linkCreateAccount);
            this.pnlResetpassword.Controls.Add(this.linkLogin);
            this.pnlResetpassword.Controls.Add(this.bntExit);
            this.pnlResetpassword.Controls.Add(this.btnReset);
            this.pnlResetpassword.Controls.Add(this.chkShowpassword);
            this.pnlResetpassword.Controls.Add(this.picConfirmpassword);
            this.pnlResetpassword.Controls.Add(this.picNewpassword);
            this.pnlResetpassword.Controls.Add(this.lblConfirmpassword);
            this.pnlResetpassword.Controls.Add(this.lblNewpassword);
            this.pnlResetpassword.Controls.Add(this.txtConfirmpassword);
            this.pnlResetpassword.Controls.Add(this.txtNewpassword);
            this.pnlResetpassword.Controls.Add(this.lblResetpassword);
            this.pnlResetpassword.Location = new System.Drawing.Point(60, 48);
            this.pnlResetpassword.Name = "pnlResetpassword";
            this.pnlResetpassword.Size = new System.Drawing.Size(618, 561);
            this.pnlResetpassword.TabIndex = 0;
            this.pnlResetpassword.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // lblor
            // 
            this.lblor.AutoSize = true;
            this.lblor.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblor.Location = new System.Drawing.Point(169, 435);
            this.lblor.Name = "lblor";
            this.lblor.Size = new System.Drawing.Size(29, 25);
            this.lblor.TabIndex = 12;
            this.lblor.Text = "or";
            // 
            // linkCreateAccount
            // 
            this.linkCreateAccount.AutoSize = true;
            this.linkCreateAccount.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkCreateAccount.LinkColor = System.Drawing.Color.DimGray;
            this.linkCreateAccount.Location = new System.Drawing.Point(113, 460);
            this.linkCreateAccount.Name = "linkCreateAccount";
            this.linkCreateAccount.Size = new System.Drawing.Size(155, 25);
            this.linkCreateAccount.TabIndex = 11;
            this.linkCreateAccount.TabStop = true;
            this.linkCreateAccount.Text = "Tạo tài khoản mới";
            this.linkCreateAccount.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkCreateAccount_LinkClicked);
            // 
            // linkLogin
            // 
            this.linkLogin.AutoSize = true;
            this.linkLogin.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLogin.LinkColor = System.Drawing.Color.DimGray;
            this.linkLogin.Location = new System.Drawing.Point(113, 410);
            this.linkLogin.Name = "linkLogin";
            this.linkLogin.Size = new System.Drawing.Size(150, 25);
            this.linkLogin.TabIndex = 10;
            this.linkLogin.TabStop = true;
            this.linkLogin.Text = "Trở về đăng nhập";
            this.linkLogin.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLogin_LinkClicked);
            // 
            // bntExit
            // 
            this.bntExit.BackColor = System.Drawing.Color.Red;
            this.bntExit.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bntExit.Location = new System.Drawing.Point(346, 332);
            this.bntExit.Name = "bntExit";
            this.bntExit.Size = new System.Drawing.Size(196, 76);
            this.bntExit.TabIndex = 9;
            this.bntExit.Text = "Thoát";
            this.bntExit.UseVisualStyleBackColor = false;
            this.bntExit.Click += new System.EventHandler(this.bntExit_Click);
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.btnReset.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReset.Location = new System.Drawing.Point(98, 332);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(196, 76);
            this.btnReset.TabIndex = 8;
            this.btnReset.Text = "Tạo ";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // chkShowpassword
            // 
            this.chkShowpassword.AutoSize = true;
            this.chkShowpassword.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkShowpassword.Location = new System.Drawing.Point(393, 274);
            this.chkShowpassword.Name = "chkShowpassword";
            this.chkShowpassword.Size = new System.Drawing.Size(149, 29);
            this.chkShowpassword.TabIndex = 7;
            this.chkShowpassword.Text = "Hiện mật khẩu";
            this.chkShowpassword.UseVisualStyleBackColor = true;
            this.chkShowpassword.CheckedChanged += new System.EventHandler(this.chkShowpassword_CheckedChanged);
            // 
            // picConfirmpassword
            // 
            this.picConfirmpassword.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picConfirmpassword.BackgroundImage")));
            this.picConfirmpassword.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picConfirmpassword.Location = new System.Drawing.Point(35, 209);
            this.picConfirmpassword.Name = "picConfirmpassword";
            this.picConfirmpassword.Size = new System.Drawing.Size(57, 59);
            this.picConfirmpassword.TabIndex = 6;
            this.picConfirmpassword.TabStop = false;
            // 
            // picNewpassword
            // 
            this.picNewpassword.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picNewpassword.BackgroundImage")));
            this.picNewpassword.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picNewpassword.Location = new System.Drawing.Point(35, 127);
            this.picNewpassword.Name = "picNewpassword";
            this.picNewpassword.Size = new System.Drawing.Size(57, 59);
            this.picNewpassword.TabIndex = 5;
            this.picNewpassword.TabStop = false;
            // 
            // lblConfirmpassword
            // 
            this.lblConfirmpassword.AutoSize = true;
            this.lblConfirmpassword.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConfirmpassword.Location = new System.Drawing.Point(93, 209);
            this.lblConfirmpassword.Name = "lblConfirmpassword";
            this.lblConfirmpassword.Size = new System.Drawing.Size(185, 25);
            this.lblConfirmpassword.TabIndex = 4;
            this.lblConfirmpassword.Text = "Xác nhận lại mật khẩu";
            // 
            // lblNewpassword
            // 
            this.lblNewpassword.AutoSize = true;
            this.lblNewpassword.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNewpassword.Location = new System.Drawing.Point(93, 127);
            this.lblNewpassword.Name = "lblNewpassword";
            this.lblNewpassword.Size = new System.Drawing.Size(170, 25);
            this.lblNewpassword.TabIndex = 3;
            this.lblNewpassword.Text = "Nhập mật khẩu mới";
            // 
            // txtConfirmpassword
            // 
            this.txtConfirmpassword.BackColor = System.Drawing.SystemColors.Info;
            this.txtConfirmpassword.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConfirmpassword.Location = new System.Drawing.Point(98, 237);
            this.txtConfirmpassword.Name = "txtConfirmpassword";
            this.txtConfirmpassword.Size = new System.Drawing.Size(444, 31);
            this.txtConfirmpassword.TabIndex = 2;
            // 
            // txtNewpassword
            // 
            this.txtNewpassword.BackColor = System.Drawing.SystemColors.Info;
            this.txtNewpassword.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNewpassword.Location = new System.Drawing.Point(98, 155);
            this.txtNewpassword.Name = "txtNewpassword";
            this.txtNewpassword.Size = new System.Drawing.Size(444, 31);
            this.txtNewpassword.TabIndex = 1;
            // 
            // lblResetpassword
            // 
            this.lblResetpassword.AutoSize = true;
            this.lblResetpassword.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResetpassword.Location = new System.Drawing.Point(118, 30);
            this.lblResetpassword.Name = "lblResetpassword";
            this.lblResetpassword.Size = new System.Drawing.Size(404, 54);
            this.lblResetpassword.TabIndex = 0;
            this.lblResetpassword.Text = "Tạo lại mật khẩu mới ";
            // 
            // frmResetpassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1083, 655);
            this.Controls.Add(this.pnlResetpassword);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmResetpassword";
            this.Text = "frmResetpassword";
            this.Load += new System.EventHandler(this.frmResetpassword_Load);
            this.pnlResetpassword.ResumeLayout(false);
            this.pnlResetpassword.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picConfirmpassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picNewpassword)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlResetpassword;
        private System.Windows.Forms.Label lblResetpassword;
        private System.Windows.Forms.Label lblConfirmpassword;
        private System.Windows.Forms.Label lblNewpassword;
        private System.Windows.Forms.TextBox txtConfirmpassword;
        private System.Windows.Forms.TextBox txtNewpassword;
        private System.Windows.Forms.CheckBox chkShowpassword;
        private System.Windows.Forms.PictureBox picConfirmpassword;
        private System.Windows.Forms.PictureBox picNewpassword;
        private System.Windows.Forms.Button bntExit;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Label lblor;
        private System.Windows.Forms.LinkLabel linkCreateAccount;
        private System.Windows.Forms.LinkLabel linkLogin;
    }
}