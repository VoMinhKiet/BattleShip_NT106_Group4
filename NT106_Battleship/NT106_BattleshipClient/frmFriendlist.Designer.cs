namespace NT106_BattleshipClient
{
    partial class frmFriendlist
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFriendlist));
            this.txtID = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblID = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.btnAddfriend = new System.Windows.Forms.Button();
            this.lblFriendlist = new System.Windows.Forms.Label();
            this.cbStatus = new System.Windows.Forms.ComboBox();
            this.btnFind = new System.Windows.Forms.Button();
            this.btnDeletefriend = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnInvite = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.lvFriendlist = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlFriendlist = new System.Windows.Forms.Panel();
            this.pnlFriendlist.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtID
            // 
            this.txtID.Location = new System.Drawing.Point(446, 100);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(140, 27);
            this.txtID.TabIndex = 27;
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(195, 77);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(75, 20);
            this.lblUsername.TabIndex = 28;
            this.lblUsername.Text = "Username";
            // 
            // lblID
            // 
            this.lblID.AutoSize = true;
            this.lblID.Location = new System.Drawing.Point(442, 77);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(24, 20);
            this.lblID.TabIndex = 29;
            this.lblID.Text = "ID";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(199, 100);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(241, 27);
            this.txtUsername.TabIndex = 26;
            // 
            // btnAddfriend
            // 
            this.btnAddfriend.Location = new System.Drawing.Point(707, 99);
            this.btnAddfriend.Name = "btnAddfriend";
            this.btnAddfriend.Size = new System.Drawing.Size(108, 28);
            this.btnAddfriend.TabIndex = 30;
            this.btnAddfriend.Text = "Add friend";
            this.btnAddfriend.UseVisualStyleBackColor = true;
            // 
            // lblFriendlist
            // 
            this.lblFriendlist.AutoSize = true;
            this.lblFriendlist.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFriendlist.Location = new System.Drawing.Point(327, 21);
            this.lblFriendlist.Name = "lblFriendlist";
            this.lblFriendlist.Size = new System.Drawing.Size(337, 41);
            this.lblFriendlist.TabIndex = 25;
            this.lblFriendlist.Text = "👥 DANH SÁCH BẠN BÈ";
            // 
            // cbStatus
            // 
            this.cbStatus.FormattingEnabled = true;
            this.cbStatus.Location = new System.Drawing.Point(199, 133);
            this.cbStatus.Name = "cbStatus";
            this.cbStatus.Size = new System.Drawing.Size(121, 28);
            this.cbStatus.TabIndex = 31;
            this.cbStatus.Text = "All";
            this.cbStatus.SelectedIndexChanged += new System.EventHandler(this.cbStatus_SelectedIndexChanged);
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(593, 99);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(108, 28);
            this.btnFind.TabIndex = 32;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            // 
            // btnDeletefriend
            // 
            this.btnDeletefriend.Location = new System.Drawing.Point(707, 133);
            this.btnDeletefriend.Name = "btnDeletefriend";
            this.btnDeletefriend.Size = new System.Drawing.Size(108, 28);
            this.btnDeletefriend.TabIndex = 33;
            this.btnDeletefriend.Text = "Delete friend";
            this.btnDeletefriend.UseVisualStyleBackColor = true;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Tomato;
            this.btnExit.Location = new System.Drawing.Point(199, 432);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(96, 39);
            this.btnExit.TabIndex = 34;
            this.btnExit.Text = "Back";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnInvite
            // 
            this.btnInvite.Location = new System.Drawing.Point(593, 133);
            this.btnInvite.Name = "btnInvite";
            this.btnInvite.Size = new System.Drawing.Size(108, 27);
            this.btnInvite.TabIndex = 35;
            this.btnInvite.Text = "Invite ";
            this.btnInvite.UseVisualStyleBackColor = true;
            // 
            // btnView
            // 
            this.btnView.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnView.Location = new System.Drawing.Point(719, 433);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(96, 38);
            this.btnView.TabIndex = 36;
            this.btnView.Text = "View";
            this.btnView.UseVisualStyleBackColor = false;
            // 
            // lvFriendlist
            // 
            this.lvFriendlist.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvFriendlist.BackColor = System.Drawing.SystemColors.Info;
            this.lvFriendlist.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.lvFriendlist.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lvFriendlist.FullRowSelect = true;
            this.lvFriendlist.HideSelection = false;
            this.lvFriendlist.Location = new System.Drawing.Point(199, 166);
            this.lvFriendlist.Name = "lvFriendlist";
            this.lvFriendlist.Size = new System.Drawing.Size(615, 259);
            this.lvFriendlist.TabIndex = 37;
            this.lvFriendlist.UseCompatibleStateImageBehavior = false;
            this.lvFriendlist.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "ID";
            this.columnHeader1.Width = 120;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Username";
            this.columnHeader2.Width = 220;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Rank";
            this.columnHeader3.Width = 137;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Status";
            this.columnHeader4.Width = 135;
            // 
            // pnlFriendlist
            // 
            this.pnlFriendlist.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.pnlFriendlist.Controls.Add(this.lvFriendlist);
            this.pnlFriendlist.Controls.Add(this.btnView);
            this.pnlFriendlist.Controls.Add(this.btnInvite);
            this.pnlFriendlist.Controls.Add(this.btnExit);
            this.pnlFriendlist.Controls.Add(this.btnDeletefriend);
            this.pnlFriendlist.Controls.Add(this.btnFind);
            this.pnlFriendlist.Controls.Add(this.cbStatus);
            this.pnlFriendlist.Controls.Add(this.lblFriendlist);
            this.pnlFriendlist.Controls.Add(this.btnAddfriend);
            this.pnlFriendlist.Controls.Add(this.txtUsername);
            this.pnlFriendlist.Controls.Add(this.lblID);
            this.pnlFriendlist.Controls.Add(this.lblUsername);
            this.pnlFriendlist.Controls.Add(this.txtID);
            this.pnlFriendlist.Location = new System.Drawing.Point(31, 37);
            this.pnlFriendlist.Name = "pnlFriendlist";
            this.pnlFriendlist.Size = new System.Drawing.Size(1010, 517);
            this.pnlFriendlist.TabIndex = 0;
            this.pnlFriendlist.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlFriendlist_Paint);
            // 
            // frmFriendlist
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1071, 585);
            this.Controls.Add(this.pnlFriendlist);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmFriendlist";
            this.Text = "frmFriendlist";
            this.Load += new System.EventHandler(this.frmFriendlist_Load);
            this.pnlFriendlist.ResumeLayout(false);
            this.pnlFriendlist.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtID;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblID;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Button btnAddfriend;
        private System.Windows.Forms.Label lblFriendlist;
        private System.Windows.Forms.ComboBox cbStatus;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.Button btnDeletefriend;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnInvite;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.ListView lvFriendlist;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Panel pnlFriendlist;
    }
}